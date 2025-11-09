using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.model.request;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.view_model;
using CommunityToolkit.Mvvm.Messaging;
using NSubstitute;

namespace AdvancedBudgetManagerCoreTest.password_reset {
    [TestClass]
    public class PasswordResetTests {
        private static string userEmail = String.Empty;

        public TestContext TestContext { get; set; }


        [ClassInitialize]
        public static void SetupTestData(TestContext testContext) {
            userEmail = testContext.Properties["userEmail"]?.ToString() ?? String.Empty;
        }


        [TestMethod]
        public void ResetPassword_WhenException_SendFailureMessage() {
            IDataUpdateRequest resetPasswordRequest = Substitute.For<IDataUpdateRequest>();
            ICrudRepository resetPasswordRepository = Substitute.For<ICrudRepository>();
            string expectedMessage = "Failed to reset your password. Please try again!";

            //Argument matchers are required so that the exception will be thrown regardless of the IDataUpdateRequest instance passed to the UpdateData method
            resetPasswordRepository
                .When(x => x.UpdateData(Arg.Any<IDataUpdateRequest>()))  
                .Do(_ => { throw new SystemException(expectedMessage); });

            GenericResultMessage? actualMessage = null;
            WeakReferenceMessenger.Default.Register<GenericResultMessage>(this, (r, m) => { actualMessage = m; });

            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel(resetPasswordRepository);
            resetPasswordViewModel.ResetPassword(userEmail);


            Assert.AreEqual(expectedMessage, actualMessage!.Message);
        }

        [TestMethod]
        public void ResetPassword_WhenSuccessfulReset_SendSuccessMessage() {
            IDataUpdateRequest resetPasswordRequest = Substitute.For<IDataUpdateRequest>();
            ICrudRepository resetPasswordRepository = Substitute.For<ICrudRepository>();
            string expectedMessage = "Your password was successfully reset!";

            resetPasswordRepository
                .When(x => x.UpdateData(Arg.Any<IDataUpdateRequest>()))
                .Do(_ => { });

            GenericResultMessage? actualMessage = null;
            WeakReferenceMessenger.Default.Register<GenericResultMessage>(this, (r, m) => actualMessage = m);
            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel(resetPasswordRepository);
            resetPasswordViewModel.ResetPassword(userEmail);

            Assert.AreEqual(expectedMessage, actualMessage?.Message);
        }
    }
}
