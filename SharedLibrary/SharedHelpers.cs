using SharedLibrary.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace SharedLibrary
{
    public static class SharedHelpers
    {
        private static readonly string[] SizeSuffixes =
        {
            "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        };

        public static FileSizeMetaInformation GetFileSize(string path)
        {
            long loaded = File.ReadAllBytes(path).LongLength;

            FileInfo f = new FileInfo(path);
            long actual = (int)f.Length;

            return new FileSizeMetaInformation(actual, loaded);
        }

        public static string ByteSize(long size)
        {
            Debug.Assert(SizeSuffixes.Length > 0);

            const string formatTemplate = "{0}{1:0.#} {2}";

            if (size == 0)
            {
                return string.Format(formatTemplate, null, 0, SizeSuffixes[0]);
            }

            var absSize = Math.Abs((double)size);
            var fpPower = Math.Log(absSize, 1000);
            var intPower = (int)fpPower;
            var iUnit = intPower >= SizeSuffixes.Length
                ? SizeSuffixes.Length - 1
                : intPower;
            var normSize = absSize / Math.Pow(1000, iUnit);

            return string.Format(
                formatTemplate,
                size < 0 ? "-" : null, normSize, SizeSuffixes[iUnit]);
        }
    }
}