using System;
using System.Collections;
using System.Reflection;

namespace System.Collections.MiqroLinq
{
    public static class MicroLinqExtensions
    {
        /// <summary>
        /// Returns an object that represents the aggregate of the items in an IEnumerable defined
        /// by the delegate supplied. An optional seed parameter is used as the aggregate collector.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate.</param>
        /// <param name="seed">Optional. The object to seed the delegate.</param>
        /// <param name="a">The delegate which is applied to each element.</param>
        /// <returns>An object that is the result of the aggregate being applied to each element.</returns>
        public static object Aggregate(this IEnumerable e, object seed, Aggregate a)
        {
            foreach (var o in e)
            {
                seed = a(seed, o);
            }

            return seed;
        }


        /// <summary>
        /// Returns an object that represents the aggregate of the items in an IEnumerable defined
        /// by the delegate supplied. An optional seed parameter is used as the aggregate collector.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate.</param>
        /// <param name="a">The delegate which is applied to each element.</param>
        /// <returns>An object that is the result of the aggregate being applied to each element.</returns>
        public static object Aggregate(this IEnumerable e, Aggregate a)
        {
            return Aggregate(e, new object(), a);
        }


        /// <summary>
        /// Returns an object that represents the aggregate of the items in an IEnumerable defined
        /// by the delegate supplied. An optional seed parameter is used as the aggregate collector.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate.</param>
        /// <param name="a">The delegate which is applied to each element.</param>
        /// <param name="s">Optional. A selector delegate used to select the result from the seed.</param>
        /// <returns>An object that is the result of the aggregate being applied to each element.</returns>
        public static object Aggregate(this IEnumerable e, Aggregate a, Selector s)
        {
            return s(Aggregate(e, a));
        }


        /// <summary>
        /// Returns an object that represents the aggregate of the items in an IEnumerable defined
        /// by the delegate supplied. An optional seed parameter is used as the aggregate collector.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate.</param>
        /// <param name="seed">Optional. The object to seed the delegate.</param>
        /// <param name="a">The aggregate delegate which is applied to each element.</param>
        /// <param name="s">Optional. A selector delegate used to select the result from the seed.</param>
        /// <returns>An object that is the result of the aggregate being applied to each element.</returns>
        public static object Aggregate(this IEnumerable e, object seed, Aggregate a, Selector s)
        {
            return s(Aggregate(e, seed, a));
        }


        /// <summary>
        /// Returns true if all objects in the IEnumerable satisfy the predicate condition.
        /// </summary>
        /// <param name="e">The IEnumerable object to be scanned.</param>
        /// <param name="p">The predicate which will be applied to each element unless one fails.</param>
        /// <returns>True if all objects satisfy the predicate condition, otherwise false.</returns>
        public static bool All(this IEnumerable e, Predicate p)
        {
            foreach (var o in e)
                if (!p(o))
                    return false;

            return true;
        }


        /// <summary>
        /// Returns true if any object in the IEnumerable satisfies the predicate condition.
        /// </summary>
        /// <param name="e">The IEnumerable object to be scanned.</param>
        /// <param name="p">The predicate which will be applied to each element until one passes.</param>
        /// <returns>True if any object satisfies the predicate condition, otherwise false.</returns>
        public static bool Any(this IEnumerable e, Predicate p)
        {
            foreach (var o in e)
                if (p(o))
                    return true;

            return false;
        }


        /// <summary>
        /// Iterates the IEnumerable to see if it contains an object equal to the object being checked.
        /// </summary>
        /// <param name="e">The IEnumerable object to be scanned.</param>
        /// <param name="ob">The object to compare for equality.</param>
        /// <returns>True if the IEnumerable contains an equivalent object, otherwise false.</returns>
        public static bool Contains(this IEnumerable e, object ob)
        {
            foreach (var o in e)
                if (o.Equals(ob))
                    return true;

            return false;
        }


        /// <summary>
        /// Returns a count of the number of elements in the IEnumerable.
        /// </summary>
        /// <param name="e">The IEnumerable object of which you want to know the count.</param>
        /// <returns>The number of items in the IEnumerable.</returns>
        public static int Count(this IEnumerable e)
        {
            ICollection list = e as ICollection;
            if (list != null)
                return list.Count;

            return Count(e, o => true);
        }


        /// <summary>
        /// Returns a count of the number of elements in a IEnumerable which satisfy a condition.
        /// </summary>
        /// <param name="e">The IEnumerable object of which you want to know the count.</param>
        /// <param name="p">The predicate which will be applied to each element before it is counted.</param>
        /// <returns>The number of items in the IEnumerable which pass the predicate condition.</returns>
        public static int Count(this IEnumerable e, Predicate p)
        {
            int total = 0;

            foreach (var o in e)
            {
                if (p(o))
                    total++;
            }

            return total;
        }


        /// <summary>
        /// Returns the first object in an IEnumerable or null if the collection is empty.
        /// </summary>
        /// <param name="e">The IEnumerable object from which to get the first object.</param>
        /// <returns>The first object in an IEnumerable or null if the IEnumerable is empty.</returns>
        public static object FirstOrDefault(this IEnumerable e)
        {
            return FirstOrDefault(e, o => true);
        }


        /// <summary>
        /// Returns the first object in an IEnumerable which satisfies a condition or null.
        /// </summary>
        /// <param name="e">The IEnumerable object to scan.</param>
        /// <param name="p">The predicate which will be applied to each element until one passes.</param>
        /// <returns>The first object which satisfies the predicate condition.</returns>
        public static object FirstOrDefault(this IEnumerable e, Predicate p)
        {
            foreach (var o in e)
                if (p(o))
                    return o;

            return null;
        }


        /// <summary>
        /// Returns a wrapper around an IEnumerable to return the combined results of the elements and the supplied delegate.
        /// </summary>
        /// <param name="e">The IEnumerable object over which you want to Select.</param>
        /// <param name="a">The delegate to be applied to each element.</param>
        /// <returns>An IEnunumerable which will return objects after the delegate is applied.</returns>
        public static IEnumerable Select(this IEnumerable e, ActionWithReturn a)
        {
            return new CombinatorialEnumerable(e, a);
        }


        /// <summary>
        /// Convenience extension to wrap Aggregate for numeric operations with an initial seed value of 0.
        /// This overload should only be used with int types.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate. Should be a ValueType array or collection.</param>
        /// <returns>ValueType with the sum value.</returns>
        public static ValueType Sum(this IEnumerable e)
        {
            return Aggregate(e, 0, (i1, i2) => (int)i1 + (int)i2) as ValueType;
        }


        /// <summary>
        /// Convenience extension to wrap Aggregate for numeric operations with an initial seed value of 0.
        /// This overload should only be used with int types.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate. Should be a ValueType array or collection.</param>
        /// <param name="s">Optional. A selector delegate used to select the value from an object.</param>
        /// <returns>ValueType with the sum value.</returns>
        public static ValueType Sum(this IEnumerable e, Selector s)
        {
            return Aggregate(e, 0, (o1, o2) => (int)s(o1) + (int)s(o2)) as ValueType;
        }


        /// <summary>
        /// Convenience extension to wrap Aggregate for numeric operations with an initial seed value of 0.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate. Should be a ValueType array or collection.</param>
        /// <param name="seed">The initial value to start the sum. This must be the same type as used in the aggregate delegate.</param>
        /// <param name="a">The aggregate delegate used to perform the sum operation on the elements.</param>
        /// <returns></returns>
        public static ValueType Sum(this IEnumerable e, ValueType seed, Aggregate a)
        {
            return Aggregate(e, seed, a) as ValueType;
        }

        /// <summary>
        /// Convenience extension to wrap Aggregate for numeric operations with an initial seed value of 0.
        /// </summary>
        /// <param name="e">The IEnumerable to iterate. Should be a ValueType array or collection.</param>
        /// <param name="seed">The initial value to start the sum. This must be the same type as used in the aggregate delegate.</param>
        /// <param name="a">The aggregate delegate used to perform the sum operation on the elements.</param>
        /// <param name="s">Optional. A selector delegate used to select the value from an object.</param>
        /// <returns>ValueType with the sum value.</returns>
        public static ValueType Sum(this IEnumerable e, ValueType seed, Aggregate a, Selector s)
        {
            return Aggregate(e, seed, a, s) as ValueType;
        }

        /// <summary>
        /// Returns a wrapper around an IEnumerable to return elements matching the requested type.
        /// </summary>
        /// <param name="e">The IEnumerable object of which you want to get objects of a specified type.</param>
        /// <param name="t">The type of objects you want returned.</param>
        /// <returns>An IEnumerable which will return only objects castable to the requested type.</returns>
        public static IEnumerable OfType(this IEnumerable e, Type t)
        {
            return new ConditionalEnumerable(e, o => t.IsInstanceOfType(o));
        }


        /// <summary>
        /// Copies the elements from an IEnumerable into an Array of objects. A simple quick sort
        /// is applied to the new list by applying the Selector delegate to each object element and
        /// casting it to IComparable (or a reflected invoke of CompareTo) to perform the comparison.
        /// If a comparison delegate is provided it will be used to perform the comparison.
        /// </summary>
        /// <param name="e">The IEnumerable to copy and process.</param>
        /// <param name="s">The delegate used to select which property in an object should be compared.</param>
        /// <returns>A new IEnumerable with elements ordered by the property specified in the selector delegate.</returns>
        [Obsolete("For performance reasons, consider passing an explicit Comparer delegate.")]
        public static IEnumerable OrderBy(this IEnumerable e, Selector s)
        {
            return OrderBy(e, s, Compare);
        }


        /// <summary>
        /// Copies the elements from an IEnumerable into an Array of objects. A simple quick sort
        /// is applied to the new list by applying the Selector delegate to each object element and
        /// casting it to IComparable (or a reflected invoke of CompareTo) to perform the comparison.
        /// If a comparison delegate is provided it will be used to perform the comparison.
        /// </summary>
        /// <param name="e">The IEnumerable to copy and process.</param>
        /// <param name="s">The delegate used to select which property in an object should be compared.</param>
        /// <param name="c">The delegate used to perform comparisons on objects during the order.</param>
        /// <returns>A new IEnumerable with elements ordered by the property specified in the selector delegate.</returns>
        public static IEnumerable OrderBy(this IEnumerable e, Selector s, Comparer c)
        {
            ICollection d = e as ICollection;
            object[] data;

            if (null == d)
            {
                var temp = new ArrayList();
                foreach (var o in e)
                    temp.Add(o);
                data = temp.ToArray();

                if (data.Length <= 1)
                    return data;
            }
            else
            {
                if (d.Count <= 1)
                    return d;

                data = new object[d.Count];
                d.CopyTo((object[])data, 0);
            }

            return MergeSort(data, s, c);
        }

        #region OrderBy Workers

        private static IEnumerable MergeSort(object[] data, Selector s, Comparer c)
        {
            object[] left = SplitArray(data, 0, data.Length / 2 - 1);
            object[] right = SplitArray(data, data.Length / 2, data.Length - 1);

            if (left.Length > 1) MergeSort(left, s, c);
            if (right.Length > 1) MergeSort(right, s, c);

            Merge(left, right, data, s, c);

            return data;
        }

        private static void Merge(object[] left, object[] right, object[] result, Selector s, Comparer c)
        {
            int i = 0, j = 0, h = 0;

            while (i < left.Length || j < right.Length)
            {
                if (i == left.Length) result[h++] = right[j++];
                else if (j == right.Length) result[h++] = left[i++];
                else if (c(s(left[i]), s(right[j])) < 0)
                {
                    result[h++] = left[i++];
                }
                else
                {
                    result[h++] = right[j++];
                }
            }
        }

        private static object[] SplitArray(object[] array, int start, int end)
        {
            object[] result = new object[end - start + 1];
            Array.Copy(array, start, result, 0, result.Length);
            return result;
        }

        private static int Compare(object a, object b)
        {
            IComparable ca = a as IComparable;
            if (null != ca)
                return ca.CompareTo(b);

            MethodInfo mi = a.GetType().GetMethod("CompareTo", new Type[] { typeof(object) });
            if (null != mi)
                return (int)mi.Invoke(a, new[] { b });

            if (object.Equals(a, b))
                return 0;

            if (a is ValueType && b is ValueType)
            {
                if ((a is long || a is int || a is short || a is sbyte || a is bool || a is char) &&
                    (b is long || b is int || b is short || b is sbyte || b is bool || b is char))
                    return (long)a < (long)b ? -1 : 1;

                if ((a is double || a is float) &&
                    (b is double || b is float))
                    return (double)a < (double)b ? -1 : 1;

                if ((a is ulong || a is uint || a is ushort || a is byte) &&
                    (b is ulong || b is uint || b is ushort || b is byte))
                    return (ulong)a < (ulong)b ? -1 : 1;
            }

            throw new NotSupportedException("This type is not supported for automatic sorting. Please specify a Comparer delegate.");
        }

        #endregion

        /// <summary>
        /// Returns a wrapper around an IEnumerable to return elements matching the supplied condition.
        /// </summary>
        /// <param name="e">The IEnumerable object from which you want to get items matching a condition.</param>
        /// <param name="p">The predicate which will be applied to each element.</param>
        /// <returns>An IEnumerable which will return only objects which pass the predicate condition.</returns>
        public static IEnumerable Where(this IEnumerable e, Predicate p)
        {
            return new ConditionalEnumerable(e, p);
        }

    }
}
