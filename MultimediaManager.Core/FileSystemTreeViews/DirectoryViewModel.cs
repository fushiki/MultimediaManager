
using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystemTreeViews
{
    public class DirectoryViewModel : FileSystemEntityViewModel
    {
        readonly ObservableCollection<FileSystemEntityViewModel> _children;
        bool _isExpanded;
        bool _isLazy;
        bool _isLoaded;
        private static FileSystemEntityViewModel DummyChild = new DirectoryViewModel(null, null, true);

        public bool HasChildrenLoaded
        {
            get { return _isLoaded; }
        }
        //public RelayCommand Test { get { return new RelayCommand(param => System.Windows.MessageBox.Show("Asd")); } }
        public ObservableCollection<FileSystemEntityViewModel> Children
        {
            get
            {
                if (!_isLoaded)
                {
                    if (_isLazy && _children.Count > 0)
                        _children.RemoveAt(0);
                    Load();
                }
                return _children;
            }
        }

        protected Directory _directory { get { return _entity as Directory; } }

        public DirectoryViewModel(Directory directory, DirectoryViewModel parent, bool isLazy)
            : base(directory, parent)
        {
            _children = new ObservableCollection<FileSystemEntityViewModel>();
            _isLazy = isLazy;
            _isLoaded = false;

            if (isLazy)
            {
                _children.Add(DummyChild);
            }
            else
            {
                Load();
                _isLoaded = true;
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
                if (_isExpanded && !_isLoaded)
                {
                    if (_isLazy && _children.Count > 0)
                        _children.RemoveAt(0);
                    Load();
                }
                if(!_isExpanded)
                {
                    foreach(var child in _children)
                    {
                        if (child is DirectoryViewModel)
                            (child as DirectoryViewModel).IsExpanded = false;
                    }
                }
            }
        }

        public void Load()
        {
            _children.Clear();
            foreach (FileSystemEntity entity in _directory.GetChilds())
            {
                if (entity is File)
                {
                    _children.Add(new FileViewModel(entity as File, this));
                }
                else
                {
                    _children.Add(new DirectoryViewModel(entity as Directory, this, _isLazy));
                }
            }
            _isLoaded = true;
        }

        public void Insert(FileSystemEntityViewModel entity, int index)
        {
            FileSystemEntity inserted = _directory.InsertChild(entity.Entity, index);
            FileSystemEntityViewModel model;
            if (inserted is File)
                model = new FileViewModel(inserted as File, this);
            else
                model = new DirectoryViewModel(inserted as Directory, this, _isLazy);

            _children.Insert(index, model);
        }

        public void Delete(FileSystemEntityViewModel entity)
        {
            _directory.RemoveChild(entity.Entity);
        }
    }
}
