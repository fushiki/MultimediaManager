using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultimediaManager.Core.SafeDictionary
{
    public delegate void ItemChangedHandler<TKey, TValue>(object sender, ItemChangedEventArg<TKey, TValue> arg);
    public class SafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private bool _blocked;
        public event EventHandler Cleared;
        public event ItemChangedHandler<TKey, TValue> ItemChanged;
        Dictionary<TKey, TValue> _dic = new Dictionary<TKey, TValue>();
        readonly object sync = new object();

        public SafeDictionary()
        {
            _blocked = false;
        }

        protected void OnItemChanged(ItemChangedEventArg<TKey,TValue> args)
        {
            if (ItemChanged != null)
                ItemChanged(this, args);
        }
        protected void OnCleared()
        {
            if (Cleared != null)
                Cleared(this, EventArgs.Empty);
        }
        public void Add(TKey key, TValue value)
        {
            lock (sync)
            {
                _dic.Add(key, value);
            }
            var arg = new ItemChangedEventArg<TKey, TValue>(key, value, key, value, ChangeType.Insert);
            OnItemChanged(arg);
        }

        public bool ContainsKey(TKey key)
        {
            bool result = false;
            lock(sync)
            {
                result = _dic.ContainsKey(key);
            }
            return result;
        }

        
        public ICollection<TKey> Keys
        {
            get 
            {  
                ICollection<TKey> collection = null;
                lock(sync)
                {
                    collection = _dic.Keys;
                }
                return collection;
            }
        }

        public bool Remove(TKey key)
        {
            TValue value;
            bool result;
            ItemChangedEventArg<TKey, TValue> eventarg = null;
            lock(sync)
            {
                result = _dic.TryGetValue(key, out value);
                if(result)
                {
                    if(result = _dic.Remove(key))
                        eventarg = new ItemChangedEventArg<TKey, TValue>(key, value, key, value, ChangeType.Delete);
                }
            }
            if(eventarg!=null)
            {
                OnItemChanged(eventarg);
            }
            return result;

        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            bool result;
            lock(sync)
            {
                result = _dic.TryGetValue(key, out value);
            }
            return result;
        }

        public ICollection<TValue> Values
        {
            get 
            {
                ICollection<TValue> collection = null;
                lock (sync)
                {
                    collection = _dic.Values;
                }
                return collection;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue ret = default(TValue);
                Exception ex = null;
                lock (sync) 
                {
                    try
                    {
                        ret = _dic[key];
                    }catch(Exception e)
                    {
                        ex = e;
                    }
                }
                if (ex != null)
                    throw ex;
                return ret;
            }
            set
            {
                TValue old = default(TValue);
                ItemChangedEventArg<TKey, TValue> eventarg = null;
                bool result;
                lock (sync)
                {
                    result = _dic.TryGetValue(key, out old);
                    
                    if (result )
                    {
                        if (!old.Equals(value))
                        {
                            _dic[key] = value;
                            eventarg = new ItemChangedEventArg<TKey, TValue>(key, old, key, value, ChangeType.Changed);
                        }
                    }else
                    {
                        _dic[key] = value;
                        eventarg = new ItemChangedEventArg<TKey, TValue>(key, value, key, value, ChangeType.Insert);
                    }
                }
                if(eventarg!=null)
                {
                    OnItemChanged(eventarg);
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            lock(sync)
            {
                _dic.Clear();
            }
            OnCleared();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            bool result = false;
            lock (sync)
            {
                result = _dic.Contains(item);
            }
            return result;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock(sync)
            {
                (_dic as IDictionary<TKey,TValue>).CopyTo(array,arrayIndex);
            }
        }

        public int Count
        {
            get { return _dic.Count; }
        }

        public bool IsReadOnly
        {
            get { return (_dic as IDictionary<TKey, TValue>).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new SynchronizedEnumerator<TKey, TValue>(sync, this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SynchronizedEnumerator<TKey, TValue>(sync, this);
        }

        private class SynchronizedEnumerator<TKey, TValue>:IEnumerator<KeyValuePair<TKey,TValue>>
        {
            object _sync;
            SafeDictionary<TKey, TValue> _sdic;
            IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;
            bool _failed;

            public bool IsFailed
            {
                get { return _failed; }
            }
            public SynchronizedEnumerator(object sync, SafeDictionary<TKey, TValue> sdic)
            {
                _sync = sync;
                _sdic = sdic;
                _enumerator = _sdic._dic.GetEnumerator();

            }

            private void checkAndAcquire()
            {
                if (_failed) throw new InvalidOperationException("Action on failed enumerator.");
                if (!Monitor.IsEntered(_sync))
                    Monitor.Enter(_sync);
            }
            public KeyValuePair<TKey, TValue> Current
            {
                get 
                { 
                    checkAndAcquire();
                    KeyValuePair<TKey, TValue> pair = default( KeyValuePair<TKey, TValue>);
                    Exception ex = null;
                    try
                    {
                        pair = _enumerator.Current;
                    }
                    catch (Exception e)
                    {
                        ex = e;
                    }
                    if(ex!=null)
                    {
                        _failed = true;
                        Monitor.Exit(_sync);
                        throw ex;
                    }
                    return pair;
                }
            }

            public void Dispose()
            {
                Monitor.Exit(_sync);
                _enumerator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                checkAndAcquire();
                bool result = false;
                Exception ex = null;
                try
                {
                    result = _enumerator.MoveNext();
                }catch(Exception e)
                {
                    ex = e;
                }
                if (ex != null)
                {
                    _failed = true;
                    Monitor.Exit(_sync);
                    throw ex;
                }
                return result;
            }

            public void Reset()
            {
                checkAndAcquire();
                Exception ex = null;
                try
                {
                     _enumerator.Reset();
                }
                catch (Exception e)
                {
                    ex = e;
                }
                if (ex != null)
                {
                    _failed = true;
                    Monitor.Exit(_sync);
                    throw ex;
                }
            }
        }

        public void Block()
        {
            if (_blocked)
                return;
            _blocked = true;
            Monitor.Enter(sync);
        }

        public void Release()
        {
            if (!_blocked)
                return;
            
            Monitor.Exit(sync);
            _blocked = false;
        }
    }
}
