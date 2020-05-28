using System;
using System.Collections;

namespace System.Collections.MiqroLinq
{
    /// <summary>
    /// Because the yield statement is broken for current C# and NETMF 4.1 we have to
    /// use a custom IEnumerable and IEnumerator (respective ConditionalEnumerable and
    /// ConditionalEnumerator) to accomplish the 'magic' of Linq filtering.
    /// </summary>
    sealed class ConditionalEnumerator : IEnumerator, IDisposable
    {
        IEnumerator e;
        Predicate p;

        internal ConditionalEnumerator(IEnumerator e, Predicate p)
        {
            this.e = e;
            this.p = p;
        }

        object IEnumerator.Current
        {
            get { return e.Current; }
        }

        void IEnumerator.Reset()
        {
            e.Reset();
        }

        bool IEnumerator.MoveNext()
        {
            var b = e.MoveNext();
            while (b && !p(e.Current))
            {
                b = e.MoveNext();
            }
            return b;
        }

        public void Dispose()
        {
            var d = e as IDisposable;
            if (null != d)
                d.Dispose();
        }
    }
}
