using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public abstract class FileSystemEntity
    {
        #region Fields
        protected string _path;
        #endregion

        #region Properties
        public string Path { get { return _path; } }

        #region Abstract Properties
        public abstract bool Exists { get; }
        public abstract string Name { get; set; }
        public abstract string DisplayName { get; }
        public abstract string Extension { get; }

        #endregion

        #endregion


        #region Abstract Methods
        public abstract bool Materialize();
        public abstract bool Delete();

        //public abstract void Move();
        #endregion

        #region Constructors
        protected FileSystemEntity() { _path = String.Empty; }
        protected FileSystemEntity(string path) { _path = path; }
        #endregion


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is FileSystemEntity)) return false;
            FileSystemEntity entity = obj as FileSystemEntity;
            return _path.Equals(entity._path) && Name.Equals(entity.Name);
        }

        public static File CreateFileReference(string path)
        {
            return FileCreator.Instance.CreateFileReference(path);
            //return new JKAudioFile(path);
        }
    }
}
