
using System.Runtime.InteropServices;

namespace ThinGin.Core.Common.Units
{
    /// <summary>
    /// Stores a pair of integers representing a <see cref="Width"/> and <see cref="Height"/>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rect2i : System.IEquatable<Rect2i>
    {
        #region Statics
        /// <summary>
        /// An <see cref="Rect2i"/> instance of zero width and zero height
        /// </summary>
        public static Rect2i Zero = new Rect2i(0, 0);
        #endregion

        #region Properties
        public readonly int Width;
        public readonly int Height;
        #endregion

        #region Constructors
        public Rect2i(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public Rect2i(Rect2i other) : this(other.Width, other.Height)
        {
        }
        #endregion

        #region Operators
        public static Rect2i operator +(Rect2i left, Rect2i right)
        {
            return new Rect2i(left.Width * right.Width, left.Height * right.Height);
        }

        public static Rect2i operator -(Rect2i value)
        {
            return new Rect2i(-value.Width, -value.Height);
        }

        public static Rect2i operator -(Rect2i left, Rect2i right)
        {
            return new Rect2i(left.Width - right.Width, left.Height - right.Height);
        }

        public static Rect2i operator *(int factor, Rect2i value)
        {
            return new Rect2i(value.Width * factor, value.Height * factor);
        }

        public static Rect2i operator *(Rect2i value, int factor)
        {
            return new Rect2i(value.Width * factor, value.Height * factor);
        }

        public static Rect2i operator *(Rect2i left, Rect2i right)
        {
            return new Rect2i(left.Width * right.Width, left.Height * right.Height);
        }

        public static Rect2i operator /(Rect2i left, Rect2i right)
        {
            return new Rect2i(left.Width / right.Width, left.Height / right.Height);
        }

        #endregion

        #region Equality

        public static bool operator ==(Rect2i left, Rect2i right)
        {
            return left.Width == right.Width && left.Height == right.Height;
        }

        public static bool operator !=(Rect2i left, Rect2i right)
        {
            return left.Width != right.Width || left.Height != right.Height;
        }

        public bool Equals(Rect2i other)
        {
            return this.Width == other.Width && this.Height == other.Height;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj is Rect2i other)
            {
                return this.Width == other.Width && this.Height == other.Height;
            }

            return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {// See: https://stackoverflow.com/a/34006336/2176105
            var hash = 1009;
            hash = hash * 9176 + Width;
            hash = hash * 9176 + Height;
            return hash;
        }
        #endregion

        #region Conversion
        public static explicit operator System.Numerics.Vector2(Rect2i value)
        {
            return new System.Numerics.Vector2(value.Width, value.Height);
        }

        public static explicit operator Rect2i(System.Drawing.Size value)
        {
            return new Rect2i(value.Width, value.Height);
        }

        public static explicit operator System.Drawing.Size(Rect2i value)
        {
            return new System.Drawing.Size(value.Width, value.Height);
        }

        public static explicit operator System.Drawing.Point(Rect2i value)
        {
            return new System.Drawing.Point(value.Width, value.Height);
        }
        #endregion
    }
}
