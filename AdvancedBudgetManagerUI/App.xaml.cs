using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManager.view.window;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.view_model;
using AdvancedBudgetManagerUI.view.window;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using AutofacContainer = Autofac.IContainer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AdvancedBudgetManager {
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application {
        public static AutofacContainer? Container { get; private set; }

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
                //Single instances
                container.RegisterType<UserRegistrationEmailConfirmationNotifier>()
                       .SingleInstance()
                       .Keyed<IConfirmationNotifier>("UserRegistrationNotifier");

                //Windows
                container.RegisterType<LoginWindow>()
                         .SingleInstance();
                container.RegisterType<UserDashboard>()
                         .SingleInstance();
                container.RegisterType<ConfirmEmailWindow>()
                         .SingleInstance();
                container.RegisterType<ResetPasswordWindow>()
                         .SingleInstance();
                container.RegisterType<RegisterUserWindow>()
                         .SingleInstance()
                         .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(EmailConfirmationViewModel),
                               (pi, ctx) => ctx.ResolveKeyed<EmailConfirmationViewModel>("UserRegistrationEmailConfirmationVM")
                );

                //InputDialogs
                container.RegisterType<ConfirmationCodeInputDialog>()
                         .SingleInstance();

                //ViewModels
                //FIX AFTER IMPLEMENTING THE USER REGISTRATION SYSTEM!!
                container.RegisterType<LoginViewModel>()
                         .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(ICrudRepository<User, long>),
                               (pi, ctx) => ctx.ResolveKeyed<ICrudRepository<User, long>>("UserLoginRepo")
                );

                container.RegisterType<SharedPropertiesViewModelWrapper>();

                //FIX AFTER IMPLEMENTING THE USER REGISTRATION SYSTEM!!
                container.RegisterType<ResetPasswordViewModel>()
                         .SingleInstance()
                         .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(IUserRepository),
                               (pi, ctx) => ctx.ResolveKeyed<IUserRepository>("ResetPasswordRepo")

                );

                container.RegisterType<RegisterUserViewModel>()
                         .SingleInstance()
                         .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(RegisterUserService),
                                (pi, ctx) => ctx.ResolveKeyed<RegisterUserService>("RegisterUserService")
                    );

                //Registers object with default constructor
                container.RegisterType<EmailConfirmationViewModel>()
                         .AsSelf()
                         .SingleInstance()
                         .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(IConfirmationNotifier),
                                (pi, ctx) => ctx.ResolveKeyed<IConfirmationNotifier>("UserRegistrationNotifier"))
                         .Keyed<EmailConfirmationViewModel>("UserRegistrationEmailConfirmationVM");


                //Services
                container.RegisterType<RegisterUserService>()
                         .WithParameter(
                                 (pi, ctx) => pi.ParameterType == typeof(IUserRepository),
                                 (pi, ctx) => ctx.ResolveKeyed<IUserRepository>("UserRepo"))
                         .Keyed<RegisterUserService>("RegisterUserService");


                //Repositories
                //FIX AFTER IMPLEMENTING THE USER REGISTRATION SYSTEM!!
                container.RegisterType<UserLoginRepository>()
                         .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(IDatabaseConnection),
                               (pi, ctx) => ctx.ResolveKeyed<IDatabaseConnection>("MySqlDbConnection"))
                         .Keyed<ICrudRepository<User, long>>("UserLoginRepo");

                container.RegisterType<ResetPasswordRepository>()
                        .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(IDatabaseConnection),
                               (pi, ctx) => ctx.ResolveKeyed<IDatabaseConnection>("MySqlDbConnection"))
                        .Keyed<IUserRepository>("ResetPasswordRepo");

                container.RegisterType<UserRepository>()
                         .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(IDatabaseConnection),
                               (pi, ctx) => ctx.ResolveKeyed<IDatabaseConnection>("MySqlDbConnection"))
                         .Keyed<IUserRepository>("UserRepo");

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
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            if (Container == null) {
                throw new InvalidOperationException("An error occurred when trying to setup the required objects.");
            }

            loginWindow = App.Container.Resolve<LoginWindow>();

            if (loginWindow == null) {
                throw new InvalidOperationException("Unable to initialize the login window!");
            } else {
                loginWindow.Activate();

                FrameworkElement rootElement = (FrameworkElement)loginWindow.Content;

                if (!rootElement.IsLoaded) {
                    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                    rootElement.Loaded += (s, e) => tcs.SetResult(null!);

                    await tcs.Task;
                }

                XamlRoot loginWindowRoot = loginWindow.Content.XamlRoot;

                //Retrieves the ConfirmEmailWindow object from the DI container
                ConfirmEmailWindow confirmEmailWindow = Container.Resolve<ConfirmEmailWindow>();

                /*Sets the BaseWindowXamlRoot property of the ConfirmEmailWindow to the XamlRoot of the LoginWindow
                This allows the display of the password reset dialog on top of the login window*/
                confirmEmailWindow.BaseWindowXamlRoot = loginWindowRoot;
            }
        }
    }
}
