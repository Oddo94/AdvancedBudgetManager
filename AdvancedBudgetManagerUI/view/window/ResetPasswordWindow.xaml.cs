using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.utils.security;
using AdvancedBudgetManagerCore.view_model;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResetPasswordWindow : Window, IRecipient<GenericResultMessage> {
        private TaskCompletionSource<bool> taskCompletionSource = new();
        private ResetPasswordViewModel resetPasswordViewModel;
        private SecureString secureString;

        public ResetPasswordWindow(ResetPasswordViewModel resetPasswordViewModel) {
            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(500, 500));

            this.resetPasswordViewModel = resetPasswordViewModel;
            this.InitializeComponent();
            this.Closed += (_, _) => taskCompletionSource.TrySetResult(false);

            WeakReferenceMessenger.Default.Register<GenericResultMessage>(this);
        }

        public Task<bool> ShowModalAsync(Window parentWindow) {
            WinRT.Interop.WindowNative.GetWindowHandle(parentWindow);


            this.Activate();
            return taskCompletionSource.Task;
        }

        public async void ResetPasswordButton_Click(object sender, RoutedEventArgs e) {
            //Debug.WriteLine("Inside ResetPasswordButton_Click");

            //try {
            //    //resetPasswordViewModel.ResetPassword();

            //    ContentDialog passwordResetSuccessDialog = new ContentDialog {
            //        Title = "Password reset",
            //        Content = "Your password was successfully reset!",
            //        CloseButtonText = "OK",
            //        XamlRoot = this.Content.XamlRoot
            //    };
            //    await passwordResetSuccessDialog.ShowAsync();

            //    this.Close();

            //} catch (SystemException ex) {
            //    ContentDialog passwordResetErrorDialog = new ContentDialog {
            //        Title = "Reset password",
            //        Content = ex.Message,
            //        CloseButtonText = "OK",
            //        XamlRoot = this.Content.XamlRoot
            //    };
            //    await passwordResetErrorDialog.ShowAsync();
            //}

            //taskCompletionSource.TrySetResult(true);   

            PasswordSecurityManager passwordSecurityManager = new PasswordSecurityManager();
            passwordSecurityManager.ToSecureString("sgdfasgfhgsjf");
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e) {
            taskCompletionSource.TrySetResult(false);
            this.Close();
        }

        public async void Receive(GenericResultMessage message) {
            string title = "Password reset";

            ContentDialog passwordResetContentDialog = new ContentDialog {
                Title = "Password reset",
                Content = message.Message,
                PrimaryButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            ContentDialogResult displayResult = await passwordResetContentDialog.ShowAsync();

            if (displayResult == ContentDialogResult.Primary && message.IsSuccess) {
                Debug.WriteLine("Closing the password reset window...");
                this.Close();
            }

            //message.Reply(true);

        }
    }
}
