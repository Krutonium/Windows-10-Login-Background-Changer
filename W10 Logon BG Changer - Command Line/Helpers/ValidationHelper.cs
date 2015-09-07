using System;
using System.Collections.Generic;
using System.IO;
using SharedLibrary;

namespace W10_Logon_BG_Changer___Command_Line.Helpers
{
    internal class ValidationHelper
    {
        public static void ValidateImageArgument(string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("Usage: /i pathtoimage");

            var filePath = args[1];

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"An error occurred: The file \"{filePath}\" does not exist!");

            var validExtensions = new List<string> { ".bmp", ".jpg", ".png", ".tif", ".tiff" };
            var fileExtension = Path.GetExtension(filePath);

            if (!validExtensions.Contains(fileExtension))
                throw new ArgumentException($"An error occurred: The file \"{filePath}\" is not a valid file!");
        }

        public static void ValidateColorArgument(string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("Usage: /c hexcode (Example: /c #000000)");

            if (!args[1].IsHex())
                throw new ArgumentException("Not a valid Hex color!");
        }

        public static void ValidateFirstArgument(string[] args)
        {
            const string message = "Please use '/h' for help";

            if (args.Length <= 0)
                throw new ArgumentException(message);

            var firstArgument = args[0].ToLower();

            if (!(firstArgument.StartsWith("/") || firstArgument.StartsWith("-")))
                throw new ArgumentException(message);
        }
    }
}
