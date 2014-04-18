using MultimediaManager.Core.FileSystem;
using MultimediaManager.Core.Modules;
using MultimediaManager.Core.SafeDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core
{
    public class CoreSettings
    {
        #region Singleton
        private static CoreSettings instance;

        public static CoreSettings Instance
        {
            get { return instance; }
        }

        static CoreSettings() { instance = new CoreSettings(); }

        #endregion
        private CoreSettings() 
        {
            _settingsDictionary = new SafeDictionary<string, ISettings>();
            _globalObjects = new SafeDictionary<string, object>();
        }

        private Dictionary<string, Module> _extensionMap;
        public Dictionary<string, Module> ExtensionMap { get { return _extensionMap; } }
        private Dictionary<string, Module> _modules = new Dictionary<string, Module>() { };
        private IProgram _program;

        public ITreeDatabase Database { get { return null; } }
        public IProgram Program { get { return _program; } }

        private void LoadDictionary<T>(Module m,IList<KeyValuePair<string,T>> properties, IDictionary<string,T> dictionary)
        {
            bool has = false;
            StringBuilder logError = new StringBuilder(String.Format("Cannot load module {0}. Existring properties: ", m.Name));

            foreach (var prop in properties)
            {
                if (dictionary.ContainsKey(prop.Key))
                {
                    has = true;
                    logError.AppendLine(prop.Key);
                }
            }
            if (has)
            {
                Logger.Error(logError.ToString());
            }
            else
            {
                foreach (var prop in properties)
                {
                    dictionary.Add(prop);
                }
            }
        }
        public void Initialize(IProgram program)
        {
            _program = program;
            foreach(Module m in program.GetModules())
            {
                if (m.IsInstalled)
                {
                    _modules[m.Name]=m;
                    _settingsDictionary.Block();
                    LoadDictionary<ISettings>(m, m.Settings, _settingsDictionary);
                    _settingsDictionary.Release();
                    m.Setup();
                    _globalObjects.Block();
                    LoadDictionary<object>(m, m.GlobalObjects, _globalObjects);
                    _globalObjects.Release();
                }
            }
            _extensionMap[".mp3"] = _modules["Music Module"];
        }

        public Dictionary<string,Module> Modules
        {
            get { return _modules; }
        }
        
        #region Settings

        private SafeDictionary<string, ISettings> _settingsDictionary;
        private SafeDictionary<string, Object> _globalObjects;

        public IDictionary<string, object> GlobalObjects { get { return _globalObjects; } }
        public IDictionary<String, ISettings> Settings { get { return _settingsDictionary; } }
        
        #endregion
    }
}
