using System;
using W10_Logon_BG_Changer___Command_Line.Helpers;

namespace W10_Logon_BG_Changer___Command_Line
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ImageHelper.TakeOwnership();
                ValidationHelper.ValidateFirstArgument(args);
                var firstArgument = args[0].ToLower().Replace("/", "").Replace("-", "");

                switch (firstArgument)
                {
                    case "h":
                        MenuHelper.ShowHelp();
                        break;
                    case "a":
                        MenuHelper.ShowAuthors();
                        break;
                    case "c":
                        ValidationHelper.ValidateColorArgument(args);
                        ImageHelper.ChangeColor(args[1]);
                        break;
                    case "i":
                        ValidationHelper.ValidateImageArgument(args);
                        ImageHelper.ChangeImage(args[1]);
                        MenuHelper.ShowSuccessMessage();
                        break;
                    case "r":
                        ImageHelper.Restore();
                        MenuHelper.ShowSuccessMessage();
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