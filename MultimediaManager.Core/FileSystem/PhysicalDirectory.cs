using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public class PhysicalDirecotry : Directory
    {
        public override string Name
        {
            get
            {
                string[] dirs = _path.Split(System.IO.Path.DirectorySeparatorChar);
                if (dirs.Length == 0) throw new Exception();
                if (dirs[dirs.Length - 1].Equals(String.Empty))
                {
                    if (dirs.Length == 1) throw new Exception();
                    return dirs[dirs.Length - 2];
                }
                return dirs[dirs.Length - 1];
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override string DisplayName
        {
            get { return Name; }
        }



        public override IList<FileSystemEntity> GetChilds()
        {
            if (!Exists) return new List<FileSystemEntity>();
            return (from dir in System.IO.Directory.GetFileSystemEntries(_path)
                    select
                    (System.IO.Path.GetExtension(dir).Equals(String.Empty) ? new PhysicalDirecotry(dir) as FileSystemEntity : CreateFileReference(dir) as FileSystemEntity)
                    ).ToList();
        }

        public override FileSystemEntity InsertChild(FileSystemEntity entity)
        {
            return InsertChild(entity, 0);
        }

        public override FileSystemEntity InsertChild(FileSystemEntity entity, int index)
        {
            string __path = System.IO.Path.Combine(_path, entity.Name);
            if (entity is File)
            {

                if (entity.Exists)
                {
                    System.IO.File.Copy(entity.Path, __path);
                }
                else
                {
                    (entity as File).Save(__path);

                }
                return CreateFileReference(__path);
            }
            else
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_path, entity.Name));
                return new PhysicalDirecotry(__path);
            }
        }

        public override void RemoveChild(FileSystemEntity entity)
        {
            if (System.IO.Directory.GetFileSystemEntries(_path).Contains(entity.Path))
                entity.Delete();
        }

        public PhysicalDirecotry(string path) : base(path) { }
        public PhysicalDirecotry() : base() { }

    }
}
