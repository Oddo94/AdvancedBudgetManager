using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfirmEmailWindow : Window {
        public ConfirmEmailWindow() {
            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(500, 500));

            if (appWindow.Presenter is OverlappedPresenter appWindowPresenter) {
                appWindowPresenter.IsResizable = false;
            }

            this.InitializeComponent();
            //this.ResetPasswordContentFrame.Navigate(typeof(EmailInputPage));
        }

        public void ConfirmEmailButton_Click(object sender, RoutedEventArgs e) {
            //string emailAddress = EmailAddressTextBox.Text;

            ResetPasswordWindow resetPasswordWindow = new ResetPasswordWindow();
            resetPasswordWindow.Activate();


            //MailAddress mailAddress = new MailAddress(emailAddress);
        }
    }
}
