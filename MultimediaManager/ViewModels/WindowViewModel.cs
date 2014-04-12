using MultimediaManager.Core;
using System;
using System.Windows.Input;


namespace MultimediaManager.ViewModels
{
    public class WindowViewModel:BaseViewModel
    {
        String _title;
        public String Title {
            get { return _title; }
            set 
            {
                if (String.IsNullOrEmpty(value) || _title.Equals(value))
                    return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public WindowViewModel(string title)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException("title");
            _title = title;
        }

        RelayCommand _closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());
                return _closeCommand;
            }
        }


        public event EventHandler RequestClose;
        protected void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
