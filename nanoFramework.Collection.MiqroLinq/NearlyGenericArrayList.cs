using System;
using System.Collections;
using System.Reflection;

namespace System.Collections.MiqroLinq
{
    public class NearlyGenericArrayList
    {
        private ArrayList InternalList;

        private void Init(Type type, int capacity)
        {
            InternalList = new ArrayList();

            CollectionType = type;

            if (capacity > 0)
                InternalList.Capacity = capacity;
        }

        /// <summary>
        /// Constructs an ArrayList and adds type checking for the specified type.
        /// Only objects that are of that type or inherit from that type can be
        /// added to the collection. The type is also checked to see if IComparable
        /// is implemented to provide sorting capabilities.
        /// </summary>
        /// <param name="type">The base type for allowed objects.</param>
        public NearlyGenericArrayList(Type type)
        {
            Init(type, 4);
        }


        /// <summary>
        /// Constructs an ArrayList and adds type checking for the specified type.
        /// Only objects that are of that type or inherit from that type can be
        /// added to the collection. The type is also checked to see if IComparable
        /// is implemented to provide sorting capabilities. An initial size can
        /// be specified to reduce the number of dynamic memory resizes.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="capacity"></param>
        public NearlyGenericArrayList(Type type, int capacity)
        {
            Init(type, capacity);
        }


        /// <summary>
        /// Checks to see if the object that's being added is an instance of
        /// the CollectionType specified when the collection was created. If
        /// so, the object is added, otherwise an exception is raised.
        /// </summary>
        /// <param name="o">The object to be added.</param>
        /// <returns>The index position at which the object was added (from base ArrayList.Add).</returns>
        /// <exception cref="ArgumentException">Thrown if the object is not of the correct type.</exception>
        public int Add(object o)
        {
            if (CollectionType.IsInstanceOfType(o))
                return InternalList.Add(o);
            
            throw new ArgumentException();
        }

        public void Insert(int pos, object o)
        {
            if (CollectionType.IsInstanceOfType(o))
                InternalList.Insert(pos, o);

            throw new ArgumentException();
        }

        public void Remove(object o)
        {
            InternalList.Remove(o);
        }

        public void Clear()
        {
            InternalList.Clear();
        }

        public void RemoveAt(int pos)
        {
            InternalList.RemoveAt(pos);
        }

        public IEnumerator GetEnumerator()
        {
            return InternalList.GetEnumerator();
        }

        public bool Contains(object o)
        {
            if (!CollectionType.IsInstanceOfType(o))
                return false;

            return InternalList.Contains(o);
        }

        public int Capacity
        {
            get
            {
                return InternalList.Capacity;
            }
            set
            {
                InternalList.Capacity = value;
            }
        }

        public int Count
        {
            get
            {
                return InternalList.Count;
            }
        }

        public Type CollectionType { get; private set; }


        public static implicit operator ArrayList(NearlyGenericArrayList ngal)
        {
            return (ArrayList)ngal.InternalList.Clone();
        }

        public static implicit operator NearlyGenericArrayList(object[] al)
        {
            NearlyGenericArrayList ngal = new NearlyGenericArrayList(al.GetType(), al.Length);
            for (int i = 0; i < al.Length; i++)
            {
                ngal.InternalList.Add(al[i]);
            }
            return ngal;
        }
    }
}
