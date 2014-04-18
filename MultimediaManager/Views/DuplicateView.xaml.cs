using MultimediaManager.Core;
using MultimediaManager.Mp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaManager.Views
{
    /// <summary>
    /// Interaction logic for DuplicateView.xaml
    /// </summary>
    public partial class DuplicateView : UserControl
    {
        private Player player;

        private void check()
        {
            if(player == null)
            {
                player = (Player)CoreSettings.Instance.GlobalObjects[MusicModule.KeyPlayer];
                string file = "C:\\Temp\\test - Copy.mp3";
                MultimediaManager.Mp3.TaglibAudioFile afile = new MultimediaManager.Mp3.TaglibAudioFile(file);

                player.LoadSong(afile.SongStream);
            }
        }
        public DuplicateView()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            check();
            float f = (float)(e.NewValue / 100);
            player.Rewind(f);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            check();
            player.Stop();
        }

        /// <summary>
        /// Play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            check();
            player.Play();
        }
    }
}
