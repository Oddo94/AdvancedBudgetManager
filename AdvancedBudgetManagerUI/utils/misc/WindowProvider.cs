using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedBudgetManager.utils.misc {
    public class WindowProvider : IWindowProvider {

        private readonly List<Window> windows;
        private Window? activeWindow;

        public WindowProvider() {
            this.windows = new List<Window>();
        }

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

        public Window GetActiveWindow() {
            return activeWindow ?? throw new InvalidOperationException("No active window is available."); ;
        }

        public XamlRoot GetActiveXamlRoot() {
            return GetActiveWindow().Content.XamlRoot;
        }
    }
}
