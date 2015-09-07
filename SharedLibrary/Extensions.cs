using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

namespace SharedLibrary
{
    public static class Extensions
    {
        public static Image ResizeImage(this Image imgToResize, System.Drawing.Size size) => (Image)(new Bitmap(imgToResize, size));

        public static MColor ToMediaColor(this DColor color) => MColor.FromArgb(color.A, color.R, color.G, color.B);

        public static BitmapSource ToBitmapSource(this Image source)
        {
            var bitmap = new Bitmap(source);

            var bitSrc = bitmap.ToBitmapSource();

            bitmap.Dispose();

            return bitSrc;
        }

        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            BitmapSource bitSrc;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitSrc;
        }

        public static bool IsImage(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            var jpg = new List<string> { "FF", "D8" };
            var bmp = new List<string> { "42", "4D" };
            var gif = new List<string> { "47", "49", "46" };
            var png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            var imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            var bytesIterated = new List<string>();

            for (var i = 0; i < 8; i++)
            {
                var bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                var isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }

        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }

        public static bool IsHex(this string hex) => (Regex.Match(hex, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success);

        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                var value = kvp.Value as IDictionary<string, object>;
                if (value != null)
                {
                    var expandoValue = value.ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else
                {
                    var items = kvp.Value as ICollection;
                    if (items != null)
                    {
                        // iterate through the collection and convert any strin-object dictionaries
                        // along the way into expando objects
                        var itemList = new List<object>();
                        foreach (var item in items)
                        {
                            var objects = item as IDictionary<string, object>;
                            if (objects != null)
                            {
                                var expandoItem = objects.ToExpando();
                                itemList.Add(expandoItem);
                            }
                            else
                            {
                                itemList.Add(item);
                            }
                        }

                        expandoDic.Add(kvp.Key, itemList);
                    }
                    else
                    {
                        expandoDic.Add(kvp);
                    }
                }
            }
            return expando;
        }
    }
}