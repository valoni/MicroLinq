using System;
using System.Collections;

namespace System.Collections.MiqroLinq
{
    /// <summary>
    /// Because the yield statement is broken for current C# and NETMF 4.1 we have to
    /// use a custom IEnumerable and IEnumerator (respective ConditionalEnumerable and
    /// ConditionalEnumerator) to accomplish the 'magic' of Linq filtering.
    /// </summary>
    sealed class CombinatorialEnumerator : IEnumerator, IDisposable
    {
        IEnumerator e;
        ActionWithReturn p;

        internal CombinatorialEnumerator(IEnumerator e, ActionWithReturn p)
        {
            this.e = e;
            this.p = p;
        }

        object IEnumerator.Current
        {
            get { return p(e.Current); }
        }

        void IEnumerator.Reset()
        {
            e.Reset();
        }

        bool IEnumerator.MoveNext()
        {
            return e.MoveNext();
        }

        public void Dispose()
        {
            var d = e as IDisposable;
            if (null != d)
                d.Dispose();
        }
    }
}
