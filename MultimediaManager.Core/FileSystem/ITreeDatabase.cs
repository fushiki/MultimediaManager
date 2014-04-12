using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public interface ITreeDatabase
    {
        IList<Tuple<string, string>> GetChilds(DatabaseVirtualDirectory dir);
        long InsertChild(DatabaseVirtualDirectory dir, string path, int index);
        void RemoveChild(DatabaseVirtualDirectory dir, string path);
        void RemoveChildDir(DatabaseVirtualDirectory dir, long key);


        long InsertChild(DatabaseVirtualDirectory databaseVirtualDirectory, string p);
    }
}
