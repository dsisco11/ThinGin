using System;

namespace ThinGin.Core.Common.Types
{
    /// <summary>
    /// Facilitates an N-th dimensional bounding box
    /// </summary>
    public class BoundingDimensions : IComparable<BoundingDimensions>, IEquatable<BoundingDimensions>
    {
        #region Structs
        public struct AxisMinMax 
        {
            #region Properties
            public readonly double Min;
            public readonly double Max;
            #endregion

            #region Constructors
            public AxisMinMax(double min, double max) : this()
            {
                Min = min;
                Max = max;
            }
            #endregion
        }
        #endregion

        #region Data
        private readonly AxisMinMax[] _axis;
        #endregion

        #region Properties
        // Tracks the min/max coordinate for each axis
        public AxisMinMax[] Axis => _axis;
        #endregion

        #region Accessors
        public int AxisCount => _axis.Length;
        #endregion

        #region Constructors
        public BoundingDimensions(int axisCount = 3)
        {
            _axis = new AxisMinMax[axisCount];

            for (int i = 0; i < axisCount; i++)
            {
                _axis[i] = new AxisMinMax(0d, 0d);
            }
        }
        #endregion

        #region Encompass
        /// <summary>
        ///  Expands the bounds such that they encompass the given axis coordinates
        /// </summary>
        /// <param name="Coordinate">Spatial coordinates</param>
        public void Encompass(double[] Coordinate)
        {
            var Dimensions = System.Math.Min(_axis.Length, Coordinate.Length);
            for (int i = 0; i < Dimensions; i++)
            {
                var coord = Coordinate[i];
                var ax = _axis[i];
                var mn = ax.Min;
                var mx = ax.Max;
                
                if (mn.CompareTo(coord) > 0) mn = coord;
                if (mx.CompareTo(coord) < 0) mx = coord;

                _axis[i] = new AxisMinMax(mn, mx);
            }
        }
        #endregion

        #region Size
        public void Set_Size(in AxisMinMax[] Bounds)
        {
            for (int i = 0; i < _axis.Length; i++)
            {
                _axis[i] = Bounds[i];
            }
        }

        /// <summary>
        /// Calculates the bounding structures size in each of its axiis
        /// </summary>
        /// <returns></returns>
        public double[] Get_Size()
        {
            double[] size = new double[_axis.Length];
            for (int i = 0; i<_axis.Length; i++)
            {
                size[i] = Math.Abs(_axis[i].Max - _axis[i].Min);
            }

            return size;
        }
        #endregion

        #region Volume
        /// <summary>
        /// Calculates the bounding structures total volume
        /// </summary>
        /// <returns></returns>
        public double Get_Volume()
        {
            var sizes = Get_Size();
            double volume = sizes[0];
            for (int i = 1; i < _axis.Length; i++)
            {
                volume *= sizes[i];
            }
            return volume;
        }
        #endregion

        #region Comparison
        /// <summary>
        ///     Compares the current instance with another object of the same type and returns
        ///     an integer that indicates whether the current instance precedes, follows, or
        ///     occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        ///     A value that indicates the relative volume of the objects being compared. The
        ///     return value has these meanings: Value Meaning Less than zero This instance has a lesser volume. 
        ///     Zero This instance has the same volume.
        ///     Greater than zero This instance has a greater volume
        ///     order.
        /// </returns>
        public int CompareTo(BoundingDimensions other)
        {
            if (other is null) return 1;

            var v1 = Get_Volume();
            var v2 = other.Get_Volume();

            var dv = v1 - v2;
            return Math.Sign(dv);
        }
        #endregion

        #region Equality
        /// <summary>
        /// Returns a value indicating whether the given bounding structure has the same axis values as this one.
        /// </summary>
        public bool Equals(BoundingDimensions other)
        {
            if (other is null) return false;
            if (other.Axis.Length != Axis.Length) return false;

            for (int i = 0; i < _axis.Length; i++)
            {
                var a1 = _axis[i];
                var a2 = other._axis[i];
                if (a1.Min != a2.Min || a1.Max != a2.Max)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(System.Drawing.Rectangle Rect)
        {
            if (_axis.Length != 2) return false;

            return _axis[0].Min == Rect.Left
                   && _axis[0].Max == Rect.Right
                   && _axis[1].Min == Rect.Top
                   && _axis[1].Max == Rect.Bottom;
        }
        #endregion

    }
}
