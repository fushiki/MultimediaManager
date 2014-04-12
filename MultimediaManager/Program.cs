using MultimediaManager.Core;
using MultimediaManager.Core.Modules;
using MultimediaManager.Mp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Managment
{
    public sealed class Program
    {
        #region Singleton

        private static Program _instance;

        public static Program Instance
        {
            get { return _instance; }
        }
        static Program() { _instance = new Program(); }
        
        #endregion

        private Program()
        {
        }

        public void Initialize()
        {
            Settings.Instance.Initialize(GetModules());
        }



        public string[] GetPartitions()
        {
            String exepath = System.AppDomain.CurrentDomain.BaseDirectory;
            //HARDCODED FOR ITERATION 1
            return new String[] { exepath };
        }

        public IList<Module> GetModules()
        {
            var list = new List<Module>();
            var musicmodule = new MusicModule();
            musicmodule.IsInstalled = true;
            list.Add(musicmodule);
            return list;
        }
    }
}
