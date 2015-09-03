using System;
using System.Windows;
using System.Windows.Input;

namespace W10_Logon_BG_Changer.Controls.Commands
{
    public class SelectImageCommand : ICommand
    {
        private readonly BgEditorControl _bgEditorControl;
        private readonly MainWindow _mainWindow;

        public SelectImageCommand(BgEditorControl bgEditorControl, MainWindow mainWindow)
        {
            _bgEditorControl = bgEditorControl;
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object parameter) => parameter != null;

        public void Execute(object parameter) => _bgEditorControl.SelectImageEvent_Clicked(this, new RoutedEventArgs());

        public event EventHandler CanExecuteChanged;
    }
}
