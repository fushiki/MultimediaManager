using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Mp3
{
    public class JKAudioFile : AudioFile
    {
        public JKAudioFile(string path) : base(path) { }

        public override void WriteData(System.IO.FileStream stream)
        {
            throw new NotImplementedException();
        }

        public override string Title
        {
            get
            {
                return "TestTitle";
            }
            set
            {

            }
        }

        public override string Artist
        {
            get
            {
                return "TestArtist";
            }
            set
            {

            }
        }
    }
}