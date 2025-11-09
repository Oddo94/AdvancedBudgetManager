using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.page {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmailInputPage : Page {
        //ResetPasswordWindow parentWindow;
        public EmailInputPage() {
            //this.parentWindow = parentWindow;
            this.InitializeComponent();
        }


        public void SendResetPasswordEmailButton_Click(object sender, RoutedEventArgs e) {
            //string emailAddress = EmailAddressTextBox.Text;

            //MailAddress mailAddress = new MailAddress(emailAddress);      
        }
    }
}
