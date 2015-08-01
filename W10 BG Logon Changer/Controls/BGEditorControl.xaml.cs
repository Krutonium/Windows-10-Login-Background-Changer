using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using W10_BG_Logon_Changer.Tools.UserColorHandler;
using Brush = System.Windows.Media.Brush;
using Button = System.Windows.Controls.Button;
using Color = System.Drawing.Color;
using ComboBox = System.Windows.Controls.ComboBox;
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

            ShowUserImageToggle.IsChecked = Properties.Settings.Default.uimage;
            ShowGlyphsIconsToggle.IsChecked = Properties.Settings.Default.gimage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png",
                Title = "Select a Image",
                Multiselect = false
            };

            var dialog = ofd.ShowDialog();
            if (dialog != true) return;
            _mainWindow.SelectedFile = ofd.FileName;
            SelectedFile.Text = ofd.SafeFileName;
            ColorPreview.Background = _orgColor;
        }

        private void ColorPickerButton_Click(object sender, RoutedEventArgs e)
        {
            var cfd = new ColorDialog();
            var dialog = cfd.ShowDialog();

            if (dialog == DialogResult.OK || dialog == DialogResult.Yes)
            {
                _mainWindow.SelectedFile = FillImageColor(cfd.Color);

                var sd = new SolidColorBrush(cfd.Color.ToMediaColor());
                ColorPreview.Background = sd;

                SelectedFile.Text = "Background location...";
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
                MessageBox.Show("You must select a file first before you can patch (Default options count as file)",
                    "Error trying to patch");
                return;
            }

            _runningApplySettings = true;
            var holderContent = ((Button) sender);
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

            Properties.Resources.trans.Save(f, ImageFormat.Png);

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
            SelectedFile.Text = "Background location...";
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
            Scaling = ((ComboBox) sender).SelectedIndex;
        }
    }
}