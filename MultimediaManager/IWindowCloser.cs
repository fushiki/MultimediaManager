using MultimediaManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultimediaManager
{
    public interface IWindowCloser
    {
        WindowViewModel ParentWindow { get; }
    }
}
