using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedBudgetManager.utils.misc {

    /// <summary>
    /// Class that provides a system for keeping track of the currently active window.
    /// </summary>
    public class WindowProvider : IWindowProvider {
        /// <summary>
        /// The windows list.
        /// </summary>
        private readonly List<Window> windows;

        /// <summary>
        /// The currently active window.
        /// </summary>
        private Window? activeWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowProvider"/> class.
        /// </summary>
        public WindowProvider() {
            this.windows = new List<Window>();
        }

        /// <inheritdoc/>
        public void Register(Window window) {
            //Prevents duplicate window registration
            if (windows.Contains(window)) {
                return;
            }

            windows.Add(window);

            //Sets active window on activation
            window.Activated += (_, args) => {
                if (args.WindowActivationState != WindowActivationState.Deactivated) {
                    activeWindow = window;
                }
            };

            //Cleanup on window close
            window.Closed += (_, _) => {
                windows.Remove(window);
                if (activeWindow == window) {
                    activeWindow = windows.LastOrDefault();
                }
            };
        }

        /// <inheritdoc/>
        public Window GetActiveWindow() {
            return activeWindow ?? throw new InvalidOperationException("No active window is available."); ;
        }

        /// <inheritdoc/>
        public XamlRoot GetActiveXamlRoot() {
            return GetActiveWindow().Content.XamlRoot;
        }
    }
}
