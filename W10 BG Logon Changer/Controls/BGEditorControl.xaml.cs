using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using W10_BG_Logon_Changer.Tools.UserColorHandler;
using Brush = System.Windows.Media.Brush;
using Color = System.Drawing.Color;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace W10_BG_Logon_Changer.Controls
{
    /// <summary>
    /// Interaction logic for BGEditorControl.xaml
    /// </summary>
    public partial class BGEditorControl : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly Brush _orgColor;

        public BGEditorControl(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            _orgColor = ColorPreview.Background;
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

        private string FillImageColor(Color c)
        {
            var image = Path.GetTempFileName();

            DrawFilledRectangle(3840, 2160, new SolidBrush(c)).Save(image, ImageFormat.Jpeg);

            return image;
        }

        private Bitmap DrawFilledRectangle(int x, int y, System.Drawing.Brush b)
        {
            var bmp = new Bitmap(x, y);
            using (var graph = Graphics.FromImage(bmp))
            {
                var imageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(b, imageSize);
            }
            return bmp;
        }

        private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show("Are you sure you wish to reset the image?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (msg == MessageBoxResult.Yes)
            {
                File.Copy(Config.BakPriFileLocation, Config.PriFileLocation, true);
            }
        }

        private void ApplySettings_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ApplyChanges();
        }

        private void CurrentAccentButton_Click(object sender, RoutedEventArgs e)
        {
            var f = Path.GetTempFileName();

            Properties.Resources.trans.Save(f, ImageFormat.Png);

            _mainWindow.SelectedFile = f;
            Color c = ColorFunctions.GetImmersiveColor(ImmersiveColors.ImmersiveStartBackground);

            _mainWindow.WallpaperViewer.Source = new BitmapImage(new Uri(FillImageColor(c)));


            SelectedFile.Text = "Background location...";
            ColorPreview.Background = _orgColor;
        }

        private void RestoreHeroDefaults_Click(object sender, RoutedEventArgs e)
        {
            var f = Path.GetTempFileName();

            Properties.Resources._default.Save(f, ImageFormat.Png);

            _mainWindow.SelectedFile = f;

            SelectedFile.Text = "Background location...";
            ColorPreview.Background = _orgColor;
        }
    }
}
