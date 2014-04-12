using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public abstract class Directory : FileSystemEntity
    {
        public override string Extension
        {
            get { return String.Empty; }
        }
        public override bool Exists
        {
            get { return System.IO.Directory.Exists(_path); }
        }
        public override bool Materialize()
        {
            if (Exists) return false;
            System.IO.Directory.CreateDirectory(_path);
            return true;
        }
        public override bool Delete()
        {
            if (!Exists) return false;
            System.IO.Directory.Delete(_path, true);
            return true;
        }

        public abstract IList<FileSystemEntity> GetChilds();

        public abstract FileSystemEntity InsertChild(FileSystemEntity entity);
        public abstract FileSystemEntity InsertChild(FileSystemEntity entity, int index);
        public abstract void RemoveChild(FileSystemEntity entity);
        protected Directory(string path) : base(path) { }
        protected Directory() : base() { }

    }
}
