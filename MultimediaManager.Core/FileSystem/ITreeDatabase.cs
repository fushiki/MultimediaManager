using MultimediaManager.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public interface ITreeDatabase
    {

        IList<TreeViewDatabasEntity> GetChilds(long key);
        long InsertChild(TreeViewDatabasEntity child);
        void RemoveChild(TreeViewDatabasEntity child);

        long Insert(string treeName);

        long Insert(long nodeId, string treeName);

        long Insert(long nodeId, long treeId);


    }
}
