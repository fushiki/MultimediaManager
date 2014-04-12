using MultimediaManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.ViewModels
{
    public class MainWindowViewModel :WindowViewModel
    {
        #region Views
        BaseViewModel _currentView;
        BaseViewModel _duplicateView;
        BaseViewModel _tagsView;

        public BaseViewModel DuplicateView 
        { 
            get
            {
                if (_duplicateView == null)
                    _duplicateView = new DuplicateViewModel();
                return _duplicateView;
            }
        }
        public BaseViewModel TagsView
        {
            get
            {
                if (_tagsView == null)
                    _tagsView = new TagsViewModel();
                return _tagsView;
            }
        }

        #endregion


        private RelayCommand _openCommand;

        public RelayCommand OpenCommand
        {
            get 
            {
                if (_openCommand == null)
                    _openCommand = new RelayCommand(param => OnOpenView(param));
                return _openCommand; 
            }
        }
        
        public void OnOpenView(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("OnOpenView parameter.");
            BaseViewModel viewModel = parameter as BaseViewModel;
            if (viewModel == null)
                throw new InvalidCastException("OnOpenView parameter type=" + parameter.GetType().ToString() + ". Expected: BaseViewModel");
            CurrentView = viewModel;
        }
        public BaseViewModel CurrentView
        {
            get
            {
                return _currentView;
            }
             set
            {
                if (value == null || value.Equals(_currentView))
                    return;
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        public MainWindowViewModel(String title):base(title)
        {
            _currentView = new DuplicateViewModel();
        }

        void onClickR(object sender, EventArgs e)
        {

        }
    }
}
