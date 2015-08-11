using System.Diagnostics;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using TSettings;

namespace W10_Logon_BG_Changer.Controls
{
    /// <summary>
    /// Interaction logic for SettingsMenuControl.xaml
    /// </summary>
    public partial class SettingsMenuControl : UserControl
    {
        private readonly MainWindow _mainWindow;

        public SettingsMenuControl(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();

            LanguageGridLocation.Children.Add(new LanguageSelectControl(mainWindow));

            ShowUserImageToggle.Checked += _mainWindow.ToggleButton_OnChecked;
            ShowUserImageToggle.Unchecked += _mainWindow.ToggleButton_OnUnchecked;

            ShowGlyphsIconsToggle.Checked += _mainWindow.ToggleButton_OnChecked;
            ShowGlyphsIconsToggle.Unchecked += _mainWindow.ToggleButton_OnUnchecked;

            ShowUserImageToggle.IsChecked = Settings.Default.Get("uimage", true); //Settings.Default.uimage;
            ShowGlyphsIconsToggle.IsChecked = Settings.Default.Get("gimage", true); //Settings.Default.gimage;

            switch (Settings.Default.Get("flyout", Position.Right))
            {
                case Position.Right:
                    FlyoutPosSelect.SelectedIndex = 1;
                    break;

                case Position.Left:
                    FlyoutPosSelect.SelectedIndex = 0;
                    break;
            }

            ShowUserInfoLabel.Text = LanguageLibrary.Language.Default.show_user_info;
            ShowGlyphsLabel.Text = LanguageLibrary.Language.Default.show_glyphs;
            FlyoutLocationLabel.Text = LanguageLibrary.Language.Default.flyout_loc;
            AdvancedOptionsArea.Header = LanguageLibrary.Language.Default.group_advanced_options;
            LanguageSelect.Header = LanguageLibrary.Language.Default.main_top_sel_lang;

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
