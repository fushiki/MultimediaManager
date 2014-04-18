using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Mp3
{
    public class NAudioPlayer:Player
    {
        private WaveStream _blockAlignedStream;
        private WaveOut _waveOut;
        private bool _loaded = false;
        private bool _playing { get { return _waveOut != null && _waveOut.PlaybackState == PlaybackState.Playing; } }
        public override void Play()
        {
            if (!_loaded) throw new InvalidOperationException("Player not loaded.");
            _waveOut.Play();
            
        }

        public override TimeSpan TotalTime
        {
            get
            {
                if (!_loaded) throw new InvalidOperationException("Player not loaded.");
                return _blockAlignedStream.TotalTime;
            }
        }


        public override void Stop()
        {
            if (!_loaded) throw new InvalidOperationException("Player not loaded.");
            _waveOut.Stop();
        }

        public override void Rewind(float percent)
        {
            if(percent<0.00001 || percent > 1.00000 ) throw 
                new ArgumentOutOfRangeException("Prercent must be higher than 0.00001 and lower than 1.0000");
            long position = (long)((double)_blockAlignedStream.Length * (double)percent);
            if(position>_blockAlignedStream.Length-1) position = _blockAlignedStream.Length-1;
            if(position<0) position = 0;
            SetPosition(position);
        }
        private void SetPosition(long position)
        {
            long adj = position % _blockAlignedStream.WaveFormat.BlockAlign;
            long newPos = Math.Max(0, Math.Min(_blockAlignedStream.Length, position - adj));
            _blockAlignedStream.Position = newPos;
        }

        private void SetPosition(double seconds)
        {
            SetPosition((long)(seconds * _blockAlignedStream.WaveFormat.AverageBytesPerSecond));
        }
        public override void DisposeSong()
        {
            if(_loaded)
            {
                if(_playing)
                {
                    _waveOut.Stop();
                }
                _waveOut.Dispose();
                _blockAlignedStream.Dispose();
                _waveOut = null;
                _blockAlignedStream = null;
            }
        }

        public override void Dispose()
        {
            DisposeSong();
        }

        public override void LoadSong(System.IO.Stream songstream)
        {
            songstream.Position = 0;
            var mp3filereader = new Mp3FileReader(songstream);
            var pcmstream = WaveFormatConversionStream.CreatePcmStream(mp3filereader);
            //DisposeSong();
            _blockAlignedStream =new BlockAlignReductionStream(pcmstream);
            if(_waveOut!=null)
            {
                _waveOut.PlaybackStopped -= OnPlaybackStopped;
            }
            _waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            _waveOut.Init(_blockAlignedStream);
            _waveOut.PlaybackStopped += OnPlaybackStopped;
            _loaded = true;
        }

        public void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
                MultimediaManager.Core.Logger.Error(e.Exception);
        }
    }
}
