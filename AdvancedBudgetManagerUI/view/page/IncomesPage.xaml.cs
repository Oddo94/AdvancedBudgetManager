using AdvancedBudgetManager.view_model;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics.CodeAnalysis;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager.view.page {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IncomesPage : Page {
        private IncomesViewModel incomesViewModel;
        public IncomesPage([NotNull] IncomesViewModel incomesViewModel) {
            this.incomesViewModel = incomesViewModel;
            InitializeComponent();
        }
    }
}
