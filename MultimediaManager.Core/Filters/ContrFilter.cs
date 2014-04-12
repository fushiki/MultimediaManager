using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Filters
{
    public class ContrFilter<T>:Filter<T>
    {
        Filter<T> _innerFilter;
        public ContrFilter(Filter<T> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            _innerFilter = filter;
        }

        public override bool Execute(T entity)
        {
            return !_innerFilter.Execute(entity);
        }

        public override Filter<T> Clone()
        {
            return new ContrFilter<T>(_innerFilter.Clone());
        }
    }
}
