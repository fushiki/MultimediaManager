using MultimediaManager.Core;
using MultimediaManager.Core.Modules;
using MultimediaManager.Mp3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Managment
{
    public sealed class Program:IProgram
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
            CoreSettings.Instance.Initialize(this);

        }



        public string[] GetPartitions()
        {
            //String exepath = System.AppDomain.CurrentDomain.BaseDirectory;
            //HARDCODED FOR ITERATION 1
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<String> partitions = new List<string>();
            foreach(var di in allDrives)
            {
                if(di.IsReady)
                {
                    partitions.Add(di.Name);
                }
            }
            return partitions.ToArray();
        }

        public IList<Module> GetModules()
        {
            var list = new List<Module>();
            var musicmodule = new MusicModule();
            musicmodule.IsInstalled = true;
            list.Add(musicmodule);
            return list;
        }

        public void DisposeModules()
        {
            foreach(Module m in CoreSettings.Instance.Modules.Values)
            {
                m.Dispose();
            }
            CoreSettings.Instance.Modules.Clear();
        }
    }
}
