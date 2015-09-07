using System;
using System.Collections.Generic;
using System.IO;
using SharedLibrary;

namespace W10_Logon_BG_Changer___Command_Line.Helpers
{
    internal class ValidationHelper
    {
        public void ValidateImageArgument(string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("Usage: /i pathtoimage");

            string filePath = args[1];

            if (!File.Exists(filePath))
                throw new FileNotFoundException(string.Format("An error occurred: The file \"{0}\" does not exist!", filePath));

            var validExtensions = new List<string> { ".bmp", ".jpg", ".png", ".tif", ".tiff" };
            string fileExtension = Path.GetExtension(filePath);

            if (!validExtensions.Contains(fileExtension))
                throw new ArgumentException(string.Format("An error occurred: The file \"{0}\" is not a valid file!", filePath));
        }

        public void ValidateColorArgument(string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException("Usage: /c hexcode (Example: /c #000000)");

            if (!args[1].IsHex())
                throw new ArgumentException("Not a valid Hex color!");
        }

        public void ValidateFirstArgument(string[] args)
        {
            const string message = "Please use '/h' for help";

            if (args.Length <= 0)
                throw new ArgumentException(message);

            string firstArgument = args[0].ToLower();

            if (!(firstArgument.StartsWith("/") || firstArgument.StartsWith("-")))
                throw new ArgumentException(message);
        }
    }
}
