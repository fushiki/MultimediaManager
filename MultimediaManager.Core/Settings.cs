using MultimediaManager.Core.Modules;
using MultimediaManager.Core.SafeDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core
{
    public class Settings
    {
        private static Settings instance;

        public static Settings Instance
        {
            get { return instance; }
        }

        static Settings() { instance = new Settings(); }
        private Settings() 
        {
            _settingsDictionary = new SafeDictionary<string, object>(); 
        }


        private List<Module> _modules = new List<Module>() {  };

        public void Initialize(IList<Module> modules)
        {
            foreach(Module m in modules)
            {
                if (m.IsInstalled)
                {
                    _modules.Add(m);
                    var properties = m.Properties;
                    bool has = false;
                    StringBuilder logError =new StringBuilder( String.Format("Cannot load module {0}. Existring properties: ", m.Name));

                    _settingsDictionary.Block();
                    foreach(var prop in properties)
                    {
                        if(_settingsDictionary.ContainsKey(prop.Key))
                        {
                            has = true;
                            logError.AppendLine(prop.Key);
                        }
                    }
                    if(has)
                    {
                        Logger.Error(logError.ToString());
                    }
                    else
                    {
                        foreach(var prop in properties)
                        {
                            _settingsDictionary.Add(prop);
                        }
                    }
                    _settingsDictionary.Release();
                }
            }
        }

        public List<Module> Modules
        {
            get { return _modules; }
        }
        
        #region Settings

        private SafeDictionary<string, object> _settingsDictionary;

        public object this[string key]
        {
            get
            {
                return _settingsDictionary[key];
            }
            set
            {
                _settingsDictionary[key] = value;
            }
        }

        public IDictionary<String, Object> Properties { get { return _settingsDictionary; } }
        
        #endregion
    }
}
