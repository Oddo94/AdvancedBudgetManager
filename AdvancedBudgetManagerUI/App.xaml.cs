using AdvancedBudgetManager.utils.misc;
using AdvancedBudgetManager.view.dialog;
using AdvancedBudgetManager.view.window;
using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.model.message;
using AdvancedBudgetManagerCore.repository;
using AdvancedBudgetManagerCore.service;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
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

                container.RegisterType<ConfirmEmailWindow>()
                         .WithParameter(
                            (pi, ctx) => pi.ParameterType == typeof(EmailConfirmationViewModel),
                            (pi, ctx) => ctx.ResolveKeyed<EmailConfirmationViewModel>("PasswordResetEmailConfirmationVM"))
                         .OnActivated(e => {
                             Window window = (Window)e.Instance;
                             IWindowProvider windowProvider = e.Context.Resolve<IWindowProvider>();

                             windowProvider.Register(window);
                         })
                         .Keyed<Window>(WindowKey.CONFIRM_EMAIL_WINDOW);

                container.RegisterType<ResetPasswordWindow>()
                         .Keyed<Window>(WindowKey.RESET_PASSWORD_WINDOW);

                container.RegisterType<RegisterUserWindow>()
                         .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(EmailConfirmationViewModel),
                               (pi, ctx) => ctx.ResolveKeyed<EmailConfirmationViewModel>("UserRegistrationEmailConfirmationVM"))
                         .Keyed<Window>(WindowKey.REGISTER_USER_WINDOW);

                //InputDialogs
                container.RegisterType<ConfirmationCodeInputDialog>();

                //NavigationServices
                container.RegisterType<WindowNavigationService>()
                    .As<IWindowNavigationService>()
                    .SingleInstance();

                //ViewModels
                container.RegisterType<LoginViewModel>()
                         .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(LoginUserService),
                               (pi, ctx) => ctx.ResolveKeyed<LoginUserService>("LoginUserService")
                );

                /*Since the constructor of SharedPropertiesViewModelWrapper needs two EmailConfirmationViewModel the way in which they
                they are injected will be determined by their parameter name*/
                container.RegisterType<SharedPropertiesViewModelWrapper>()
                        .WithParameter(
                               (pi, ctx) => pi.Name == "userRegistrationEmailConfirmationVM",
                               (pi, ctx) => ctx.ResolveKeyed<EmailConfirmationViewModel>("UserRegistrationEmailConfirmationVM"))
                        .WithParameter(
                              (pi, ctx) => pi.Name == "passwordResetEmailConfirmationVM",
                              (pi, ctx) => ctx.ResolveKeyed<EmailConfirmationViewModel>("PasswordResetEmailConfirmationVM")
                 );

                container.RegisterType<RegisterUserViewModel>()
                         .SingleInstance()
                         .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(RegisterUserService),
                                (pi, ctx) => ctx.ResolveKeyed<RegisterUserService>("RegisterUserService")
                    );

                container.RegisterType<ResetPasswordViewModel>()
                     .SingleInstance()
                     .WithParameter(
                           (pi, ctx) => pi.ParameterType == typeof(ResetPasswordService),
                           (pi, ctx) => ctx.ResolveKeyed<ResetPasswordService>("ResetPasswordService")

            );

                //Registers object with default constructor
                container.RegisterType<EmailConfirmationViewModel>()
                         .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(IConfirmationNotifier),
                                (pi, ctx) => ctx.ResolveKeyed<IConfirmationNotifier>("UserRegistrationNotifier"))
                         .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(EmailService),
                                (pi, ctx) => ctx.ResolveKeyed<EmailService>("EmailService"))
                         .Keyed<EmailConfirmationViewModel>("UserRegistrationEmailConfirmationVM");

                container.RegisterType<EmailConfirmationViewModel>()
                         .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(IConfirmationNotifier),
                                (pi, ctx) => ctx.ResolveKeyed<IConfirmationNotifier>("PasswordResetNotifier"))
                         .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(EmailService),
                                (pi, ctx) => ctx.ResolveKeyed<EmailService>("EmailService"))
                         .Keyed<EmailConfirmationViewModel>("PasswordResetEmailConfirmationVM");

                //Single instances
                //1.Notifiers
                container.RegisterType<UserRegistrationEmailConfirmationNotifier>()
                       .SingleInstance()
                       .Keyed<IConfirmationNotifier>("UserRegistrationNotifier");

                container.RegisterType<PasswordResetEmailConfirmationNotifier>()
                       .SingleInstance()
                       .Keyed<IConfirmationNotifier>("PasswordResetNotifier");

                //2.Providers
                container.RegisterType<WindowProvider>()
                         .As<IWindowProvider>()
                         .SingleInstance();


                //Services
                container.RegisterType<LoginUserService>()
                .WithParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IUserRepository),
                    (pi, ctx) => ctx.ResolveKeyed<IUserRepository>("UserRepo"))
                .Keyed<LoginUserService>("LoginUserService");

                container.RegisterType<RegisterUserService>()
                         .WithParameter(
                                 (pi, ctx) => pi.ParameterType == typeof(IUserRepository),
                                 (pi, ctx) => ctx.ResolveKeyed<IUserRepository>("UserRepo"))
                         .Keyed<RegisterUserService>("RegisterUserService");

                container.RegisterType<ResetPasswordService>()
                        .WithParameter(
                                (pi, ctx) => pi.ParameterType == typeof(IUserRepository),
                                (pi, ctx) => ctx.ResolveKeyed<IUserRepository>("UserRepo"))
                        .Keyed<ResetPasswordService>("ResetPasswordService");

                container.RegisterType<EmailService>()
                         .AsSelf()
                         .SingleInstance()
                         .Keyed<EmailService>("EmailService");

                container.RegisterType<ErrorService>()
                        .As<IErrorService>();


                //Repositories
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
                //Window confirmEmailWindow = Container
                //.ResolveKeyed<Window>(WindowKey.CONFIRM_EMAIL_WINDOW);

                /*Sets the BaseWindowXamlRoot property of the ConfirmEmailWindow to the XamlRoot of the LoginWindow
                This allows the display of the password reset dialog on top of the login window*/
                //confirmEmailWindow.BaseWindowXamlRoot = loginWindowRoot;
            }
        }
    }
}
