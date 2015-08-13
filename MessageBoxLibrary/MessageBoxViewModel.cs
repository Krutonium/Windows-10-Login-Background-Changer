using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MessageBoxLibrary
{
    internal class MessageBoxViewModel : INotifyPropertyChanged
    {
        public MessageBoxResult Result { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isOkDefault;
        private bool _isYesDefault;
        private bool _isNoDefault;
        private bool _isCancelDefault;

        private string _title;
        private string _message;
        private MessageBoxButton _buttonOption;
        private MessageBoxOptions _options;
        
        private Visibility _yesNoVisibility;
        private Visibility _cancelVisibility;
        private Visibility _okVisibility;

        private HorizontalAlignment _contentTextAlignment;
        private FlowDirection _contentFlowDirection;
        private FlowDirection _titleFlowDirection;

        private ICommand _yesCommand;
        private ICommand _noCommand;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _escapeCommand;
        private ICommand _closeCommand;

        private WPFMessageBoxWindow _view;
        private ImageSource _messageImageSource;

        public MessageBoxViewModel(
            WPFMessageBoxWindow view,
            string title,
            string message,
            MessageBoxButton buttonOption,
            MessageBoxImage image,
            MessageBoxResult defaultResult,
            MessageBoxOptions options)
        {
            //TextAlignment
            Title = title;
            Message = message;
            ButtonOption = buttonOption;
            Options = options;

            SetDirections(options);
            SetButtonVisibility(buttonOption);
            SetImageSource(image);
            SetButtonDefault(defaultResult);
            _view = view;
        }

        public MessageBoxButton ButtonOption
        {
            get { return _buttonOption; }
            set
            {
                if (_buttonOption != value)
                {
                    _buttonOption = value;
                    NotifyPropertyChange("ButtonOption");
                }
            }
        }

        public MessageBoxOptions Options
        {
            get { return _options; } 
            set
            {
                if (_options != value)
                {
                    _options = value;
                    NotifyPropertyChange("Options");
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChange("Title");
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChange("Message");
                }
            }
        }

        public ImageSource MessageImageSource
        {
            get { return _messageImageSource; }
            set
            {
                _messageImageSource = value;
                NotifyPropertyChange("MessageImageSource");
            }
        }

        public Visibility YesNoVisibility
        {
            get { return _yesNoVisibility; }
            set
            {
                if (_yesNoVisibility != value)
                {
                    _yesNoVisibility = value;
                    NotifyPropertyChange("YesNoVisibility");
                }
            }
        }

        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set
            {
                if (_cancelVisibility != value)
                {
                    _cancelVisibility = value;
                    NotifyPropertyChange("CancelVisibility");
                }
            }
        }

        public Visibility OkVisibility
        {
            get { return _okVisibility; }
            set
            {
                if (_okVisibility != value)
                {
                    _okVisibility = value;
                    NotifyPropertyChange("OkVisibility");
                }
            }
        }

        public HorizontalAlignment ContentTextAlignment
        {
            get { return _contentTextAlignment; }
            set
            {
                if (_contentTextAlignment != value)
                {
                    _contentTextAlignment = value;
                    NotifyPropertyChange("ContentTextAlignment");
                }
            }
        }

        public FlowDirection ContentFlowDirection
        {
            get { return _contentFlowDirection; }
            set
            {
                if (_contentFlowDirection != value)
                {
                    _contentFlowDirection = value;
                    NotifyPropertyChange("ContentFlowDirection");
                }
            }
        }


        public FlowDirection TitleFlowDirection
        {
            get { return _titleFlowDirection; }
            set
            {
                if (_titleFlowDirection != value)
                {
                    _titleFlowDirection = value;
                    NotifyPropertyChange("TitleFlowDirection");
                }
            }
        }


        public bool IsOkDefault
        {
            get { return _isOkDefault; }
            set 
            {
                if (_isOkDefault != value)
                {
                    _isOkDefault = value;
                    NotifyPropertyChange("IsOkDefault");
                }
            }
        }

        public bool IsYesDefault
        {
            get { return _isYesDefault; }
            set
            {
                if (_isYesDefault != value)
                {
                    _isYesDefault = value;
                    NotifyPropertyChange("IsYesDefault");
                }
            }
        }
        
        public bool IsNoDefault
        {
            get { return _isNoDefault; }
            set
            {
                if (_isNoDefault != value)
                {
                    _isNoDefault = value;
                    NotifyPropertyChange("IsNoDefault");
                }
            }
        }
        
        public bool IsCancelDefault
        {
            get { return _isCancelDefault; }
            set
            {
                if (_isCancelDefault != value)
                {
                    _isCancelDefault = value;
                    NotifyPropertyChange("IsCancelDefault");
                }
            }
        }

        // called when the yes button is pressed
        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new DelegateCommand(() =>
                {
                    Result = MessageBoxResult.Yes;
                    _view.Close();
                }));
            }
        }

        // called when the no button is pressed
        public ICommand NoCommand
        {
            get
            {
                return _noCommand ?? (_noCommand = new DelegateCommand(() =>
                {
                    Result = MessageBoxResult.No;
                    _view.Close();
                }));
            }
        }

        // called when the cancel button is pressed
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new DelegateCommand(() =>
                {
                    Result = MessageBoxResult.Cancel;
                    _view.Close();
                }));
            }
        }

        // called when the ok button is pressed
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new DelegateCommand(() =>
                {
                    Result = MessageBoxResult.OK;
                    _view.Close();
                }));
            }
        }

        // called when the escape key is pressed
        public ICommand EscapeCommand
        {
            get
            {
                return _escapeCommand ?? (_escapeCommand = new DelegateCommand(() =>
                {
                    switch (ButtonOption)
                    {
                        case MessageBoxButton.OK:
                            Result = MessageBoxResult.OK;
                            _view.Close();
                            break;

                        case MessageBoxButton.YesNoCancel:
                        case MessageBoxButton.OKCancel:
                            Result = MessageBoxResult.Cancel;
                            _view.Close();
                            break;

                        case MessageBoxButton.YesNo:
                            // ignore close
                            break;
                    }
                }));
            }
        }

        // called when the form is closed by via close button click or programmatically
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
                {
                    if (Result != MessageBoxResult.None) return;
                    switch (ButtonOption)
                    {
                        case MessageBoxButton.OK:
                            Result = MessageBoxResult.OK;
                            break;

                        case MessageBoxButton.YesNoCancel:
                        case MessageBoxButton.OKCancel:
                            Result = MessageBoxResult.Cancel;
                            break;

                        case MessageBoxButton.YesNo:
                            // ignore close
                            break;
                    }
                }));
            }
        }

        private void SetDirections(MessageBoxOptions options)
        {
            switch (options)
            { 
                case MessageBoxOptions.None:
                    ContentTextAlignment = HorizontalAlignment.Left;
                    ContentFlowDirection = FlowDirection.LeftToRight;
                    TitleFlowDirection = FlowDirection.LeftToRight;
                    break;

                case MessageBoxOptions.RightAlign:
                    ContentTextAlignment = HorizontalAlignment.Right;
                    ContentFlowDirection = FlowDirection.LeftToRight;
                    TitleFlowDirection = FlowDirection.LeftToRight;
                    break;

                case MessageBoxOptions.RtlReading:
                    ContentTextAlignment = HorizontalAlignment.Right;
                    ContentFlowDirection = FlowDirection.RightToLeft;
                    TitleFlowDirection = FlowDirection.RightToLeft;
                    break;

                case MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading:
                    ContentTextAlignment = HorizontalAlignment.Left;
                    ContentFlowDirection = FlowDirection.RightToLeft;
                    TitleFlowDirection = FlowDirection.RightToLeft;
                    break;

            }
        }

        private void NotifyPropertyChange(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void SetButtonDefault(MessageBoxResult defaultResult)
        {
            switch (defaultResult)
            { 
                case MessageBoxResult.OK:
                    IsOkDefault = true;
                    break;

                case MessageBoxResult.Yes:
                    IsYesDefault = true;
                    break;

                case MessageBoxResult.No:
                    IsNoDefault = true;
                    break;

                case MessageBoxResult.Cancel:
                    IsCancelDefault = true;
                    break;
            }
        }

        private void SetButtonVisibility(MessageBoxButton buttonOption)
        {
            switch (buttonOption)
            {
                case MessageBoxButton.YesNo:
                    OkVisibility = CancelVisibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.YesNoCancel:
                    OkVisibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.OK:
                    YesNoVisibility = CancelVisibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.OKCancel:
                    YesNoVisibility = Visibility.Collapsed;
                    break;

                default:
                    OkVisibility = CancelVisibility = YesNoVisibility = Visibility.Collapsed;
                    break;
            }
        }

        private void SetImageSource(MessageBoxImage image)
        {
            switch (image)
            {
                //case MessageBoxImage.Hand:
                //case MessageBoxImage.Stop:
                case MessageBoxImage.Error:
                    MessageImageSource = SystemIcons.Error.ToImageSource();
                    break;

                //case MessageBoxImage.Exclamation:
                case MessageBoxImage.Warning:
                    MessageImageSource = SystemIcons.Warning.ToImageSource();
                    break;

                case MessageBoxImage.Question:
                    MessageImageSource = SystemIcons.Question.ToImageSource();
                    break;

                //case MessageBoxImage.Asterisk:
                case MessageBoxImage.Information:
                    MessageImageSource = SystemIcons.Information.ToImageSource();
                    break;

                default:
                    MessageImageSource = null;
                    break;
            }
        }
    }
}
