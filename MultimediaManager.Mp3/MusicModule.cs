using MultimediaManager.Core.Modules;
using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaManager.Core.Filters;
using MultimediaManager.Core;


namespace MultimediaManager.Mp3
{
    public class MusicModule : Module
    {
        public readonly static String KeyPlayer = "MultimediaManager.Mp3.Player";
        public readonly static String Mp3_Extension = ".mp3";
        public readonly static Filter<FileSystemEntity> Mp3ExtensionFilter =
           new ExtensionFilter(new string[] { Mp3_Extension }, true);
        public readonly static String ModuleName = "Music Module";
        public readonly static String SettingsProperty = "MultimediaManager.Mp3.Settings";
        private List<Filter<FileSystemEntity>> _filters = new List<Filter<FileSystemEntity>>() { Mp3ExtensionFilter };
        private ISettings _settings;
        private Player _player;

        public Player Player { 
            get 
            {
                return _player; 
            } 
        }

        public MusicModule()
        {
            _settings = new Settings();
        }

        public override void Setup()
        {
            _player = new NAudioPlayer();
        }

        public override List<Filter<FileSystemEntity>> Filters
        {
            get { return _filters; }
        }
        
        public override IList<KeyValuePair<string, ISettings>> Settings
        {
            get
            {
                List<KeyValuePair<string, ISettings>> list = new List<KeyValuePair<string, ISettings>>();
                list.Add(new KeyValuePair<string, ISettings>(SettingsProperty, _settings));
                return list;
            }
        }
        public override List<string> Extensions
        {
            get { return new List<string>() { Mp3_Extension }; }
        }

        public override File CreateFileReference(string path)
        {
            if (System.IO.Path.GetExtension(path).Equals(Mp3_Extension))
            {
                return new ProxyAudioFile(path,CreateAudioFile);
            }
            return base.CreateFileReference(path);
        }

        public override string Name
        {
            get { return ModuleName; }
        }

        public AudioFile CreateAudioFile(string filename)
        {
            return new JKAudioFile(filename);
        }



        public override IList<KeyValuePair<string, object>> GlobalObjects
        {
            get 
            {
                IList<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
                list.Add(new KeyValuePair<string, object>(KeyPlayer, _player));
                return list;
            }
        }

        public override void Dispose()
        {
            _player.Dispose();
        }
    }
}


