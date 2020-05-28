using System;
using System.Threading;

using System.Collections;
using System.Collections.MiqroLinq;



namespace nanoFramework.Collection.MicroLinq.Sample
{
    public class Program
    {
        public static void Main()
        {
            NotifyingCollection watchable = new NotifyingCollection(10);
            watchable.CollectionChanged += new  NotifyCollectionChangedEventHandler(watchable_CollectionChanged);

            watchable.Add(5);
            watchable.Add(8);
            watchable.Insert(0, 9);
            watchable.Remove(8);
            watchable.Add(11);

            Console.WriteLine("Current values:");
            foreach (object i in watchable)
            {
                Console.WriteLine(i.ToString());
            }

            Console.WriteLine(string.Empty);

            watchable.Clear();
            //And the event handler…
        
           Thread.Sleep(Timeout.Infinite);

       }

        static void watchable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("WATCHABLE UPDATED! Type: " + e.Action);
        }
    }
}
