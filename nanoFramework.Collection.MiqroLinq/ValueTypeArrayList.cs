using System;
using System.Collections;
using System.Diagnostics;

namespace System.Collections.MiqroLinq
{
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class ValueTypeArrayList : IList, ICollection, IEnumerable, ICloneable
    {
        internal class ValueTypeArrayListEnumerator : IEnumerator
        {
            ValueTypeArrayList wrappedarray;
            int lastPosition;
            int currentPosition;
            
            internal ValueTypeArrayListEnumerator(ValueTypeArrayList array)
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

        private ValueType[] items;
        private int itemCount = 0;


        public ValueTypeArrayList(int initialCapacity)
            : this(initialCapacity, null)
        { }

        public ValueTypeArrayList(ValueType[] initialValues)
            :this(initialValues.Length, initialValues)
        { }

        public ValueTypeArrayList(int initialCapacity, ValueType[] initialValues)
        {
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException("initialCapacity");

            items = new ValueType[initialCapacity];

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
                    items[index] = value as ValueType;

                throw new ArgumentOutOfRangeException();
            }
        }

        public void CopyTo(Array array, int index)
        {
            if (index > Count)
                throw new ArgumentOutOfRangeException();

            Array.Copy(items, 0, array, index, itemCount);
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
            ValueType[] result = new ValueType[itemCount];
            Array.Copy(items, 0, result, 0, itemCount);
            return result;
        }

        private int InsertItemAtPosition(int index, object value)
        {
            if (index >= 0 && index <= itemCount)
            {
                if (itemCount >= items.Length)
                {
                    ValueType[] temp = new ValueType[items.Length * 2];
                    items.CopyTo(temp, 0);
                    items = temp;
                }

                if (index == itemCount)
                {
                    items[itemCount++] = (ValueType)value;
                }
                else
                {
                    ++itemCount;
                    for (int i = itemCount - 1; i > index; i--)
                    {
                        items[i] = items[i - 1];
                    }

                    items[index] = (ValueType)value;
                }
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
            }
        }


        public static implicit operator ValueType[](ValueTypeArrayList vtal)
        {
            return (ValueType[])vtal.Clone();
        }

        public static implicit operator ValueTypeArrayList(ValueType[] vals)
        {
            return new ValueTypeArrayList(vals.Length, vals);
        }
    }
}
