using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using SharedLibrary;
using TSettings;
using W10_Logon_BG_Changer.Controls.Commands;
using W10_Logon_BG_Changer.Tools.UserColorHandler;
using Brush = System.Drawing.Brush;
using Color = System.Windows.Media.Color;
using Image = System.Drawing.Image;

namespace W10_Logon_BG_Changer.Controls
{
    /// <summary>
    ///     Interaction logic for BGEditorControl.xaml
    /// </summary>
    public partial class BgEditorControl : UserControl
    {
        public static int Scaling = 5;
        private readonly MainWindow _mainWindow;
        private readonly Color _orgColor;
        private bool _runningApplySettings;

        public BgEditorControl(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            _orgColor = SolidColorPicker.SelectedColor ?? Colors.White;

            var color = Settings.Default.Get("dcolor", System.Drawing.Color.WhiteSmoke);
            SolidColorPicker.SelectedColor = color.ToMediaColor();

            ColorAccentButton.Content = LanguageLibrary.Language.Default.accet_color_button;
            ApplyChangesButton.Content = LanguageLibrary.Language.Default.apply_changes_button;
            ImageScalingLabel.Text = LanguageLibrary.Language.Default.image_scale;
            RestoreDefaultButton.Content = LanguageLibrary.Language.Default.restore_defaults_button;
            RestoreDefaultArea.Header = LanguageLibrary.Language.Default.group_restore_default;
            textBlock.Text = LanguageLibrary.Language.Default.or;
            SharebackgroundButton.Content = LanguageLibrary.Language.Default.share_bg;
            MyResolutionOption.Content = LanguageLibrary.Language.Default.image_scale_Resolution;
            NoneOption.Content = LanguageLibrary.Language.Default.scale_none_opt;

            TextBoxHelper.SetWatermark(SelectedFile, LanguageLibrary.Language.Default.select_img);

            TextBoxHelper.SetButtonCommandParameter(SelectedFile, "Hello World");
            TextBoxHelper.SetButtonCommand(SelectedFile, new SelectImageCommand(this, _mainWindow));
        }


        public void SelectImageEvent_Clicked(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.tif;*.tiff",
                Title = LanguageLibrary.Language.Default.title_select_image,
                Multiselect = false
            };

            if (!string.IsNullOrEmpty(Settings.Default.Get("last_folder", string.Empty)))
                ofd.InitialDirectory = Settings.Default.Get("last_folder", string.Empty);

            var dialog = ofd.ShowDialog();
            if (dialog != true) return;
            Settings.Default.Set("last_folder", Path.GetDirectoryName(ofd.FileName));
            var fileName = ofd.FileName;

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

            SelectedFile.Text = ofd.SafeFileName;
            //ColorPreview.Background = _orgColor;
            /*var c = ((SolidColorBrush)ColorPreview.Background).Color;
            var color = Color.FromArgb(c.R, c.G, c.B);
            pickColor.Foreground = new SolidColorBrush(Helpers.ContrastColor(color).ToMediaColor());
            */
            SolidColorPicker.SelectedColor = _orgColor;
            _mainWindow.SelectedFile = fileName;

            Settings.Default.Set("dcolor", System.Drawing.Color.FromArgb(_orgColor.R, _orgColor.G, _orgColor.B));
            Settings.Default.Set("filename", ofd.FileName);
            Settings.Default.Save();
            _mainWindow.GlyphsViewer.ToolTip = ofd.FileName;

            var t = Path.GetTempFileName();
            var f = Path.GetTempFileName();

            File.Copy(Config.BakPriFileLocation, f, true);
            LogonPriEditor.ModifyLogonPri(f, t, ofd.FileName);

            var loadedMeta = SharedHelpers.GetFileSize(ofd.FileName);
            ActualFileSizeTp.Text = loadedMeta.ActualFileSizeHuman;
            LoadedFileSizeTp.Text = loadedMeta.LoadedFileSizeHuman;
            PriFileSizeTp.Text = SharedHelpers.GetFileSize(t).ActualFileSizeHuman;

            File.Delete(t);
            File.Delete(f);
        }

        private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show(LanguageLibrary.Language.Default.reset_image_msg,
                LanguageLibrary.Language.Default.title_are_you_sure,
                MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (msg != MessageBoxResult.Yes) return;
            File.Copy(Config.BakPriFileLocation, Config.PriFileLocation, true);

            var f = Path.GetTempFileName();
            Properties.Resources._default.Save(f, ImageFormat.Png);

            Reset(f);
            Settings.Default.Delete("current.img");
            Settings.Default.Delete("dcolor");
            Settings.Default.Save();
        }

        private void ApplySettings_Click(object sender, RoutedEventArgs e)
        {
            if (_runningApplySettings) return;

            if (string.IsNullOrEmpty(_mainWindow.SelectedFile) || !File.Exists(_mainWindow.SelectedFile))
            {
                MessageBox.Show(LanguageLibrary.Language.Default.no_selected_file,
                    LanguageLibrary.Language.Default.title_error);
                return;
            }

            /*var c = ((SolidColorBrush)ColorPreview.Background).Color;
            var color = Color.FromArgb(c.R, c.G, c.B);
            pickColor.Foreground = new SolidColorBrush(Helpers.ContrastColor(color).ToMediaColor());

            Settings.Default.Set("dcolor", color);*/
            var c = SolidColorPicker.SelectedColor;
            if (c.HasValue)
                Settings.Default.Set("dcolor", System.Drawing.Color.FromArgb(c.Value.R, c.Value.G, c.Value.B));
            else
                Settings.Default.Delete("dcolor");

            Settings.Default.Set("filename", Path.GetFileName(_mainWindow.SelectedFile));
            Settings.Default.Save();
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

            DrawFilledRectangle(1024, 1024, new SolidBrush(System.Drawing.Color.Transparent)).Save(f, ImageFormat.Png);

            _mainWindow.SelectedFile = f;

            var c = ColorFunctions.GetImmersiveColor(ImmersiveColors.ImmersiveStartBackground);

            Reset(FillImageColor(c));


            SolidColorPicker.SelectedColor = c.ToMediaColor();
        }

        private void Reset(string image = "")
        {
            SelectedFile.Text = string.Empty;
            SolidColorPicker.SelectedColor = _orgColor;
            Settings.Default.Set("dcolor", System.Drawing.Color.FromArgb(_orgColor.R, _orgColor.G, _orgColor.B));

            if (image != "")
            {
                OverrideImageFromMainWindow(image);
            }
        }

        private void OverrideImageFromMainWindow(string url)
        {
            _mainWindow.WallpaperViewer.Source = new BitmapImage(new Uri(url));
        }

        public string FillImageColor(System.Drawing.Color c)
        {
            var image = Path.GetTempFileName();

            DrawFilledRectangle(3840, 2160, new SolidBrush(c)).Save(image, ImageFormat.Jpeg);

            return image;
        }

        private static Bitmap DrawFilledRectangle(int x, int y, Brush b)
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

        private void ShareBackGround_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.CreateBitmapFromVisual();
        }

        private void ColorPickerPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (SolidColorPicker.SelectedColor == null) return;
            var c = SolidColorPicker.SelectedColor.Value;
            _mainWindow.SelectedFile = FillImageColor(System.Drawing.Color.FromArgb(c.R, c.G, c.B));
        }
    }
}