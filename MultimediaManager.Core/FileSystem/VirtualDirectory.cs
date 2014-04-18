using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public abstract class VirtualDirectory : Directory
    {
        protected string _name;

        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public override string DisplayName
        {
            get { return _name; }
        }
        public VirtualDirectory(string path, string name) : base(path){ _name = name; }

    }
}
