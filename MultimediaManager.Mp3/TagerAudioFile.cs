using MultimediaManager.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Mp3
{
    public abstract class  TagerAudioFile:AudioFile
    {
        private Tager _tager;
        private MemoryStream _memoryStream;

        public override Stream SongStream
        {
            get { return _memoryStream; }
        }
        protected Player Player
        {
            get
            {
                return (CoreSettings.Instance.Settings[MusicModule.SettingsProperty] as Settings)
                    .Player;
            }
        }
        protected Tager Tager { get { return _tager; } }

        protected TagerAudioFile(string filename):base(filename)
        {
            
            _memoryStream = new MemoryStream();
            /*
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                byte[] buffer = new byte[4 * 1024];
                int readed = buffer.Length;
                while (readed == buffer.Length)
                {
                    readed = fs.Read(buffer, 0, buffer.Length);
                    _memoryStream.Write(buffer, 0, readed);
                }
            }
             */
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                byte[] buffer = new byte[32768];
                int read;
                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    _memoryStream.Write(buffer, 0, read);
                }
            }
            _tager = CreateTager();
        }

        protected abstract Tager CreateTager();




        public override string Title
        {
            get
            {
                return _tager.Title;
            }
            set
            {
                _tager.Title = value;
            }
        }

        public override string Artist
        {
            get
            {
                return _tager.Artist;
            }
            set
            {
                _tager.Artist = value;
            }
        }


        public override void Save()
        {
            _tager.Save();
        }

        public override void WriteData(FileStream stream)
        {
            _memoryStream.WriteTo(stream);
        }
        public override void Dispose()
        {
            _tager.Dispose();            
        }
    }
}
