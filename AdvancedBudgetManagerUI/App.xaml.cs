using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManager.view.window;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.view_model;
using AdvancedBudgetManagerUI.view.window;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using AutofacContainer = Autofac.IContainer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager {
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application {
        public static AutofacContainer Container { get; private set; }

        private Window? loginWindow;

        //public static IHost? AppHost { get; private set; }



        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            this.InitializeComponent();

            //TO DO: change dependency injection container to Autofac before continuing the implementation of password reset feature
            //AppHost = Host.CreateDefaultBuilder()
            //    .ConfigureServices((context, services) => {
            //        services.AddTransient<UserDashboard>();
            //        services.AddTransient<LoginViewModel>();
            //        //services.AddSingleton<ICrudRepository, EmailConfirmationRepository>();
            //        services.AddSingleton<ICrudRepository, ResetPasswordRepository>();
            //        services.AddSingleton<ResetPasswordDialog>();
            //        services.AddSingleton<ResetPasswordViewModel>();
            //        services.AddSingleton<EmailConfirmationViewModel>();
            //        services.AddSingleton<ConfirmationCodeInputDialog>();
            //        services.AddSingleton<ConfirmEmailWindow>();
            //        services.AddTransient<LoginWindow>();
            //        services.AddSingleton<IDatabaseConnection, MySqlDatabaseConnection>();
            //        services.AddSingleton<ICrudRepository, UserLoginRepository>();                                     
            //    })
            //    .Build();


            IHost serviceProvider = ConfigureServices();
            Container = (AutofacContainer)serviceProvider.Services.GetAutofacRoot();

        }

        private IHost ConfigureServices() {
            IHostBuilder builder = Host.CreateDefaultBuilder();
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.ConfigureContainer<ContainerBuilder>(container => {
                //Windows
                container.RegisterType<LoginWindow>();
                container.RegisterType<UserDashboard>();
                container.RegisterType<ConfirmEmailWindow>();

                //InputDialogs
                container.RegisterType<ConfirmationCodeInputDialog>();
                container.RegisterType<ResetPasswordDialog>();

                //ViewModels
                container.RegisterType<LoginViewModel>()
 .WithParameter(
     (pi, ctx) => pi.ParameterType == typeof(ICrudRepository),
     (pi, ctx) => ctx.ResolveKeyed<ICrudRepository>("UserLoginRepo")
 );
                container.RegisterType<ResetPasswordViewModel>()
         .WithParameter(
             (pi, ctx) => pi.ParameterType == typeof(ICrudRepository),
             (pi, ctx) => ctx.ResolveKeyed<ICrudRepository>("ResetPasswordRepo")
         );



                container.RegisterType<EmailConfirmationViewModel>();

                //Repositories
                container.RegisterType<UserLoginRepository>()
    .WithParameter(
    (pi, ctx) => pi.ParameterType == typeof(IDatabaseConnection),
    (pi, ctx) => ctx.ResolveKeyed<IDatabaseConnection>("MySqlDbConnection"))
    .Keyed<ICrudRepository>("UserLoginRepo");

                container.RegisterType<ResetPasswordRepository>()
                     .WithParameter(
(pi, ctx) => pi.ParameterType == typeof(IDatabaseConnection),
(pi, ctx) => ctx.ResolveKeyed<IDatabaseConnection>("MySqlDbConnection"))
.Keyed<ICrudRepository>("ResetPasswordRepo");

                //Database
                container.RegisterType<MySqlDatabaseConnection>()
                         .Keyed<IDatabaseConnection>("MySqlDbConnection");

            }
            );

            return builder.Build();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            //if (AppHost == null) {
            //    throw new InvalidOperationException("An error occurred when trying to setup the required objects.");
            //}

            //loginWindow = AppHost.Services.GetService<LoginWindow>();

            //if (loginWindow == null) {
            //    throw new InvalidOperationException("Unable to initialize the login window!");
            //} else {
            //   loginWindow.Activate();
            //}

            if (Container == null) {
                throw new InvalidOperationException("An error occurred when trying to setup the required objects.");
            }

            loginWindow = App.Container.Resolve<LoginWindow>();

            if (loginWindow == null) {
                throw new InvalidOperationException("Unable to initialize the login window!");
            } else {
                loginWindow.Activate();
            }
        }
    }
}
