using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ThinGin.Core.Common.IDSystem
{
    internal class id_interval : IComparable<id_interval>
    {
        #region Values
        public readonly int lower;
        public readonly int upper;
        #endregion

        #region Constructors
        public id_interval(int lower, int upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int id) => lower <= id && upper >= id;

        #region Comparison
        public int CompareTo([AllowNull] id_interval other)
        {
            // First check if the interval lies within us
            if (lower <= other.lower && upper >= other.upper)
                return 0;

            // Non-overlapping
            if (upper <= other.lower)
                return -1;

            if (lower >= other.upper)
                return 1;

            // Partial overlapping
            if (lower < other.lower)
                return -1;

            if (lower > other.lower)
                return 1;

            // I guess at this point they are equal?
            return 0;
        }
        #endregion

        #region Equals
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is id_interval other)
            {
                return lower == other.lower && upper == other.upper;
            }

            return false;
        }
        #endregion

        #region Hash
        public override int GetHashCode()
        {
            return HashCode.Combine(lower, upper);
        }
        #endregion

        #region Operators
        public static bool operator <(id_interval left, id_interval right)
        {
            return left.lower < right.lower;
        }

        public static bool operator >(id_interval left, id_interval right)
        {
            return left.lower > right.lower;
        }
        #endregion
    }
}
