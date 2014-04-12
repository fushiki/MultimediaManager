using MultimediaManager.Managment;
using MultimediaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MMProperties = MultimediaManager.Properties;

namespace MultimediaManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Program.Instance.Initialize();
            MainWindow window = new MainWindow();
            var viewModel = new MainWindowViewModel(MMProperties.Resources.MainWindowTitle);
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
