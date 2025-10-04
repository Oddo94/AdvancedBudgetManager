using AdvancedBudgetManagerCore.utils.security;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedBudgetManager.utils {
    public class PasswordBoxHelper {
        public static readonly DependencyProperty BoundPasswordProperty =
           DependencyProperty.RegisterAttached(
               "BoundPassword",
               typeof(SecureString),
               typeof(PasswordBoxHelper),
               new PropertyMetadata(null));

        public static SecureString GetBoundPassword(DependencyObject obj) =>
            (SecureString)obj.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject obj, SecureString value) =>
            obj.SetValue(BoundPasswordProperty, value);

        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false, OnBindPasswordChanged));

        public static bool GetBindPassword(DependencyObject obj) =>
            (bool)obj.GetValue(BindPasswordProperty);

        public static void SetBindPassword(DependencyObject obj, bool value) =>
            obj.SetValue(BindPasswordProperty, value);

        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is PasswordBox passwordBox) {
                if ((bool)e.NewValue) {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                } else {
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
            if (sender is PasswordBox passwordBox) {
                //var securePassword = new SecureString();
                //foreach (char c in passwordBox.Password) {
                //    securePassword.AppendChar(c);
                //}
                //securePassword.MakeReadOnly();

                //Check to avoid the password conversion to SecureString when the password box is reset for security reasons
                if (passwordBox.Password != String.Empty) {
                    PasswordSecurityManager securityManager = new PasswordSecurityManager();
                    SecureString securePassword = securityManager.ToSecureString(passwordBox.Password);

                    SetBoundPassword(passwordBox, securePassword);
                }

                // Validation hook: raise event or callback if needed
//                PasswordChanged?.Invoke(passwordBox, EventArgs.Empty);
            }
        }

        // Optional event to allow external subscribers (e.g., for validation)
        public static event EventHandler? PasswordChanged;
    }
}
