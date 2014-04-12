using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MultimediaManager.Core.FileSystemTreeViews
{

    public static class TreeViewHelper
    {
        public static FileSystemEntityViewModel GetObjectAtPoint(TreeView control, Point p)
        //where ItemContainer : DependencyObject
        {
            // ItemContainer - can be ListViewItem, or TreeViewItem and so on(depends on control)
            TreeViewItem obj = GetContainerAtPoint(control, p);
            if (obj == null)
                return null;

            return obj.DataContext as FileSystemEntityViewModel;
        }

        public static TreeViewItem GetContainerAtPoint(TreeView control, Point p)
        //where ItemContainer : DependencyObject
        {
            HitTestResult result = VisualTreeHelper.HitTest(control, p);
            DependencyObject obj = result.VisualHit;

            while (VisualTreeHelper.GetParent(obj) != null && !(obj is TreeViewItem))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            // Will return null if not found
            return obj as TreeViewItem;
        }
    }
}
