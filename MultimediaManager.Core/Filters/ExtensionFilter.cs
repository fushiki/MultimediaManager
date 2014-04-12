using MultimediaManager.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Filters
{
    public class ExtensionFilter : Filter<FileSystemEntity>
    {
        string[] _extensions;
        bool _acceptFolders;

        public ExtensionFilter(string[] extensions, bool acceptFolders)
        {
            if (extensions == null)
                throw new ArgumentNullException("Extensions.");
            _extensions = extensions;
            _acceptFolders = acceptFolders;
        }

        public override bool Execute(FileSystemEntity entity)
        {
            if (_acceptFolders && entity is Directory)
                return true;
            foreach (string ext in _extensions)
            {
                if (entity.Extension.Equals(ext))
                    return true;
            }
            return false;
        }
        public override Filter<FileSystemEntity> Clone()
        {
            return new ExtensionFilter(_extensions.ToArray(), _acceptFolders);
        }
    }
}

