using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core
{
    public static class Logger
    {
        public static void Error(Exception ex) { Logger.Error(ex.Message); }
        public static void Error(String str) { }
    }
}
