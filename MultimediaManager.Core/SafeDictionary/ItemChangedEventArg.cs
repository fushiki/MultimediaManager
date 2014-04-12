using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaManager.Core.SafeDictionary
{

    public enum ChangeType
    {
        Delete,
        Insert,
        Changed
    }

    public class ItemChangedEventArg<TKey, TValue> : EventArgs
    {
        public TKey NewKey { get; private set; }
        public TKey OldKey { get; private set; }
        public TValue NewValue { get; private set; }
        public TValue OldValue { get; private set; }

        public ChangeType Type { get; private set; }

        public ItemChangedEventArg(TKey oldk, TValue oldv, TKey newk, TValue newv, ChangeType type)
        {
            NewKey = newk;
            NewValue = newv;
            OldKey = oldk;
            OldValue = oldv;
            Type = type;

        }

    }
}
