using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Database
{
    public class TreeViewDatabasEntity
    {
        public long ID { get; set; }
        public int Index { get; set; }
        public String Path { get; set; }
        public bool IsFile { get; set; }
        public long ParentID { get; set; }

        public String Name { get; set; }

        public override string ToString()
        {
            return String.Format("{0} [{1}] {2} [[{3}]]",Name,ID,IsFile?"File":"Directory",Index);
        }

        public override bool Equals(object obj)
        {
            TreeViewDatabasEntity e = obj as TreeViewDatabasEntity;
            return e != null && e.ID == ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
