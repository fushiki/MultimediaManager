using MultimediaManager.Core.FileSystem;
using MultimediaManager.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Modules
{
    public abstract class Module:IDisposable
    {
        public abstract List<Filter<FileSystemEntity>> Filters { get; }
        //public abstract List<DuplicateComparer> DuplicateComparers { get; }
        public abstract List<String> Extensions { get; }

        public bool IsInstalled { get; set; }
        public virtual File CreateFileReference(string path)
        {
            return null;
        }
        /// <summary>
        /// This method is called when settings are loaded.
        /// In this method shoudl be initialized objects based on settings.
        /// </summary>
        public abstract void Setup();

        public abstract IList<KeyValuePair<string, ISettings>> Settings { get; }
        public abstract IList<KeyValuePair<string, Object>> GlobalObjects { get; }

        public bool IsExtensionRecognizable(string path)
        {
            string ext = System.IO.Path.GetExtension(path);
            return Extensions.Contains(ext);

        }

        public abstract string Name { get; }

        public abstract void Dispose();

        public virtual BaseViewModel GetViewModel(File filetype)
        {
            return null;
        }

    }
}
