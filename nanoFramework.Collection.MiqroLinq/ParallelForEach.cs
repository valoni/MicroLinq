using System;
using System.Collections;
using System.Threading;

namespace System.Collections.MiqroLinq.ParallelExtensions
{
    public static partial class ParallelExtensions
    {
        public static void ParallelForEach(this IEnumerable e, Action a, int millsecondsTimeout = -1)
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            Thread t = null;
            int count = 0;
            int total = 0;
            int target = 0;

            foreach (object o in e)
            {
                ++count;
                object captured = o;

                if (null != t)
                    t.Start();

                t = new Thread(() =>
                {
                    try
                    {
                        a(captured);
                    }
                    finally
                    {
                        if (Interlocked.Increment(ref total) == target)
                            mre.Set();
                    }
                });
            }

            if (null != t)
            {
                target = count;
                t.Start();
                mre.WaitOne(millsecondsTimeout, false);
            }
        }
    }
}
