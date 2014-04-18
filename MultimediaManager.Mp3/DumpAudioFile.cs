using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Mp3
{
    public class DumpAudioFile : AudioFile
    {

        public DumpAudioFile() : base("\\dumpaudiofile.mp3") { }

        public override string Title
        {
            get { return String.Empty; }
            set { }
        }

        public override string Artist
        {
            get { return String.Empty; }
            set { }
        }

        public override void WriteData(System.IO.FileStream stream)
        {
            throw new NotImplementedException("DumpAudioFile WriteData");
        }

        public override void Save()
        {
            
        }

 

        public override System.IO.Stream SongStream
        {
            get { throw new NotImplementedException(); }
        }
    }
}
