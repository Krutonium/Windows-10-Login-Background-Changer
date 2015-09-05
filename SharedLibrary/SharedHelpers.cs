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
            var loaded = File.ReadAllBytes(path).LongLength;

            var f = new FileInfo(path);
            long actual = (int)f.Length;

            return new FileSizeMetaInformation(actual, loaded);
        }

        public static string ByteSize(long size)
        {
            Debug.Assert(SizeSuffixes.Length > 0);


            if (size == 0)
            {
                return $"{null}{0:0.#} {SizeSuffixes[0]}";
            }

            var absSize = Math.Abs((double)size);
            var fpPower = Math.Log(absSize, 1000);
            var intPower = (int)fpPower;
            var iUnit = intPower >= SizeSuffixes.Length
                ? SizeSuffixes.Length - 1
                : intPower;
            var normSize = absSize / Math.Pow(1000, iUnit);

            return $"{(size < 0 ? "-" : null)}{normSize:0.#} {SizeSuffixes[iUnit]}";
        }
    }
}