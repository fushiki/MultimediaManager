using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public class TemporaryVirtualDirectory : VirtualDirectory
    {
        readonly IList<FileSystemEntity> _childs;


        public TemporaryVirtualDirectory(string path, string name)
            : base(path, name)
        {
            _childs = new List<FileSystemEntity>();
        }

        public override IList<FileSystemEntity> GetChilds()
        {
            return _childs.ToList();
        }

        public override FileSystemEntity InsertChild(FileSystemEntity entity)
        {
            return InsertChild(entity, _childs.Count);
        }
        public override FileSystemEntity InsertChild(FileSystemEntity entity, int index)
        {
            if (index < 0 || index > _childs.Count) throw new ArgumentOutOfRangeException("Index");
            if (entity is File)
            {
                _childs.Insert(index, entity);
                return entity;
            }
            else
            {
                TemporaryVirtualDirectory dir = null;
                //if (!(entity is TemporaryVirtualDirectory))
                    dir = new TemporaryVirtualDirectory(entity.Path, entity.Name);
                //else
                    //dir = entity as TemporaryVirtualDirectory;
                    _childs.Insert(index, dir);
                    foreach(FileSystemEntity e in ((Directory)entity).GetChilds())
                    {
                        dir.InsertChild(e);
                    }
                
                return dir;
            }
        }

        public override void RemoveChild(FileSystemEntity entity)
        {
            _childs.Remove(entity);
        }

    }
}
