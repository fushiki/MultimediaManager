using MultimediaManager.Core.FileSystem;
using MultimediaManager.Core.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MultimediaManager.Core.FileSystemTreeViews
{
    public enum FileSystemTreeType
    {
        Physical,
        Temporary,
        Database
    }

    public class FileSystemTreeViewModel:MultimediaManager.Core.BaseViewModel
    {


        //private static Filter<FileSystemEntityViewModel> mp3Filter =
            //new CastFilter<FileSystemEntityViewModel, FileSystemEntity>(MultimediaManager.Mp3.MusicModule.Mp3ExtensionFilter, x => x.Entity);
                
                //new ExtensionFilter(new string[] { MP3_EXTENSION }, true);
        private static Filter<FileSystemEntityViewModel> allFilter = Filter<FileSystemEntityViewModel>.AcceptAllFilter();


        #region Fields
        RelayCommand<bool> _showHiddenCommand;
        RelayCommand<bool> _mp3OnlyCommand;
        RelayCommand<bool> _deepSearchCommand;
        RelayCommand<string> _searchCommand;
        RelayCommand _clearCommand;
        RelayCommand _expandAll;
        RelayCommand _collapseAll;

        Filter<FileSystemEntityViewModel> _globalMustFilter;
        Filter<FileSystemEntityViewModel> _globalHasOnOfAttribute;
        string _searchText = String.Empty;
        bool _showHidden;
        bool _mp3Only;
        bool _deepSearch;
        bool _expandSearch;
        #endregion

        FileSystemTreeType _type;
        ObservableCollection<FileSystemEntityViewModel> _roots;
        IList<FileSystemEntityViewModel> _finded;


        #region Public Properties
        public bool ShowHidden
        {
            get { return _showHidden; }
            set
            {
                if (value == _showHidden)
                    return;
                _showHidden = value;
                OnPropertyChanged("ShowHidden");
            }
        }

        public Visibility ShowHiddenVisibility
        {
            get
            {
                if (_type == FileSystemTreeType.Physical)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public bool ExpandSearch
        {
            get { return _expandSearch; }
            set
            {
                if (value == _expandSearch)
                    return;
                _expandSearch = value;
                OnPropertyChanged("ExpandSearch");
            }
        }
        public Visibility AreExtendedSearchOptionsVisible
        {
            get
            {
                if (_type == FileSystemTreeType.Physical)
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }
        public String SearchText
        {
            get { return _searchText; }
            set
            {
                if (value == null || value.Equals(_searchText))
                    return;
                _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }
        public bool DeepSearch
        {
            get { return _deepSearch; }
            set
            {
                if (value == _deepSearch)
                    return;
                _deepSearch = value;
                if (!_deepSearch)
                    ExpandSearch = false;
                OnPropertyChanged("DeepSearch");
                
            }
        }
        public bool Mp3Only
        {
            get { return _mp3Only; }
            set
            {
                if (value == _mp3Only)
                    return;
                _mp3Only = value;
                OnPropertyChanged("Mp3Only");

            }
        }
        #endregion

        #region Commands

        public ICommand ExpandAllCommand
        {
            get
            {
                if (_expandAll == null)
                    _expandAll = new RelayCommand(OnExpandAll);
                return _expandAll;
            }
        }

        public ICommand CollapseAllCommand
        {
            get
            {
                if (_collapseAll == null)
                    _collapseAll = new RelayCommand(OnCollapseAll);
                return _collapseAll;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                    _clearCommand = new RelayCommand(OnClearPerformed);
                return _clearCommand;
            }
        }

 
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new RelayCommand<string>(OnSearchPerformed);
                return _searchCommand;
            }
        }



        public ICommand Mp3OnlyCommand
        {
            get
            {
                if (_mp3OnlyCommand == null)
                    _mp3OnlyCommand = new RelayCommand<bool>(OnMp3OnlyChanged);
                return _mp3OnlyCommand;
            }
        }
        public ICommand DeepSearchCommand
        {
            get
            {
                if (_deepSearchCommand == null)
                    _deepSearchCommand = new RelayCommand<bool>(OnDeepSearchChanged);
                return _deepSearchCommand;
            }
        }
        public ICommand ShowHiddenCommand
        {
            get
            {
                if (_showHiddenCommand == null)
                    _showHiddenCommand = new RelayCommand<bool>(OnShowHiddenChanged);
                return _showHiddenCommand;
            }
        }
        #endregion
        public ObservableCollection<FileSystemEntityViewModel> Roots { get { return _roots; } protected set { _roots = value; } }

        private Filter<FileSystemEntityViewModel> FilterWithGlobal(Filter<FileSystemEntityViewModel> filter)
        {
            Filter<FileSystemEntityViewModel> g = _globalMustFilter.Clone();
            g.Add(filter);
            return g;
        }

        #region Handlers
        protected virtual void OnSearchPerformed(string text)
        {
            if (_finded != null)
            {
                foreach (FileSystemEntityViewModel entity in _finded)
                {
                    entity.IsSelected = false;
                    entity.IsPath = false;
                }
                _finded.Clear();
            }
            performSearch(text);
        }
        protected virtual void OnClearPerformed(object obj)
        {
            SearchText = String.Empty;
            if (_finded != null)
            {
                foreach (FileSystemEntityViewModel entity in _finded)
                {
                    entity.IsSelected = false;
                    entity.IsPath = false;
                }
                _finded.Clear();
            }

        }
        protected virtual void OnShowHiddenChanged(bool param)
        {
            
        }
        protected virtual void OnDeepSearchChanged(bool parameter)
        {
            
        }
        protected virtual void OnMp3OnlyChanged(bool parameter)
        {
            Filter<FileSystemEntityViewModel> filter = null;
            Visibility visibility;
            if (parameter)
            {
                //_globalMustFilter.Add(mp3Filter);
                filter = new ContrFilter<FileSystemEntityViewModel>(_globalMustFilter);
                visibility = Visibility.Collapsed;
            }
            else
            {
                //_globalMustFilter.Remove(mp3Filter);
                filter = _globalMustFilter;
                visibility = Visibility.Visible;
            }
            foreach (var root in Roots)
                Travel(filter, root, true, true,
                    x => x.Visible = visibility);
        }
        protected virtual void OnCollapseAll(object parameter)
        {
            foreach(var root in Roots)
            {
                DirectoryViewModel dvm = root as DirectoryViewModel;
                if (dvm != null)
                    dvm.IsExpanded = false;
            }
        }

        protected virtual void OnExpandAll(object parameter)
        {
            Filter<FileSystemEntityViewModel> filter = _globalMustFilter;
            foreach(var root in Roots)
            {
                Travel(filter,root,true,false,
                    x => { var y = x as DirectoryViewModel; if (y != null) y.IsExpanded = true; }                        
                    );
            }
        }

        #endregion

        public FileSystemTreeViewModel(FileSystemTreeType type, string[] data)
        {
            _roots = new ObservableCollection<FileSystemEntityViewModel>();
            _type = type;
            _globalMustFilter = new CompositeFilter<FileSystemEntityViewModel>(CompositeFilter<FileSystemEntityViewModel>.CompositeType.AND);
            _globalHasOnOfAttribute = new CompositeFilter<FileSystemEntityViewModel>(CompositeFilter<FileSystemEntityViewModel>.CompositeType.OR);
            _globalMustFilter.Add(_globalHasOnOfAttribute);
            _globalHasOnOfAttribute.Add(allFilter);
            foreach(string rootdir in data)
            {
                Directory dir = createRootDirectory(rootdir);
                DirectoryViewModel dirvm = new DirectoryViewModel(dir, null, true);
                _roots.Add(dirvm);
            }
        }

        private Directory createRootDirectory(string rootdir)
        {
            switch(_type)
            {
                case FileSystemTreeType.Physical:
                    return new PhysicalDirecotry(rootdir);
                case FileSystemTreeType.Temporary:
                    return new TemporaryVirtualDirectory(String.Empty, rootdir);
                case FileSystemTreeType.Database:
                    throw new NotImplementedException("Database tree view");
                default:
                    throw new NotImplementedException("Not implemented type: " + _type.ToString());
            }
        }

        private void performSearch(string text)
        {
            ContainStringFilter filter = new ContainStringFilter(text);
            Filter<FileSystemEntityViewModel> globalclone = _globalMustFilter.Clone();
            globalclone.Add(filter);
            if (_finded == null)
                _finded = new List<FileSystemEntityViewModel>();
            else
                _finded.Clear();
            foreach (FileSystemEntityViewModel root in Roots)
                Travel(globalclone,
                    root,
                    DeepSearch,
                    false,
                    x => x.IsSelected = true,
                    _finded,
                    x=> x.IsPath = true);
        }


        #region Travel

        public bool Travel(Filter<FileSystemEntityViewModel> filter,
            FileSystemEntityViewModel root,
            bool deeptravel, bool travelWithoutLoading,
            Action<FileSystemEntityViewModel> action = null,
            IList<FileSystemEntityViewModel> marked = null,
            Action<FileSystemEntityViewModel> markPathAction = null)
        {
            bool root_is_result;
            bool result;
            root_is_result = result = filter.Execute(root);
            if(result)
            {
                if (action != null)
                    action(root);
                if (marked != null)
                    marked.Add(root);
            }
            DirectoryViewModel vm = root as DirectoryViewModel;
            if (vm != null)
            {
                if (!(!vm.IsExpanded && (!deeptravel || (travelWithoutLoading && !vm.HasChildrenLoaded))))            
                {
                    foreach (FileSystemEntityViewModel child in vm.Children)
                    {
                        bool tmpresult = Travel(filter, child, deeptravel, travelWithoutLoading, action, marked, markPathAction);
                        result = result || tmpresult;
                    }
                }
                if (ExpandSearch && result)
                    vm.IsExpanded = true;
            }
            if(!(markPathAction==null) && !root_is_result && result )
            {
                markPathAction(root);
                marked.Add(root);
            }
            return result;
        }
        #endregion

    }
}
