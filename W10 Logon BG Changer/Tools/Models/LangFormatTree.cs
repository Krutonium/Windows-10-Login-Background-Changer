using System.ComponentModel;
using System.Runtime.CompilerServices;
using W10_Logon_BG_Changer.Properties;

namespace W10_Logon_BG_Changer.Tools.Models
{
    public class LangFormatTree : INotifyPropertyChanged
    {
        private bool _enabled;
        private string _langcode;
        private string _name;

        public LangFormatTree(string name, string code, bool enabled = false)
        {
            _name = name;
            _langcode = code;
            _enabled = enabled;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged1();
            }
        }

        public string LangCode
        {
            get { return _langcode; }
            set
            {
                _langcode = value;
                OnPropertyChanged1();
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged1();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged1([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}