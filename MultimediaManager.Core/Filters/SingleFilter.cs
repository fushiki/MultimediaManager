using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Filters
{
    public class SingleFilter<T>:Filter<T>
    {
        Predicate<T> _predicate;

        public SingleFilter(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            _predicate = predicate;
        }
        public override bool Execute(T entity)
        {
            return _predicate(entity);
        }
        public override Filter<T> Clone()
        {
            return new SingleFilter<T>(_predicate);
        }
    }
}
