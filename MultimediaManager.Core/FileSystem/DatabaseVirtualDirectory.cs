using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public class DatabaseVirtualDirectory : VirtualDirectory
    {
        ITreeDatabase _database;
        long _key;

        public DatabaseVirtualDirectory(string path, string name, long key)
            : base(path, name)
        {
            _key = key;
        }

        public override IList<FileSystemEntity> GetChilds()
        {
            throw new NotImplementedException();
        }
        public override FileSystemEntity InsertChild(FileSystemEntity entity)
        {
            long id = _database.InsertChild(this, entity.Path);
            if (entity is File) return entity;
            else
            {
                return new DatabaseVirtualDirectory(entity.Path, entity.Name, id);
            }
        }
        public override FileSystemEntity InsertChild(FileSystemEntity entity, int index)
        {
            long id = _database.InsertChild(this, entity.Path, index);
            if (entity is File) return entity;
            else
            {
                return new DatabaseVirtualDirectory(entity.Path, entity.Name, id);
            }
        }

        public override void RemoveChild(FileSystemEntity entity)
        {
            if (entity is DatabaseVirtualDirectory)
                _database.RemoveChildDir(this, (entity as DatabaseVirtualDirectory)._key);
            else
                _database.RemoveChild(this, entity.Path);
        }
    }
}
