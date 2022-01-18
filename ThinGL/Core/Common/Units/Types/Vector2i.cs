using System.Runtime.InteropServices;

namespace ThinGin.Core.Common.Units
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vector2i : System.IEquatable<Vector2i>
    {
        #region Statics
        public static Vector2i Zero = new Vector2i(0, 0);
        #endregion

        #region Properties
        public readonly int X;
        public readonly int Y;
        #endregion

        #region Constructors
        public Vector2i(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vector2i(Vector2i other) : this(other.X, other.Y)
        {
        }
        #endregion

        #region Operators
        /// <summary>
        /// Adds all of the elements of both vectors
        /// </summary>
        public static Vector2i operator +(Vector2i left, Vector2i right)
        {
            return new Vector2i(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// Returns a new vector whose values are the negated values from the original vector
        /// </summary>
        public static Vector2i operator -(Vector2i value)
        {
            return new Vector2i(-value.X, -value.Y);
        }

        /// <summary>
        /// Subtracts all of the elements of both vectors
        /// </summary>
        public static Vector2i operator -(Vector2i left, Vector2i right)
        {
            return new Vector2i(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2i operator *(int factor, Vector2i value)
        {
            return new Vector2i(value.X * factor, value.Y * factor);
        }

        public static Vector2i operator *(Vector2i value, int factor)
        {
            return new Vector2i(value.X * factor, value.Y * factor);
        }

        public static Vector2i operator *(Vector2i left, Vector2i right)
        {
            return new Vector2i(left.X * right.X, left.Y * right.Y);
        }

        public static Vector2i operator /(Vector2i left, Vector2i right)
        {
            return new Vector2i(left.X / right.X, left.Y / right.Y);
        }
        #endregion

        #region Equality

        public static bool operator ==(Vector2i left, Vector2i right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Vector2i left, Vector2i right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        public bool Equals(Vector2i other)
        {
            //if (ReferenceEquals(this, other))// On average this will waste a computation
            //    return true;

            return this.X == other.X && this.Y == other.Y;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj is Vector2i other)
            {
                return this.X == other.X && this.Y == other.Y;
            }

            return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {// See: https://stackoverflow.com/a/34006336/2176105
            var hash = 1009;
            hash = hash * 9176 + X;
            hash = hash * 9176 + Y;
            return hash;
        }
        #endregion

        #region Conversion
        public static explicit operator System.Numerics.Vector2(Vector2i value)
        {
            return new System.Numerics.Vector2(value.X, value.Y);
        }

        public static explicit operator Vector2i(System.Drawing.Point value)
        {
            return new Vector2i(value.X, value.Y);
        }

        public static explicit operator System.Drawing.Size(Vector2i value)
        {
            return new System.Drawing.Size(value.X, value.Y);
        }

        public static explicit operator System.Drawing.Point(Vector2i value)
        {
            return new System.Drawing.Point(value.X, value.Y);
        }
        #endregion
    }
}
