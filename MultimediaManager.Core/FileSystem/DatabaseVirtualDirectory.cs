using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public class DatabaseVirtualDirectory : VirtualDirectory
    {
        
        protected ITreeDatabase Database 
        { 
            get
            {
                return CoreSettings.Instance.Database;
            }
        }
        public DatabaseVirtualDirectory(string path, string name, long key)
            : base(path, name)
        {
            
        }


        public override IList<FileSystemEntity> GetChilds()
        {
            IList<FileSystemEntity> list = new List<FileSystemEntity>();
            var childs = Database.GetChilds(Key);
            foreach(var ch in childs)
            {
                if(ch.IsFile)
                {
                    list.Add(File.CreateFileReference(ch.Path));
                }else
                {
                    list.Add(new DatabaseVirtualDirectory(ch.Path, ch.Name, ch.ID));
                }
            }
            return list;
        }
        public override FileSystemEntity InsertChild(FileSystemEntity entity,int index)
        {
            
            return null;
        }
        public override FileSystemEntity InsertChild(FileSystemEntity entity)
        {
            return null;
        }

        public override void RemoveChild(FileSystemEntity entity)
        {
            
        }
    }
}
