using System;
using System.Collections;

namespace System.Collections.MiqroLinq
{
    public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    public enum NotifyCollectionChangedAction
    {
        Add,
        Remove,
        Reset
    }

    public class NotifyCollectionChangedEventArgs
    {
        NotifyCollectionChangedAction action;

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction ChangedAction)
        {
            action = ChangedAction;
        }

        public NotifyCollectionChangedAction Action
        {
            get { return action; }
        }
    }

    public class NotifyingCollection : INotifyCollectionChanged, IList, ICollection, IEnumerable, ICloneable
    {
        internal class ValueTypeArrayListEnumerator : IEnumerator
        {
            NotifyingCollection wrappedarray;
            int lastPosition;
            int currentPosition;

            internal ValueTypeArrayListEnumerator(NotifyingCollection array)
            {
                wrappedarray = array;
                lastPosition = array.Count;
                currentPosition = -1;
            }

            public object Current
            {
                get
                {
                    return wrappedarray[currentPosition % wrappedarray.items.Length];
                }
            }

            public bool MoveNext()
            {
                if (currentPosition < lastPosition)
                {
                    return ++currentPosition < lastPosition;
                }

                return false;
            }

            public void Reset()
            {
                currentPosition = -1;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private Object[] items;
        private int itemCount = 0;

        public NotifyingCollection(int initialCapacity)
            :this(initialCapacity, null)
        { }

        public NotifyingCollection(Object[] initialValues)
            :this(initialValues.Length, initialValues)
        { }

        public NotifyingCollection(int initialCapacity, Object[] initialValues)
        {
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException("initialCapacity");

            items = new Object[initialCapacity];

            if (initialValues != null && initialValues.Length > 0)
            {
                Array.Copy(initialValues, items, items.Length);
                itemCount = items.Length;
            }
        }

        public int Add(object value)
        {
            return InsertItemAtPosition(Count, value);
        }

        public void Clear()
        {
            Array.Clear(items, 0, items.Length);

            NotifyChange(NotifyCollectionChangedAction.Reset);
        }

        public bool Contains(object value)
        {
            return IndexOf(value) >= 0;
        }

        public int IndexOf(object value)
        {
            return Array.IndexOf(items, value, 0, itemCount);
        }

        public void Insert(int index, object value)
        {
            InsertItemAtPosition(index, value);
        }

        public bool IsFixedSize
        {
            get { return items.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return items.IsReadOnly; }
        }

        public void Remove(object value)
        {
            int foundAt = IndexOf(value);

            if (foundAt >= 0)
            {
                RemoveAt(foundAt);
            }
        }

        public object this[int index]
        {
            get
            {
                if (index >= 0 && index < itemCount)
                    return items.GetValue(index);

                throw new ArgumentOutOfRangeException();
            }
            set
            {
                if (index >= 0 && index < itemCount)
                    items[index] = value as Object;

                throw new ArgumentOutOfRangeException();
            }
        }

        public void CopyTo(Array array, int index)
        {
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException();

            Array.Copy(items, 0, array, index, Math.Min(itemCount, array.Length));
        }

        public int Count
        {
            get { return itemCount; }
        }

        public bool IsSynchronized
        {
            get { return items.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return items.SyncRoot; }
        }

        public IEnumerator GetEnumerator()
        {
            return new ValueTypeArrayListEnumerator(this);
        }

        public object Clone()
        {
            Object[] result = new Object[itemCount];
            Array.Copy(items, 0, result, 0, itemCount);
            return result;
        }


        private int InsertItemAtPosition(int index, object value)
        {
            if (index >= 0 && index <= itemCount)
            {
                if (itemCount >= items.Length)
                {
                    Object[] temp = new Object[items.Length * 2];
                    items.CopyTo(temp, 0);
                    items = temp;
                }

                if (index == itemCount)
                {
                    items[itemCount++] = value;
                }
                else
                {
                    ++itemCount;
                    for (int i = itemCount - 1; i > index; i--)
                    {
                        items[i] = items[i - 1];
                    }

                    items[index] = value;
                }

                NotifyChange(NotifyCollectionChangedAction.Add);
            }

            return index;
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < itemCount)
            {
                for (int i = index; i < itemCount - 1; i++)
                {
                    items[i] = items[i + 1];
                }

                items[itemCount--] = null;

                NotifyChange(NotifyCollectionChangedAction.Remove);
            }
        }

        private void NotifyChange(NotifyCollectionChangedAction action)
        {
            if (null != CollectionChanged)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }


        public static implicit operator Object[](NotifyingCollection nc)
        {
            return (Object[])nc.Clone();
        }

        public static implicit operator NotifyingCollection(Object[] vals)
        {
            return new NotifyingCollection(vals);
        }
    }
}
