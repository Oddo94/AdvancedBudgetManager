using AdvancedBudgetManager.view.window;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.view_model;
using AdvancedBudgetManagerUI.view.window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using System.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? loginWindow;

        public static IHost? AppHost { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddTransient<UserDashboard>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<LoginWindow>();
                    services.AddSingleton<IDatabaseConnection, MySqlDatabaseConnection>();
                    services.AddSingleton<ICrudRepository, UserLoginRepository>();

                })
                .Build();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //m_window = new UserDashboard();
            //m_window.Activate();

            if (AppHost == null) {
                throw new InvalidOperationException("An error occurred when trying to setup the required objects.");
            }

            loginWindow = AppHost.Services.GetService<LoginWindow>();

            if (loginWindow == null) {
                throw new InvalidOperationException("Unable to initialize the login window!");
            } else {
                loginWindow.Activate();
            }
        }

        //private Window? m_window;
    }
}
