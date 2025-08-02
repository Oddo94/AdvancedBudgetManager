using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.window
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResetPasswordWindow : Window {
        private TaskCompletionSource<bool> taskCompletionSource = new();
        public ResetPasswordWindow() {
            AppWindow appWindow = this.AppWindow;
            appWindow.Resize(new Windows.Graphics.SizeInt32(500, 500));
            
            this.InitializeComponent();
            this.Closed += (_, _) => taskCompletionSource.TrySetResult(false);
        }

        public Task<bool> ShowModalAsync(Window parentWindow) {
            WinRT.Interop.WindowNative.GetWindowHandle(parentWindow);


            this.Activate();
            return taskCompletionSource.Task;
        }

        public void ResetPasswordButton_Click(object sender, RoutedEventArgs e) {
            taskCompletionSource.TrySetResult(true);
            this.Close();
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e) {
            taskCompletionSource.TrySetResult(false);
            this.Close();
        }
    }
}
