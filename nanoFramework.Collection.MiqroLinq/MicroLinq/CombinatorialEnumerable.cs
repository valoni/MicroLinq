using System.Collections;

namespace System.Collections.MiqroLinq
{
    /// <summary>
    /// Because the yield statement is broken for current C# and NETMF 4.1 we have to
    /// use a custom IEnumerable and IEnumerator (respective ConditionalEnumerable and
    /// ConditionalEnumerator) to accomplish the 'magic' of Linq filtering.
    /// </summary>
    sealed class CombinatorialEnumerable : IEnumerable
    {
        IEnumerable e;
        ActionWithReturn p;

        internal CombinatorialEnumerable(IEnumerable e, ActionWithReturn p)
        {
            this.e = e;
            this.p = p;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CombinatorialEnumerator(e.GetEnumerator(), p);
        }
    }
}
