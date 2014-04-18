using MultimediaManager.Core.FileSystem;
using MultimediaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void HelpMenuItem_Click(object sender, EventArgs e)
        {

        }
        void ExitMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ((DataContext) as MainWindowViewModel).CurrentView = new TagsViewModel();
        }

        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {
            ((DataContext) as MainWindowViewModel).CurrentView = new DuplicateViewModel();
        }

        private void RibbonButton_Click_2(object sender, RoutedEventArgs e)
        {
            ((DataContext) as MainWindowViewModel).CurrentView = new TestDragAndDropViewModel();
        }

        private void RibbonButton_Click_3(object sender, RoutedEventArgs e)
        {
            string file = "C:\\Temp\\test - Copy.mp3";
            MultimediaManager.Mp3.TaglibAudioFile afile = new MultimediaManager.Mp3.TaglibAudioFile(file);
            MultimediaManager.Mp3.Player player = new MultimediaManager.Mp3.NAudioPlayer();
            player.LoadSong(afile.SongStream);
            player.Play();
        }

    }
}
