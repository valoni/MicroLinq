using System.Collections;

namespace System.Collections.MiqroLinq
{
    /// <summary>
    /// Because the yield statement is broken for current C# and NETMF 4.1 we have to
    /// use a custom IEnumerable and IEnumerator (respective ConditionalEnumerable and
    /// ConditionalEnumerator) to accomplish the 'magic' of Linq filtering.
    /// </summary>
    sealed class ConditionalEnumerable : IEnumerable
    {
        IEnumerable e;
        Predicate p;

        internal ConditionalEnumerable(IEnumerable e, Predicate p)
        {
            this.e = e;
            this.p = p;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ConditionalEnumerator(e.GetEnumerator(), p);
        }
    }
}
