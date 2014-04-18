using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Mp3
{
    public class TaglibAudioFile:TagerAudioFile
    {
        public TaglibAudioFile(string path) : base(path) { }
        protected override Tager CreateTager()
        {
            return new TaglibTager(_path);
        }


    }
}
