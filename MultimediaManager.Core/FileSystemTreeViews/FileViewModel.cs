using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystemTreeViews
{
    public class FileViewModel : FileSystemEntityViewModel
    {
        public FileViewModel(File file, DirectoryViewModel parent)
            : base(file, parent)
        {

        }
    }
}
