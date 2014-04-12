using MultimediaManager.Core.FileSystem;
using MultimediaManager.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Modules
{
    public abstract class Module
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
        /// String is property name and object is default value
        /// </summary>
        public abstract IList<KeyValuePair<string, object>> Properties { get; }

        public bool IsExtensionRecognizable(string path)
        {
            string ext = System.IO.Path.GetExtension(path);
            foreach(string mext in Extensions)
            {
                if(mext.Equals(ext))
                {
                    return true;
                }
            }
            return false;
        }

        public abstract string Name { get; }
    }
}
