using System;
using W10_Logon_BG_Changer___Command_Line.Helpers;

namespace W10_Logon_BG_Changer___Command_Line
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var imageHelper = new ImageHelper();
            var validator = new ValidationHelper();
            var menuHelper = new MenuHelper();

            try
            {
                imageHelper.TakeOwnership();
                validator.ValidateFirstArgument(args);
                string firstArgument = args[0].ToLower().Replace("/", "").Replace("-", "");

                switch (firstArgument)
                {
                    case "h":
                        menuHelper.ShowHelp();
                        break;
                    case "a":
                        menuHelper.ShowAuthors();
                        break;
                    case "c":
                        validator.ValidateColorArgument(args);
                        imageHelper.ChangeColor(args[1]);
                        break;
                    case "i":
                        validator.ValidateImageArgument(args);
                        imageHelper.ChangeImage(args[1]);
                        menuHelper.ShowSuccessMessage();
                        break;
                    case "r":
                        imageHelper.Restore();
                        menuHelper.ShowSuccessMessage();
                        break;
                    default:
                        throw new ArgumentException("Please use '/h' for help");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}