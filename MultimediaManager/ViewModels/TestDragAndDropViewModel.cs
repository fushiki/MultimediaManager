using MultimediaManager.Core;
using MultimediaManager.Core.FileSystem;
using MultimediaManager.Core.FileSystemTreeViews;
using MultimediaManager.Mp3;

namespace MultimediaManager.ViewModels
{
    public class TestDragAndDropViewModel:BaseViewModel
    {
        FileSystemTreeViewModel _left;
        FileSystemTreeViewModel _right;

        public FileSystemTreeViewModel Left { get { return _left; } }
        public FileSystemTreeViewModel Right { get { return _right; } }

        public TestDragAndDropViewModel()
        {
            _left = TreeViewManager.Instance.CreateTemporaryFileSystemTreeViewModel();
            _right = TreeViewManager.Instance.CreateTemporaryFileSystemTreeViewModel();
            Directory d1,d2,d3;
            d1 = new TemporaryVirtualDirectory("","Dir1");
            d2 = new TemporaryVirtualDirectory("","Dir2");
            d3 = new TemporaryVirtualDirectory("","Dir3");
            File f1, f2, f3, f4;
            f1 = new JKAudioFile("\\f1notMp3.mp4");
            f2 = new JKAudioFile("\\f2notMp3.mp4");
            f3 = new JKAudioFile("\\f3IsMp3.mp3");
            f4 = new JKAudioFile("\\f4IsMp3.mp3");
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
            ((_right.Roots[0] as DirectoryViewModel).Entity as Directory).InsertChild(d3, 0);
            
            //DirectoryViewModel dvm1 = new DirectoryViewModel(d1,_left.Roots[0] as DirectoryViewModel,true);
            //DirectoryViewModel dvm2 = new DirectoryViewModel(d2,dvm1,true);
            //DirectoryViewModel dvm3 = new DirectoryViewModel(d3,_right.Roots[0] as DirectoryViewModel,true);
            //FileViewModel f1vm = new FileViewModel(f1,dvm1);
            //FileViewModel f2vm = new FileViewModel(f1,dvm1);
            //FileViewModel f3vm = new FileViewModel(f1,dvm2);
            //FileViewModel f4vm = new FileViewModel(f1,dvm3);
            //_left.Roots.Add(dvm1);
            //_right.Roots.Add(dvm3);
        }

        
    }
}
