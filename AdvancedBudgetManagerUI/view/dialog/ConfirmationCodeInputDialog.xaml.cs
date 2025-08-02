using AdvancedBudgetManagerCore.view_model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.dialog {
    public sealed partial class ConfirmationCodeInputDialog : ContentDialog {

        private EmailConfirmationViewModel emailConfirmationViewModel;
        private bool showErrorTipOnLoad;
        private bool isValidConfirmationCode;

        public ConfirmationCodeInputDialog([NotNull] EmailConfirmationViewModel emailConfirmationViewModel) {
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.Loaded += ConfirmationCodeInputDialog_Loaded;
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            bool confirmationCodesMatch = emailConfirmationViewModel.ConfirmationCodesMatch(emailConfirmationViewModel.InputConfirmationCode, emailConfirmationViewModel.GeneratedConfirmationCode);


            if (!confirmationCodesMatch) {
                await Task.Delay(50);
                ConfirmationCodeInputDialog onFailedValidationDialog = new ConfirmationCodeInputDialog(this.emailConfirmationViewModel) {
                    XamlRoot = this.XamlRoot,
                    ShowErrorTipOnLoad = true
                };

                isValidConfirmationCode = false;

                await onFailedValidationDialog.ShowAsync();
            } else {
                isValidConfirmationCode = true;
            }

                
        }

        private void ConfirmationCodeInputDialog_Loaded(object sender, RoutedEventArgs args) {
            if (showErrorTipOnLoad) {
                InvalidConfirmationCodeTip.IsOpen = true;
            }
        }

        public bool ShowErrorTipOnLoad {
            get { return this.showErrorTipOnLoad; }
            set { this.showErrorTipOnLoad = value; }
        }

        public bool IsValidConfirmationCode {
            get { return this.isValidConfirmationCode; }
            set { this.isValidConfirmationCode = value; }
        }
    }
}
