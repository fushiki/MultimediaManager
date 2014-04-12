using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public abstract class ImageFile : File
    {
        protected ImageFile(string path) : base(path) { }
    }
}
