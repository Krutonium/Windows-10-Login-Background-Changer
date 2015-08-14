using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace MessageBoxLibrary
{
    public class SystemMenuHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIdEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        private const uint MfBycommand = 0x00000000;
        private const uint MfGrayed = 0x00000001;
        private const uint MfEnabled = 0x00000000;

        private const uint ScSize = 0xF000;
        private const uint ScRestore = 0xF120;
        private const uint ScMinimize = 0xF020;
        private const uint ScMaximize = 0xF030;
        private const uint ScClose = 0xF060;
        
        private const int WmShowwindow = 0x00000018;
        private const int WmClose = 0x10;

        private HwndSource _hwndSource;

        public SystemMenuHelper(Window window)
        {
            AddHook(window);
        }

        public bool DisableCloseButton { get; set; }

        public bool RemoveResizeMenu { get; set; }

        private void AddHook(Visual window)
        {
            if (_hwndSource != null) return;
            _hwndSource = PresentationSource.FromVisual(window) as HwndSource;
            if (_hwndSource != null)
            {
                _hwndSource.AddHook(HwndSourceHook);
            }
        }

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != WmShowwindow) return IntPtr.Zero;
            var hMenu = GetSystemMenu(hwnd, false);
            if (hMenu == IntPtr.Zero) return IntPtr.Zero;
            // handle disabling the close button and system menu item
            if (DisableCloseButton)
            {
                EnableMenuItem(hMenu, ScClose, MfBycommand | MfGrayed);
            }

            // handles removing the resize items from the system menu
            if (RemoveResizeMenu)
            {
                RemoveMenu(hMenu, ScRestore, MfBycommand);
                RemoveMenu(hMenu, ScSize, MfBycommand);
                RemoveMenu(hMenu, ScMinimize, MfBycommand);
                RemoveMenu(hMenu, ScMaximize, MfBycommand);
            }

            return IntPtr.Zero;
        }
    }
}
