using MultimediaManager.Core.Modules;
using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaManager.Core.Filters;


namespace MultimediaManager.Mp3
{
    public class MusicModule : Module
    {

        public readonly static Filter<FileSystemEntity> Mp3ExtensionFilter =
            new ExtensionFilter(new string[]{ ".mp3"},true);
        public readonly static String Mp3MaxBufferedSize = "MusicModule.Mp3MaxBufferedSize";
        public readonly static String ModuleName = "MusicModule";
        private static String MP3 = ".mp3";

        private List<Filter<FileSystemEntity>> _filters = new List<Filter<FileSystemEntity>>() { MusicModule.Mp3ExtensionFilter };

        public override List<Filter<FileSystemEntity>> Filters
        {
            get { return _filters; }
        }
        
        public override IList<KeyValuePair<string, object>> Properties
        {
            get
            {
                List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
                list.Add(new KeyValuePair<string, object>(Mp3MaxBufferedSize, 100));
                return list;
            }
        }
        public override List<string> Extensions
        {
            get { return new List<string>() { ".mp3" }; }
        }

        public override File CreateFileReference(string path)
        {
            if (System.IO.Path.GetExtension(path).Equals(MP3))
            {
                return new JKAudioFile(path);
            }
            return base.CreateFileReference(path);
        }

        public override string Name
        {
            get { return ModuleName; }
        }
    }
}


