using AdvancedBudgetManagerCore.utils.security;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Security;

namespace AdvancedBudgetManager.utils {
    /// <summary>
    /// Helper class that provides attached properties for PasswordBox objects. 
    /// They are used for binding the password to a <see cref="SecureString"/> object whose value is automatically sent to the view model class.
    /// </summary>
    public class PasswordBoxHelper {
        /// <summary>
        /// BoundPassword property declaration.
        /// </summary>
        public static readonly DependencyProperty BoundPasswordProperty =
           DependencyProperty.RegisterAttached(
               "BoundPassword",
               typeof(SecureString),
               typeof(PasswordBoxHelper),
               new PropertyMetadata(null));

        /// <summary>
        /// Retrieves the bound password as a <see cref="SecureString"/> object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A <see cref="SecureString"/> object</returns>
        public static SecureString GetBoundPassword(DependencyObject obj) =>
            (SecureString)obj.GetValue(BoundPasswordProperty);

        /// <summary>
        /// Sets the bound password as a <see cref="SecureString"/> object.
        /// </summary>
        /// <param name="obj">The object whose value is set.</param>
        /// <param name="value">The actual value which is set, represented as a <see cref="SecureString"/> object.</param>
        public static void SetBoundPassword(DependencyObject obj, SecureString value) =>
            obj.SetValue(BoundPasswordProperty, value);

        /// <summary>
        /// BindPassword property declaration.
        /// </summary>
        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false, OnBindPasswordChanged));

        /// <summary>
        /// Retrieves the bind password property as a <see cref="bool"/> value.
        /// </summary>
        /// <param name="obj">The object which contains the value</param>
        /// <returns>The <see cref="bool"/> value of the property.</returns>
        public static bool GetBindPassword(DependencyObject obj) =>
            (bool)obj.GetValue(BindPasswordProperty);

        /// <summary>
        /// Sets the bind password property.
        /// </summary>
        /// <param name="obj">The object whose value is set.</param>
        /// <param name="value">The actual value which is set, represented as a <see cref="bool"/> value.</param>
        public static void SetBindPassword(DependencyObject obj, bool value) =>
            obj.SetValue(BindPasswordProperty, value);

        /// <summary>
        /// Adds or removes the event handler when the PasswordBox content changes.
        /// </summary>
        /// <param name="d">The dependency object, in this case the PasswordBox.</param>
        /// <param name="e">The event triggered when the password value is changed.</param>
        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is PasswordBox passwordBox) {
                if ((bool)e.NewValue) {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                } else {
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                }
            }
        }

        /// <summary>
        /// Event handler method which is invoked when the PasswordBox value is changed.
        /// </summary>
        /// <param name="sender">The source object, in this case the PasswordBox.</param>
        /// <param name="e">The event which contains the updated data.</param>
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
            if (sender is PasswordBox passwordBox) {
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
        //public static event EventHandler? PasswordChanged;
    }
}
