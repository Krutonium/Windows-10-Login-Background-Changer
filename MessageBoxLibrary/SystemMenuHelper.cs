using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;

namespace MessageBoxLibrary
{
    public class SystemMenuHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        private static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint MF_ENABLED = 0x00000000;

        private const uint SC_SIZE = 0xF000;
        private const uint SC_RESTORE = 0xF120;
        private const uint SC_MINIMIZE = 0xF020;
        private const uint SC_MAXIMIZE = 0xF030;
        private const uint SC_CLOSE = 0xF060;
        
        private const int WM_SHOWWINDOW = 0x00000018;
        private const int WM_CLOSE = 0x10;

        private HwndSource _hwndSource;

        public SystemMenuHelper(Window window)
        {
            AddHook(window);
        }

        public bool DisableCloseButton { get; set; }

        public bool RemoveResizeMenu { get; set; }

        private void AddHook(Window window)
        {
            if (_hwndSource == null)
            {
                _hwndSource = PresentationSource.FromVisual(window) as HwndSource;
                if (_hwndSource != null)
                {
                    _hwndSource.AddHook(new HwndSourceHook(hwndSourceHook));
                }
            }
        }

        private IntPtr hwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SHOWWINDOW)
            {
                IntPtr hMenu = GetSystemMenu(hwnd, false);
                if (hMenu != IntPtr.Zero)
                {
                    // handle disabling the close button and system menu item
                    if (DisableCloseButton)
                    {
                        EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                    }

                    // handles removing the resize items from the system menu
                    if (RemoveResizeMenu)
                    {
                        RemoveMenu(hMenu, SC_RESTORE, MF_BYCOMMAND);
                        RemoveMenu(hMenu, SC_SIZE, MF_BYCOMMAND);
                        RemoveMenu(hMenu, SC_MINIMIZE, MF_BYCOMMAND);
                        RemoveMenu(hMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                    }
                }
            }

            return IntPtr.Zero;
        }
    }
}
