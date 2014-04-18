using MultimediaManager.Core.FileSystem;
using MultimediaManager.Core.FileSystemTreeViews;
using System;
using System.Collections.Generic;

namespace MultimediaManager.Core
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
            
            string[] partitions = CoreSettings.Instance.Program.GetPartitions();
            FileSystemTreeViewModel model = new FileSystemTreeViewModel(FileSystemTreeType.Physical, partitions);
            return model;

        }

        Dictionary<String,FileSystemTreeViewModel> createViews()
        {
            FileSystemTreeViewModel _left = CreateTemporaryFileSystemTreeViewModel();
            //FileSystemTreeViewModel _right = Managment.TreeViewManager.Instance.CreateTemporaryFileSystemTreeViewModel();
            Directory d1, d2, d3;
            d1 = new TemporaryVirtualDirectory("", "Dir1");
            d2 = new TemporaryVirtualDirectory("", "Dir2");
            d3 = new TemporaryVirtualDirectory("", "Dir3");
            File f1, f2, f3, f4;
            f1 = new UnknownFile("\\f1notMp3.mp4");
            f2 = new UnknownFile("\\f2notMp3.mp4");
            f3 = new UnknownFile("\\f3IsMp3.mp3");
            f4 = new UnknownFile("\\f4IsMp3.mp3");
            d1.InsertChild(f1);
            d2.InsertChild(f2);
            d2.InsertChild(f3);
            d3.InsertChild(f4);

            d1.InsertChild(d2, 0);
            var d1c = d1.GetChilds();
            var chd1 = d1c[0];
            var lr = ((_left.Roots[0] as DirectoryViewModel).Entity as Directory);
            ((_left.Roots[0] as DirectoryViewModel).Entity as Directory).InsertChild(d1, 0);
            var chrl = lr.GetChilds();
            //((_right.Roots[0] as DirectoryViewModel).Entity as Directory).InsertChild(d3, 0);
            var dic = new Dictionary<string,FileSystemTreeViewModel>();
            dic [ "test" ] = _left;
            return dic;   
        }

        public FileSystemTreeViewModel CreateTemporaryFileSystemTreeViewModel()
        {
            FileSystemTreeViewModel model = new FileSystemTreeViewModel
                (FileSystemTreeType.Temporary, new string[] { "Root" });
            return model;
        }

        
    }
}
