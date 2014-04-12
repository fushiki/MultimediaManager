using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public abstract class AudioFile : File
    {
        public abstract String Title { get; set; }
        public abstract String Artist { get; set; }

        protected AudioFile(string path) : base(path) { }
    }
}
