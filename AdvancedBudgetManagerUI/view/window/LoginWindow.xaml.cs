using AdvancedBudgetManagerCore.model.response;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.view_model;
using AdvancedBudgetManagerUI.view.window;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window {
        private LoginViewModel loginViewModel;
        private UserDashboard userDashboard;
        private ConfirmEmailWindow confirmEmailWindow;

        public LoginWindow(LoginViewModel loginViewModel, UserDashboard userDashboard, ConfirmEmailWindow confirmEmailWindow) {
            this.loginViewModel = loginViewModel;
            this.userDashboard = userDashboard;
            this.confirmEmailWindow = confirmEmailWindow;

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
                loginViewModel.CheckCredentials();

                LoginResponse loginResponse = loginViewModel.LoginResponse;
                //LoginResponse loginResponse = new LoginResponse(ResultCode.OK, "User successfully logged in");//ONLY FOR TESTING PURPOSES!!
                if (loginResponse.ResultCode == ResultCode.OK) {
                    this.Close();
                    userDashboard.Activate();
                } else {
                    loginErrorDialog = new ContentDialog {
                        Title = "Login",
                        Content = loginResponse.ResponseMessage,
                        CloseButtonText = "OK"
                    };

                    loginErrorDialog.XamlRoot = this.Content.XamlRoot;
                    await loginErrorDialog.ShowAsync();
                }
            } catch (SystemException ex) {
                loginErrorDialog = new ContentDialog {
                    Title = "Login",
                    Content = ex.Message,
                    CloseButtonText = "OK"
                };

                loginErrorDialog.XamlRoot = this.Content.XamlRoot;
                await loginErrorDialog.ShowAsync();
            }
        }

        public void ResetLink_Click(object sender, RoutedEventArgs e) {
            //ConfirmEmailWindow confirmEmailWindow = new ConfirmEmailWindow();
            confirmEmailWindow.Activate();
        }
    }
}
