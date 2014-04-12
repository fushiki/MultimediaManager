using MultimediaManager.Core.FileSystemTreeViews;
using System;
using System.Collections.Generic;

namespace MultimediaManager.Managment
{

    public sealed class TreeViewManager
    {
        #region Singleton
        private static TreeViewManager _instance;

        public static TreeViewManager Instance
        {
            get { return _instance; }
        }

        static TreeViewManager() { _instance = new TreeViewManager(); }

        #endregion

        FileSystemTreeViewModel _physicalTreeView;
        Dictionary<String,FileSystemTreeViewModel> _views;

        public FileSystemTreeViewModel PhysicalTreeView { get { return _physicalTreeView; } }

        private TreeViewManager() 
        {
            _physicalTreeView = createPhysicalModel();
            _views = createViews();
        }


        FileSystemTreeViewModel createPhysicalModel()
        {
            
            string[] partitions = Program.Instance.GetPartitions();
            FileSystemTreeViewModel model = new FileSystemTreeViewModel(FileSystemTreeType.Physical, partitions);
            return model;

        }

        Dictionary<String,FileSystemTreeViewModel> createViews()
        {
            //TODO
            return null;   
        }

        public FileSystemTreeViewModel CreateTemporaryFileSystemTreeViewModel()
        {
            FileSystemTreeViewModel model = new FileSystemTreeViewModel
                (FileSystemTreeType.Temporary, new string[] { "Root" });
            return model;
        }

        
    }
}
