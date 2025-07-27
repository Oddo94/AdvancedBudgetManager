using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using AdvancedBudgetManagerCore.view_model;
using System.Diagnostics.CodeAnalysis;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.dialog
{
    public sealed partial class ConfirmationCodeInputDialog : ContentDialog {
        
        
        private EmailConfirmationViewModel emailConfirmationViewModel;
        
        public ConfirmationCodeInputDialog([NotNull] EmailConfirmationViewModel emailConfirmationViewModel) {
            this.emailConfirmationViewModel = emailConfirmationViewModel;
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            bool confirmationCodesMatch = emailConfirmationViewModel.ConfirmationCodesMatch(emailConfirmationViewModel.InputConfirmationCode, emailConfirmationViewModel.GeneratedConfirmationCode);
            
            if(!confirmationCodesMatch) {
                ContentDialog errorDialog = new ContentDialog() {
                    Title = "Error",
                    Content = "Invalid confirmation code. Please try again!",
                    CloseButtonText = "OK"
                };
            }

            //Add success logic
        }
    }
}
