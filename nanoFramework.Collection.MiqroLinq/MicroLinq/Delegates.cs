using System;

namespace System.Collections
{
    public delegate object ActionWithReturn(object o);
    public delegate object Aggregate(object ob1, object ob2);
    public delegate int Comparer(object ob1, object ob2);
    public delegate bool Predicate(object o);
    public delegate object Selector(object o); // originally IComparable but System.String isn't marked(!)
}
