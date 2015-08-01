using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HelperLibrary;
using MahApps.Metro.Controls;
using W10_BG_Logon_Changer.Controls;
using W10_BG_Logon_Changer.Properties;
using W10_BG_Logon_Changer.Tools;
using W10_BG_Logon_Changer.Tools.UserColorHandler;

namespace W10_BG_Logon_Changer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly string _newPriLocation = Path.Combine(Path.GetTempPath(), "new_temp_pri.pri");
        private readonly string _tempPriFile = Path.Combine(Path.GetTempPath(), "bak_temp_pri.pri");
        private string _selectedFile;
        public AssemblyInfo AssemblyInfo = new AssemblyInfo(Assembly.GetEntryAssembly());

        public MainWindow()
        {
            InitializeComponent();

            if (!Settings.Default.eula)
            {
                var dlg =
                    MessageBox.Show(
                        "Upon using this software you agree that we are not at fault if your system stops working corretly.",
                        "EULA", MessageBoxButton.YesNo);

                if (dlg == MessageBoxResult.No)
                {
                    Close();
                }
            }

            Debug.WriteLine(ColorFunctions.GetImmersiveColor(ImmersiveColors.ImmersiveStartBackground));

            Title += " - " + AssemblyInfo.Version;

            Settings.Default.eula = true;
            Settings.Default.Save();

            SettingFlyout.Content = new BgEditorControl(this);
            SettingFlyout.IsOpen = true;

            AboutFlyout.Content = new AboutControl(this);

            HelperLib.TakeOwnership(Config.LogonFolder);
            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
                Debug.WriteLine("Orginal file didn't exist D:");
                File.Copy(Config.PriFileLocation, Config.BakPriFileLocation);
            }

            HelperLib.TakeOwnership(Config.BakPriFileLocation);

            File.Copy(Config.BakPriFileLocation, _tempPriFile, true);

            if (File.Exists(Config.CurrentImageLocation))
            {
                var temp = Path.GetTempFileName();

                File.Copy(Config.CurrentImageLocation, temp, true);
                WallpaperViewer.Source = new BitmapImage(new Uri(temp));
            }
        }

        public string SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                //WallpaperViewer.Source = new BitmapImage(new Uri(value));
                switch (BgEditorControl.Scaling)
                {
                    case 0:
                        WallpaperViewer.Source = ResizeImage(Image.FromFile(value), 1280, 720).ToBitmapSource();
                        break;
                    case 1:
                        WallpaperViewer.Source = ResizeImage(Image.FromFile(value), 1920, 1080).ToBitmapSource();
                        break;
                    case 2:
                        WallpaperViewer.Source = ResizeImage(Image.FromFile(value), 3840, 2160).ToBitmapSource();
                        break;
                    case 3:
                        WallpaperViewer.Source =
                            ResizeImage(Image.FromFile(value), (int) SystemParameters.PrimaryScreenWidth,
                                (int) SystemParameters.PrimaryScreenHeight).ToBitmapSource();
                        break;
                    case 4:
                        WallpaperViewer.Source = new BitmapImage(new Uri(value));
                        break;
                }

                _selectedFile = value;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingFlyout.IsOpen = !SettingFlyout.IsOpen;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutFlyout.IsOpen = !AboutFlyout.IsOpen;
        }

        public void ApplyChanges()
        {
            //Lets just delete the old one maybe this was getting bugged?
            if (File.Exists(_newPriLocation))
            {
                File.Delete(_newPriLocation);
            }

            File.Copy(SelectedFile, Config.CurrentImageLocation, true);


            File.Copy(Config.BakPriFileLocation, _tempPriFile, true);

            var imagetemp = Path.GetTempFileName();
            //ResizeImage(Image.FromFile(SelectedFile), 1920, 1080).Save(imagetemp, ImageFormat.Png);
            switch (BgEditorControl.Scaling)
            {
                case 0:
                    ResizeImage(Image.FromFile(SelectedFile), 1280, 720).Save(imagetemp, ImageFormat.Png);
                    break;
                case 1:
                    ResizeImage(Image.FromFile(SelectedFile), 1920, 1080).Save(imagetemp, ImageFormat.Png);
                    break;
                case 2:
                    ResizeImage(Image.FromFile(SelectedFile), 3840, 2160).Save(imagetemp, ImageFormat.Png);
                    break;
                case 3:
                    ResizeImage(Image.FromFile(SelectedFile), (int) SystemParameters.PrimaryScreenWidth,
                        (int) SystemParameters.PrimaryScreenHeight).Save(imagetemp, ImageFormat.Png);
                    break;
                case 4:
                    imagetemp = SelectedFile;
                    break;
            }

            PriBuilder.CreatePri(_tempPriFile, _newPriLocation, imagetemp);

            File.Copy(_newPriLocation, Config.PriFileLocation, true);
            MessageBox.Show("Finished patching the file please lock and look at it", "Finished patching");
        }

        public void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var tb = sender as ToggleButton;

            DoToggleStuff(tb);
        }

        public void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var tb = sender as ToggleButton;

            DoToggleStuff(tb);
        }

        private void DoToggleStuff(ToggleButton tb)
        {
            if (tb == null) return;

            switch (tb.Tag.ToString())
            {
                case "gimage":
                    Debug.Assert(tb.IsChecked != null, "tb.IsChecked != null");
                    Settings.Default.gimage = tb.IsChecked.Value;
                    GlyphsViewer.Visibility = tb.IsChecked != null && (bool) tb.IsChecked
                        ? Visibility.Visible
                        : Visibility.Hidden;
                    break;
                case "uimage":
                    Debug.Assert(tb.IsChecked != null, "tb.IsChecked != null");
                    Settings.Default.uimage = tb.IsChecked.Value;
                    ImageSource image = tb.IsChecked != null && tb.IsChecked.Value
                        ? Properties.Resources.login.ToBitmapSource()
                        : Properties.Resources.login_noUser.ToBitmapSource();

                    LoginViewer.Source = image;
                    break;
            }

            Settings.Default.Save();
            //LoginViewer.Visibility = !tb.IsChecked.Value ? Visibility.Hidden : Visibility.Visible;
        }

        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            Win32Api.LockWorkStation();
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}