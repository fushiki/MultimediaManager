using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Filters
{
    public class CastFilter<T,V>:Filter<T>
    {

        private Filter<V> _inner;
        private Func<T,V> _function;
        public CastFilter(Filter<V> inner, Func<T,V> cast_function)
        {
            if (inner == null)
                throw new ArgumentNullException("Inner filter.");
            if (cast_function == null)
                throw new ArgumentNullException("Cast function.");
            _inner = inner;
            _function = cast_function;
        }
        public override bool Execute(T entity)
        {
            V obj = _function(entity);
            return _inner.Execute(obj);
        }
        public override Filter<T> Clone()
        {
            return new CastFilter<T, V>(_inner.Clone(), _function);
        }
    }
}
