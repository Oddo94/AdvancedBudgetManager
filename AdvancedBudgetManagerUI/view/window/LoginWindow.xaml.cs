using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.view_model;
using AdvancedBudgetManagerUI.view.window;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window {
    /// <summary>
    /// The user login window.
    /// </summary>
    public sealed partial class LoginWindow : Window {
        private LoginViewModel loginViewModel;
        private UserDashboard userDashboard;
        //private ConfirmEmailWindow confirmEmailWindow;
        private IWindowNavigationService windowNavigationService;
        //private RegisterUserWindow registerUserWindow;

        public LoginWindow([NotNull] LoginViewModel loginViewModel,
            [NotNull] UserDashboard userDashboard,
            //[NotNull] ConfirmEmailWindow confirmEmailWindow,
            //[NotNull] RegisterUserWindow registerUserWindow)
            [NotNull] IWindowNavigationService windowNavigationService) {
            this.loginViewModel = loginViewModel;
            this.userDashboard = userDashboard;
            //this.confirmEmailWindow = confirmEmailWindow;
            //this.registerUserWindow = registerUserWindow;
            this.windowNavigationService = windowNavigationService;

            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(600, 600));

            if (appWindow.Presenter is OverlappedPresenter appWindowPresenter) {
                Debug.WriteLine($"isResizable state before change: {appWindowPresenter.IsResizable}");

                appWindowPresenter.IsResizable = false;

                Debug.WriteLine($"isResizable state after change: {appWindowPresenter.IsResizable}");
            }

            this.InitializeComponent();
        }

        public async void LoginButton_Click(object sender, RoutedEventArgs e) {
            ContentDialog loginErrorDialog;

            try {
                loginViewModel.LoginUser();
                PasswordBox.Password = String.Empty;

                GenericResponse loginResponse = loginViewModel.loginResponse;
                if (loginResponse.ResultCode == ResultCode.OK) {
                    this.Close();
                    userDashboard.Activate();
                } else {
                    loginErrorDialog = new ContentDialog {
                        Title = "Login",
                        Content = loginResponse.ResponseMessage,
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };

                    await loginErrorDialog.ShowAsync();
                }
            } catch (SystemException ex) {
                loginErrorDialog = new ContentDialog {
                    Title = "Login",
                    Content = ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };

                await loginErrorDialog.ShowAsync();
            }
        }

        public void ResetLink_Click(object sender, RoutedEventArgs e) {
            windowNavigationService.Show(WindowKey.CONFIRM_EMAIL_WINDOW);
        }

        public void RegisterUser_Click(object sender, RoutedEventArgs e) {
            windowNavigationService.Show(WindowKey.REGISTER_USER_WINDOW);
        }
    }
}
