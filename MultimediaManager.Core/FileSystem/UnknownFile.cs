using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystem
{
    public class UnknownFile:File
    {
        private string staticpath;
        public UnknownFile(string path):base(path)
        {
            staticpath = path;
        }
        public override void WriteData(FileStream stream)
        {
            byte[] buffer = new byte[4 * 1024];
            int readed = -1;
            using(FileStream sr = System.IO.File.OpenRead(staticpath))
            {
                while(readed!=0)
                {
                    readed = sr.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, readed);
                }
            }
        }
    }
}
