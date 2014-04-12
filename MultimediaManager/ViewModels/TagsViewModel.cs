
using MultimediaManager.Core;
using MultimediaManager.Core.FileSystemTreeViews;
using MultimediaManager.Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.ViewModels
{
    public class AS
    {
        public String z { get { return "Asdss"; } }
    }
    public class TagsViewModel:BaseViewModel,IUsingFileSystemTree
    {

        public String TestS { get { return "Ala"; } }
        public AS X { get { return new AS(); } }
        public String Y { get { return new AS().z; } }
        public FileSystemTreeViewModel Physical 
        { 
            get { return TreeViewManager.Instance.PhysicalTreeView; } 
        }

        FileSystemTreeViewModel _temporary;

        public FileSystemTreeViewModel Temporary
        {
            get { return _temporary; }
        }

        private TagViewModel _tagViewModel;

        public TagViewModel Tag
        {
            get { return _tagViewModel; }
            protected set
            {
                if (value == null || _tagViewModel.Equals(value))
                    return;
                _tagViewModel = value;
                OnPropertyChanged("Tag");

                
            }
        }
        
        public TagsViewModel()
        {
            _temporary = TreeViewManager.Instance.CreateTemporaryFileSystemTreeViewModel();
            _tagViewModel = TagViewModel.Empty;
        }

    }
}
