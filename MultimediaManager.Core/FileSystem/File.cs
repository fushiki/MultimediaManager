using MultimediaManager.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public abstract class File : FileSystemEntity,IDisposable
    {
        public abstract Module ParentModule { get; }
        public virtual void Dispose()
        {

        }
        public override string Extension
        {
            get { return System.IO.Path.GetExtension(_path); }
        }
        public override string Name
        {
            get { return System.IO.Path.GetFileName(_path); }
            set { throw new NotImplementedException(); }
        }
        public override string DisplayName
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(_path); }
        }
        public override bool Exists
        {
            get { return System.IO.File.Exists(_path); }
        }
        public override bool Materialize()
        {
            if (Exists) return false;
            System.IO.FileStream fs = System.IO.File.Create(_path);
            WriteData(fs);
            return true;
        }
        public void Save(string path)
        {
            System.IO.FileStream fs = System.IO.File.Create(path);
            WriteData(fs);
        }
        public override bool Delete()
        {
            if (!Exists) return false;
            System.IO.File.Delete(_path);
            return true;
        }

        public File(string path) : base(path) { }

        public abstract void WriteData(System.IO.FileStream stream);
    }
}
