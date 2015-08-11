using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace MessageBoxLibrary
{
    internal static class IconUtilities 
    { 
        [DllImport("gdi32.dll", SetLastError = true)]     
        private static extern bool DeleteObject(IntPtr hObject); 
        
        public static ImageSource ToImageSource(this Icon icon) 
        { 
            Bitmap bitmap = icon.ToBitmap(); 
            IntPtr hBitmap = bitmap.GetHbitmap(); 
            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); 
            if (!DeleteObject(hBitmap)) 
            { 
                throw new Win32Exception(); 
            } 
            return wpfBitmap; 
        }
    } 
}
