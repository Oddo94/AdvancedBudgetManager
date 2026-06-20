using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedBudgetManager.utils.misc {
    public class DialogService {
        private readonly SemaphoreSlim currentLock = new(1, 1);

        private XamlRoot xamlRoot;

        public DialogService() { }
        public DialogService(XamlRoot xamlRoot) {
            this.xamlRoot = xamlRoot;
        }

        public async Task ShowAsync(string message, string title) {
            await currentLock.WaitAsync();

            try {
                ContentDialog dialog = new ContentDialog {
                    Title = title,
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.xamlRoot
                };

                await dialog.ShowAsync();
                await Task.Delay(250);
            } catch (Exception ex) {
                Debug.WriteLine("DIALOG SERVICE ERROR. REASON: " + ex);
                throw;

            } finally {
                currentLock.Release();
            }
        }

        public XamlRoot XamlRoot {
            set { this.xamlRoot = value; }
        }
    }
}
