using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaManager.Core.FileSystemTreeViews
{
    /// <summary>
    /// Interaction logic for FileSystemTreeView.xaml
    /// </summary>
    public partial class FileSystemTreeView : UserControl
    {
        private Point startPoint;
        private DragDropEffects effect;
        private FileSystemTreeViewModel _model;
        //public bool HasModel { get { return _model != null; } }

        public FileSystemTreeViewModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                treeView.DataContext = value;
            }
        }

        public FileSystemTreeView()
        {
            InitializeComponent();
        }



        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!HasModel) return;
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            Console.WriteLine(diff.ToString() + " " + SystemParameters.MinimumHorizontalDragDistance + " " + SystemParameters.MinimumVerticalDragDistance);
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                FileSystemEntityViewModel entity = (treeView.SelectedItem) as FileSystemEntityViewModel;
                if (entity != null)
                {
                    DataObject dragData = new DataObject("treeViewEntity", entity);

                    DragDrop.DoDragDrop(treeView, dragData, effect);
                }
            }
        }

        private void treeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                effect = DragDropEffects.Copy;
            else
                effect = DragDropEffects.Move;
        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("treeViewEntity"))
            {
                FileSystemEntityViewModel entity = e.Data.GetData("treeViewEntity") as FileSystemEntityViewModel;
                TreeView tree = sender as TreeView;
                Point p = e.GetPosition(tree);
                FileSystemTreeViewModel treemodel = tree.DataContext as FileSystemTreeViewModel;
                FileSystemEntityViewModel hited = TreeViewHelper.GetObjectAtPoint(tree, p);
                if (hited == null)
                    return;
                if (hited is DirectoryViewModel)
                {
                    (hited as DirectoryViewModel).Insert(entity, 0);
                }
                else
                {
                    hited.Parent.Insert(entity, 0);
                }
            }
        }

        private void treeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                effect = DragDropEffects.Copy;
            else
                effect = DragDropEffects.Move;
        }
    }
}
