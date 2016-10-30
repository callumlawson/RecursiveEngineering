using System.Collections.Generic;

namespace Assets.SmartConsole.Code
{
    public class AutoCompleteDictionary<T> : SortedDictionary<string, T>
    {
        private readonly AutoCompleteComparer comparer;

        public AutoCompleteDictionary()
            : base(new AutoCompleteComparer())
        {
            comparer = Comparer as AutoCompleteComparer;
        }

        public T LowerBound(string lookupString)
        {
            comparer.Reset();
            ContainsKey(lookupString);
            return this[comparer.LowerBound];
        }

        public T UpperBound(string lookupString)
        {
            comparer.Reset();
            ContainsKey(lookupString);
            return this[comparer.UpperBound];
        }

        public T AutoCompleteLookup(string lookupString)
        {
            comparer.Reset();
            ContainsKey(lookupString);
            var key = comparer.UpperBound == null ? comparer.LowerBound : comparer.UpperBound;
            return this[key];
        }
    }
}