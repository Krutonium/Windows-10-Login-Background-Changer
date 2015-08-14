using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace MessageBoxLibrary
{
    public static class WindowHelper
    {
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                   int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hwnd, uint msg,
                   IntPtr wParam, IntPtr lParam);

        const int GwlExstyle = -20;
        const int WsExDlgmodalframe = 0x0001;
        const int WsExRight = 0x00001000;
        const int WsExRtlreading = 0x00002000;

        const int SwpNosize = 0x0001;
        const int SwpNomove = 0x0002;
        const int SwpNozorder = 0x0004;
        const int SwpFramechanged = 0x0020;
        const uint WmSeticon = 0x0080;

        public static void RemoveIcon(Window window)
        {
            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to not show a window icon
            int extendedStyle = GetWindowLong(hwnd, GwlExstyle);
            SetWindowLong(hwnd, GwlExstyle, extendedStyle | WsExDlgmodalframe);

            // Update the window's non-client area to reflect the changes
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SwpNomove |
                  SwpNosize | SwpNozorder | SwpFramechanged);
        }

        public static void SetRightAligned(Window window)
        {
            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to not show a window icon
            int extendedStyle = GetWindowLong(hwnd, GwlExstyle);
            SetWindowLong(hwnd, GwlExstyle, extendedStyle | WsExRight);

            // Update the window's non-client area to reflect the changes
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SwpNomove |
                  SwpNosize | SwpNozorder | SwpFramechanged);
        }

        public static void SetRtlReading(Window window)
        {
            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to not show a window icon
            int extendedStyle = GetWindowLong(hwnd, GwlExstyle);
            SetWindowLong(hwnd, GwlExstyle, extendedStyle | WsExRtlreading);

            // Update the window's non-client area to reflect the changes
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SwpNomove |
                  SwpNosize | SwpNozorder | SwpFramechanged);
        }

    }
}
