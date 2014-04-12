
using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultimediaManager.Core.FileSystemTreeViews
{
    public abstract class FileSystemEntityViewModel : INotifyPropertyChanged
    {
        bool _isSelected;
        Visibility _visibility;
        bool _isPath;
        protected FileSystemEntity _entity;
        DirectoryViewModel _parent;
        public String Extension { get { return _entity.Extension; } }
        public FileSystemEntity Entity { get { return _entity; } }
        public DirectoryViewModel Parent { get { return _parent; } }

        public Visibility Visible
        {
            get { return _visibility; }
            set
            {
                if(value != _visibility)
                {
                    _visibility = value;
                    OnPropertyChanged("Visible");
                }
            }
        }

        protected FileSystemEntityViewModel(FileSystemEntity entity, DirectoryViewModel parent)
        {
            _entity = entity;
            _parent = parent;
        }


        public bool Exists { get { return _entity.Exists; } }

        public string Name
        {
            get { return _entity.DisplayName; }
        }

        public void Materialize()
        {
            if (_entity.Materialize())
            {
                OnPropertyChanged("Exists");
            }
        }

        public bool IsPath
        {
            get { return _isPath; }
            set
            {
                if(value != _isPath)
                {
                    _isPath = value;
                    OnPropertyChanged("IsPath");
                }
            }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));


        }
    }
}
