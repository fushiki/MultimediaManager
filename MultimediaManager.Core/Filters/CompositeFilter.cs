using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.Filters
{
    public class CompositeFilter<T>:Filter<T>
    {
        public enum CompositeType
        {
            AND,
            OR
        }
        IList<Filter<T>> _filters;
        Func<bool, bool, bool> _function;
        bool _seed;

        public CompositeFilter(CompositeType type)
        {
            switch(type)
            {
                case CompositeType.AND:
                    _function = (x, y) => x && y;
                    _seed = true;
                    break;
                case CompositeType.OR:
                    _function = (x, y) => x || y;
                    _seed = false;
                    break;
                default:
                    throw new ArgumentException("type");
            }
            _filters = new List<Filter<T>>();
        }

        public CompositeFilter(Func<bool,bool,bool> compositeFunction, bool seed)
        {
            if (compositeFunction == null)
                throw new ArgumentNullException("Composite Function.");
            _function = compositeFunction;
            _seed = seed;
            _filters = new List<Filter<T>>();
        }
        

        public override void Add(Filter<T> filter)
        {
            _filters.Add(filter);
        }
        public override IList<Filter<T>> InnerFilters
        {
            get
            {
                return _filters;
            }
        }
        public override void Remove(Filter<T> filter)
        {
            _filters.Remove(filter);
        }


        public override bool Execute(T entity)
        {
            return _filters.Aggregate<Filter<T>, bool>
                (_seed,
                (calculated, next) => _function(calculated, next.Execute(entity)
                    ));
        }

        public override Filter<T> Clone()
        {
            Filter<T> res = new CompositeFilter<T>(_function, _seed);
            foreach (var filter in _filters)
                res.Add(filter.Clone());
            return res;
        }
    }
}
