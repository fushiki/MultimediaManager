using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Mp3
{
    public abstract class Tager:IDisposable
    {
        public abstract String Artist { get; set; }
        public abstract String Title { get; set; }

        public abstract void Dispose();

        public abstract void Save();
    }
}
