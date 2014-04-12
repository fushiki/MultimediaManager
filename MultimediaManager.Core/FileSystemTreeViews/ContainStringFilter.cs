using MultimediaManager.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.FileSystemTreeViews
{
    public class ContainStringFilter:Filter<FileSystemEntityViewModel>
    {
        public String FilterString { get; set; }

        public ContainStringFilter(String filterString)
        {
            if (filterString == null)
                throw new ArgumentNullException("Filter String.");
            FilterString = filterString;
        }
        public override bool Execute(FileSystemEntityViewModel entity)
        {
            return entity.Name.Contains(FilterString);
        }

        public override Filter<FileSystemEntityViewModel> Clone()
        {
            return this.MemberwiseClone() as Filter<FileSystemEntityViewModel>;
        }
    }
}
