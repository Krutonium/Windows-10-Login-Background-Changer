using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using W10_Logon_BG_Changer.Tools;

namespace W10_Logon_BG_Changer.Controls
{
    /// <summary>
    ///     Interaction logic for AboutControl.xaml
    /// </summary>
    public partial class AboutControl : UserControl
    {
        public AboutControl(MainWindow mainWindow)
        {
            InitializeComponent();

            AboutBox.Text = mainWindow.AssemblyInfo.Description;

            AboutArea.Header = LanguageLibrary.Language.Default.group_about_area;
            AuthorsGroupBox.Header = LanguageLibrary.Language.Default.group_authors_area;

            var authors = Properties.Resources.Authors;

            var sp = new StackPanel { Orientation = Orientation.Vertical };

            using (var reader = new StringReader(authors))
            {
                var line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        var tb = new TextBlock
                        {
                            Text = line,
                            Padding = new Thickness(5)
                        };

                        var l = line.Split('|');

                        if (l.Length > 0)
                        {
                            var inlineExpression =
                                string.Format(
                                    "<bold>{0}</bold> | <hyperlink NavigateUri='{1}' Click='Url_Click'>{1}</hyperlink>",
                                    l[0], l[1]);
                            InlineExpression.SetInlineExpression(tb, inlineExpression);
                        }

                        sp.Children.Add(tb);
                    }
                } while (line != null);
            }

            AuthorsGroupBox.Content = sp;
        }

        private void Url_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }
    }
}