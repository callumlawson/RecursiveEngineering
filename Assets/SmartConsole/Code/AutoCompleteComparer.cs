using System.Collections.Generic;

namespace Assets.SmartConsole.Code
{
    public class AutoCompleteComparer : IComparer<string>
    {
        public string LowerBound { get; private set; }
        public string UpperBound { get; private set; }

        public int Compare(string x, string y)
        {
            var comparison = Comparer<string>.Default.Compare(x, y);

            if (comparison >= 0)
            {
                LowerBound = y;
            }

            if (comparison <= 0)
            {
                UpperBound = y;
            }

            return comparison;
        }

        public void Reset()
        {
            LowerBound = null;
            UpperBound = null;
        }
    }
}
