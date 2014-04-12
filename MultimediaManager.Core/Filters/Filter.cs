using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Filters
{
    public abstract class Filter<T>
    {
        public static Filter<T> AcceptAllFilter()
        {
            return new SingleFilter<T>(x => true);
        }
        public abstract bool Execute(T entity);
        public virtual void Add(Filter<T> filter) { }
        public virtual void Remove(Filter<T> filter) { }
        public virtual IList<Filter<T>> InnerFilters { get { return null; } }

        public abstract Filter<T> Clone();

        public virtual Filter<T> ContrFilter()
        {
            return new ContrFilter<T>(this);
        }
    }
}
