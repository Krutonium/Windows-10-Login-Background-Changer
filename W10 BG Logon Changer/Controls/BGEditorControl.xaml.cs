using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using W10_BG_Logon_Changer.Tools;
using W10_BG_Logon_Changer.Tools.UserColorHandler;
using Brush = System.Windows.Media.Brush;
using Button = System.Windows.Controls.Button;
using Color = System.Drawing.Color;
using ComboBox = System.Windows.Controls.ComboBox;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace W10_BG_Logon_Changer.Controls
{
    /// <summary>
    ///     Interaction logic for BGEditorControl.xaml
    /// </summary>
    public partial class BgEditorControl : UserControl
    {
        public static int Scaling = 5;
        private readonly MainWindow _mainWindow;
        private readonly Brush _orgColor;
        private bool _runningApplySettings;

        public BgEditorControl(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            _orgColor = ColorPreview.Background;

            ShowUserImageToggle.Checked += _mainWindow.ToggleButton_OnChecked;
            ShowUserImageToggle.Unchecked += _mainWindow.ToggleButton_OnUnchecked;

            ShowGlyphsIconsToggle.Checked += _mainWindow.ToggleButton_OnChecked;
            ShowGlyphsIconsToggle.Unchecked += _mainWindow.ToggleButton_OnUnchecked;

            ShowUserImageToggle.IsChecked = Settings.Get("uimage", true);//Settings.Default.uimage;
            ShowGlyphsIconsToggle.IsChecked = Settings.Get("gimage", true);//Settings.Default.gimage;

            //Debug.WriteLine(Settings.Default.flyoutloc);
            switch (Settings.Get("flyout", Position.Right))
            {
                case Position.Right:
                    FlyoutPosSelect.SelectedIndex = 1;
                    break;

                case Position.Left:
                    FlyoutPosSelect.SelectedIndex = 0;
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Image Formats|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff",
                Title = "Select a Image",
                Multiselect = false
            };

            if (!string.IsNullOrEmpty(Settings.Get("last_folder", string.Empty)))
                ofd.InitialDirectory = Settings.Get("last_folder", string.Empty);

            var dialog = ofd.ShowDialog();
            if (dialog != true) return;
            Settings.Set("last_folder", Path.GetDirectoryName(ofd.FileName));
            string fileName = ofd.FileName;

            var extension = Path.GetExtension(fileName);
            if (extension != null)
            {
                var ext = extension.ToLower();
                if (ext != ".png" || ext != ".jpg" || ext != ".jpeg")
                {
                    var temp = Path.GetTempFileName();
                    Image.FromFile(fileName).Save(temp, ImageFormat.Png);
                    fileName = temp;
                }
            }

            _mainWindow.SelectedFile = fileName;
            SelectedFile.Text = ofd.SafeFileName;
            ColorPreview.Background = _orgColor;
            Settings.Set("filename", ofd.FileName);
            Settings.Save();
            _mainWindow.GlyphsViewer.ToolTip = ofd.FileName;
        }

        private void ColorPickerButton_Click(object sender, RoutedEventArgs e)
        {
            var cfd = new ColorDialog();
            var dialog = cfd.ShowDialog();

            if (dialog != DialogResult.OK && dialog != DialogResult.Yes) return;
            _mainWindow.SelectedFile = FillImageColor(cfd.Color);

            var sd = new SolidColorBrush(cfd.Color.ToMediaColor());
            ColorPreview.Background = sd;

            SelectedFile.Text = "Background filename will appear here.";

            Color clr = cfd.Color;

            int r = 0;
            int g = 0;
            int b = 0;

            r = Convert.ToInt32(clr.R);
            g = Convert.ToInt32(clr.G);
            b = Convert.ToInt32(clr.B);

            int rgb = r + g + b;

            if (rgb > 382)
            {
                pickColor.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                pickColor.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show("Are you sure you wish to reset the image?", "Are you sure?",
                MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (msg != MessageBoxResult.Yes) return;
            File.Copy(Config.BakPriFileLocation, Config.PriFileLocation, true);

            File.Delete(Config.CurrentImageLocation);

            var f = Path.GetTempFileName();
            Properties.Resources._default.Save(f, ImageFormat.Png);

            Reset(f);
        }

        private void ApplySettings_Click(object sender, RoutedEventArgs e)
        {
            if (_runningApplySettings) return;

            if (string.IsNullOrEmpty(_mainWindow.SelectedFile) || !File.Exists(_mainWindow.SelectedFile))
            {
                MessageBox.Show("You must first select a file before you can set your login background! (Default options count as a file)",
                    "Error");
                return;
            }

            Settings.Set("filename", Path.GetFileName(_mainWindow.SelectedFile));
            Settings.Save();
            _runningApplySettings = true;
            var holderContent = ((Button)sender);
            var progress = new ProgressRing
            {
                IsActive = true,
                Visibility = Visibility.Visible,
                IsLarge = false,
                Width = 30,
                Height = 30
            };

            var def = holderContent.Content;
            holderContent.Content = progress;

            Task.Run(() => _mainWindow.ApplyChanges()).ContinueWith(delegate
            {
                Dispatcher.Invoke(() =>
                {
                    holderContent.Content = def;
                    _runningApplySettings = false;
                });
            });
        }

        private void CurrentAccentButton_Click(object sender, RoutedEventArgs e)
        {
            var f = Path.GetTempFileName();

            DrawFilledRectangle(1024, 1024, new SolidBrush(Color.Transparent)).Save(f, ImageFormat.Png);

            _mainWindow.SelectedFile = f;
            var c = ColorFunctions.GetImmersiveColor(ImmersiveColors.ImmersiveStartBackground);

            Reset(FillImageColor(c));
        }

        private void RestoreHeroDefaults_Click(object sender, RoutedEventArgs e)
        {
            var f = Path.GetTempFileName();

            Properties.Resources._default.Save(f, ImageFormat.Png);

            _mainWindow.SelectedFile = f;

            Reset();
        }

        private void Reset(string image = "")
        {
            SelectedFile.Text = "Background filename appears here.";
            ColorPreview.Background = _orgColor;

            if (image != "")
            {
                OverrideImageFromMainWindow(image);
            }
        }

        private void OverrideImageFromMainWindow(string url)
        {
            _mainWindow.WallpaperViewer.Source = new BitmapImage(new Uri(url));
        }

        public string FillImageColor(Color c)
        {
            var image = Path.GetTempFileName();

            DrawFilledRectangle(3840, 2160, new SolidBrush(c)).Save(image, ImageFormat.Jpeg);

            return image;
        }

        private static Bitmap DrawFilledRectangle(int x, int y, System.Drawing.Brush b)
        {
            var bmp = new Bitmap(x, y);
            using (var graph = Graphics.FromImage(bmp))
            {
                var imageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(b, imageSize);
            }
            return bmp;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Scaling = ((ComboBox)sender).SelectedIndex;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(((ComboBox)sender).SelectedIndex);
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    _mainWindow.ChangeFlyoutLocation("left");
                    break;

                case 1:
                    _mainWindow.ChangeFlyoutLocation("right");
                    break;
            }
        }
    }
}