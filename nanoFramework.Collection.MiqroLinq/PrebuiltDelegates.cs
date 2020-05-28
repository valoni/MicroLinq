using System;

namespace System.Collections.MiqroLinq.PrebuiltDelegates
{
    public static class PrebuiltDelegates
    {
        public static class Aggregates
        {
            public static Aggregate StringConcat = (c1, c2) => string.Concat(c1, c2);
            public static Aggregate IntAdd = (i1, i2) => (int)i1 + (int)i2;
            public static Aggregate LongAdd = (l1, l2) => (long)l1 + (long)l2;
            public static Aggregate DoubleAdd = (d1, d2) => (double)d1 + (double)d2;
        }

        public static class Comparers
        {
            public static Comparer StringCompare = (s1, s2) => string.Compare((string)s1, (string)s2);
            public static Comparer StringCompareNearlyInsensitive = (s1, s2) => string.Compare(((string)s1).ToUpper(), ((string)s2).ToUpper());
            public static Comparer IntCompare = (i1, i2) => (int)i1 == (int)i2 ? 0 : (int)i1 < (int)i2 ? -1 : 1;
            public static Comparer LongCompare = (l1, l2) => (long)l1 == (long)l2 ? 0 : (long)l1 < (long)l2 ? -1 : 1;
            public static Comparer DoubleCompare = (l1, l2) => (double)l1 == (double)l2 ? 0 : (double)l1 < (double)l2 ? -1 : 1;
        }
    }
}
