using HelperLibrary;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TSettings;
using W10_Logon_BG_Changer.Controls;
using W10_Logon_BG_Changer.Tools;
using W10_Logon_BG_Changer.Tools.Animations;
using W10_Logon_BG_Changer.Tools.UserColorHandler;
using Image = System.Drawing.Image;

namespace W10_Logon_BG_Changer
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

            Settings.Init(Config.SettingsFilePath, new TSettings.Encryptions.DesEncrpytion("W10Logon", "W10Logon"));

            //default all strings to en-us
            LanguageLibrary.Language.Init();
            LanguageLibrary.Language.Set(Settings.Default.Get("language", "en_us"));

            Debug.WriteLine((string)LanguageLibrary.Language.Default.title_error);

            if (!Settings.Default.Get("eula", false))
            {
                var dlg =
                    MessageBox.Show(
                        LanguageLibrary.Language.Default.EULA,
                        LanguageLibrary.Language.Default.title_eula, MessageBoxButton.YesNo);

                if (dlg == MessageBoxResult.No)
                {
                    Close();
                }
            }

            if (Helpers.IsBackgroundDisabled())
            {
                MessageBox.Show(LanguageLibrary.Language.Default.background_disabled, LanguageLibrary.Language.Default.title_dg_disabled);
            }

            Debug.WriteLine("[AccentColor]: " +
                            ColorFunctions.GetImmersiveColor(ImmersiveColors.ImmersiveStartBackground));

            Title += " - " + AssemblyInfo.Version;

            Settings.Default.Set("eula", true);
            Settings.Default.Save();

            Debug.WriteLine("[EULA Test] {0}", Settings.Default.Get<bool>("eula"));

            LanguageFlyout.Content = new LanguageSelectControl(this);

            SettingFlyout.Content = new BgEditorControl(this);
            SettingFlyout.IsOpen = true;

            AboutFlyout.Content = new AboutControl(this);

            HelperLib.TakeOwnership(Config.LogonFolder);
            HelperLib.TakeOwnership(Config.PriFileLocation);

            if (!File.Exists(Config.BakPriFileLocation))
            {
                Debug.WriteLine("[Warning]: Could not find Windows.UI.Logon.pri.bak file. Creating new.");
                File.Copy(Config.PriFileLocation, Config.BakPriFileLocation);
            }

            HelperLib.TakeOwnership(Config.BakPriFileLocation);

            File.Copy(Config.BakPriFileLocation, _tempPriFile, true);

            if (File.Exists(Config.CurrentImageLocation))
            {
                var temp = Path.GetTempFileName();
                File.Copy(Config.CurrentImageLocation, temp, true);

                Settings.Default.Set("current.img", File.ReadAllBytes(temp));
                Settings.Default.Save();

                File.Delete(Config.CurrentImageLocation);
                //WallpaperViewer.Source = new BitmapImage(new Uri(temp));
            }

            Loaded += (o, i) =>
            {
                SettingFlyout.Position = Settings.Default.Get("flyout", Position.Right);
                GlyphsViewer.ToolTip = Settings.Default.Get("filename", "No File");
            };

            if (Settings.Default.Exist("current.img"))
            {
                var temp = Path.GetTempFileName();

                File.WriteAllBytes(temp, Settings.Default.Get<byte[]>("current.img"));

                WallpaperViewer.Source = new BitmapImage(new Uri(temp));
            }

            EditBackgroundLabel.Text = LanguageLibrary.Language.Default.main_top_edit;
            LockWindowsLabel.Text = LanguageLibrary.Language.Default.main_top_lock;
            AboutButton.Content = LanguageLibrary.Language.Default.main_top_about;

            SettingFlyout.Header = LanguageLibrary.Language.Default.flyout_edit_title;
            AboutFlyout.Header = LanguageLibrary.Language.Default.flyout_about_title;
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
                            ResizeImage(Image.FromFile(value), (int)SystemParameters.PrimaryScreenWidth,
                                (int)SystemParameters.PrimaryScreenHeight).ToBitmapSource();
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

            //File.Copy(SelectedFile, Config.CurrentImageLocation, true);

            Settings.Default.Set("current.img", File.ReadAllBytes(SelectedFile));
            Settings.Default.Save();

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
                    ResizeImage(Image.FromFile(SelectedFile), (int)SystemParameters.PrimaryScreenWidth,
                        (int)SystemParameters.PrimaryScreenHeight).Save(imagetemp, ImageFormat.Png);
                    break;

                case 4:
                    imagetemp = SelectedFile;
                    break;
            }

            PriBuilder.CreatePri(_tempPriFile, _newPriLocation, imagetemp);

            File.Copy(_newPriLocation, Config.PriFileLocation, true);

            MessageBox.Show(
                "Successfully changed login background! Please lock your device to view your new background.",
                "Success!");
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
            switch (tb.Tag.ToString())
            {
                case "gimage":
                    switch (tb.IsChecked)
                    {
                        case true:
                            ImageFader.fadeIn(GlyphsViewer);
                            break;

                        case false:
                            ImageFader.fadeOut(GlyphsViewer);
                            break;
                    }
                    break;

                case "uimage":
                    switch (tb.IsChecked)
                    {
                        case true:
                            ImageFader.fadeIn(UserViewer);
                            break;

                        case false:
                            ImageFader.fadeOut(UserViewer);
                            break;
                    }
                    break;
            }
            Settings.Default.Set(tb.Tag.ToString(), tb.IsChecked != null && (bool)tb.IsChecked);
            Settings.Default.Save();
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

        public void ChangeFlyoutLocation(string loc)
        {
            Debug.WriteLine("[SettingFlyout]: " + loc);
            switch (loc)
            {
                case "left":
                    SettingFlyout.Position = Position.Left;
                    AboutFlyout.Position = Position.Right;
                    Debug.WriteLine("[AboutFlyout]: right");
                    break;

                case "right":
                    SettingFlyout.Position = Position.Right;
                    AboutFlyout.Position = Position.Left;
                    Debug.WriteLine("[AboutFlyout]: left");
                    break;
            }

            Settings.Default.Set("flyout", SettingFlyout.Position);
            Settings.Default.Save();
        }

        private void ImageViewer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SettingFlyout.IsOpen = false;
            AboutFlyout.IsOpen = false;
        }

        private void LanguageSelect_OnClick(object sender, RoutedEventArgs e)
        {
            LanguageFlyout.IsOpen = !LanguageFlyout.IsOpen;
        }
    }
}