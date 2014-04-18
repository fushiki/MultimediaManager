using MultimediaManager.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public sealed class FileCreator
    {
        private static FileCreator instance;

        public static FileCreator Instance
        {
            get { return instance; }
        }

        static FileCreator() { instance = new FileCreator(); }

        private FileCreator() {  }

        public File CreateFileReference(String path)
        {
            foreach(Module m in CoreSettings.Instance.Modules.Values)
            {
                if(m.IsExtensionRecognizable(path))
                {
                    return m.CreateFileReference(path);
                }
            }
            return new UnknownFile(path);
        }
    }
}
