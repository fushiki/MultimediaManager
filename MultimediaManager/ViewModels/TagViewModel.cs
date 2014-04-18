using MultimediaManager.Core;
using MultimediaManager.Core.FileSystem;
using MultimediaManager.Mp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.ViewModels
{
    public class TagViewModel:BaseViewModel
    {
        private static DumpAudioFile _dumpAudioFile = new DumpAudioFile();

        AudioFile _audioFile;

        public String Title
        {
            get { return _audioFile.Title; }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Equals(_audioFile.Title))
                    return;
                _audioFile.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public String Artist
        {
            get { return _audioFile.Artist; }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Equals(_audioFile.Artist))
                    return;
                _audioFile.Artist = value;
                OnPropertyChanged("Artist");
            }
        }

        public TagViewModel(AudioFile audiofile)
        {
            _audioFile = audiofile;
        }

        private TagViewModel()
        {
            _audioFile = _dumpAudioFile;
        }

        public static TagViewModel Empty { get { return new TagViewModel(); } }

    }
}
