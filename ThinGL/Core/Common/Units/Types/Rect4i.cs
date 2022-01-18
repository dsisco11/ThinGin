
using System.Runtime.InteropServices;

namespace ThinGin.Core.Common.Units
{
    /// <summary>
    /// Stores a pair of integers representing a <see cref="Top"/>, <see cref="Right"/>, <see cref="Bottom"/>, <see cref="Left"/>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rect4i : System.IEquatable<Rect4i>
    {
        #region Static
        public static Rect4i Zero = new Rect4i(0, 0, 0, 0);
        public static Rect4i One = new Rect4i(0, 1, 1, 0);
        #endregion


        #region Properties
        public int Top;
        public int Right;
        public int Bottom;
        public int Left;

        public int X => this.Left;
        public int Y => this.Top;
        public int Width => this.Right - this.Left;
        public int Height => this.Bottom - this.Top;

        public Rect2i Size()
        {
            return new Rect2i(this.Width, this.Height);
        }

        public Vector2i Location()
        {
            return new Vector2i(this.X, this.Y);
        }
        #endregion

        #region Constructors
        public Rect4i(int Top, int Right, int Bottom, int Left)
        {
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
            this.Left = Left;
        }

        public Rect4i(Rect4i other) : this(other.Top, other.Right, other.Bottom, other.Left)
        {
        }

        public Rect4i(Vector2i origin, Rect2i size) :
            this(origin.Y,
                origin.X + size.Width,
                origin.Y + size.Height,
                origin.X)
        {
        }
        #endregion

        #region Operators
        public static Rect4i operator +(Rect4i left, Rect4i right)
        {
            return new Rect4i();
        }

        public static Rect4i operator -(Rect4i left, Rect4i right)
        {
            return new Rect4i();
        }

        public static Rect4i operator *(Rect4i left, Rect4i right)
        {
            return new Rect4i();
        }

        public static Rect4i operator /(Rect4i left, Rect4i right)
        {
            return new Rect4i();
        }
        #endregion

        #region Equality

        public static bool operator ==(Rect4i left, Rect4i right)
        {
            return left.Top == right.Top &&
                left.Right == right.Right &&
                left.Bottom == right.Bottom &&
                left.Left == right.Left;
        }

        public static bool operator !=(Rect4i left, Rect4i right)
        {
            return left.Top != right.Top ||
                left.Right != right.Right ||
                left.Bottom != right.Bottom ||
                left.Left != right.Left;
        }

        public bool Equals(Rect4i other)
        {
            return this.Top == other.Top &&
                this.Right == other.Right &&
                this.Bottom == other.Bottom &&
                this.Left == other.Left;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj is Rect4i other)
            {
                return this.Top == other.Top &&
                    this.Right == other.Right &&
                    this.Bottom == other.Bottom &&
                    this.Left == other.Left;
            }

            return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {// See: https://stackoverflow.com/a/34006336/2176105
            var hash = 1009;
            hash = hash * 9176 + Top;
            hash = hash * 9176 + Right;
            hash = hash * 9176 + Bottom;
            hash = hash * 9176 + Left;
            return hash;
        }
        #endregion

        #region Conversion
        public static explicit operator System.Numerics.Vector2(Rect4i value)
        {
            return new System.Numerics.Vector2(value.Left, value.Top);
        }

        public static explicit operator System.Drawing.Size(Rect4i value)
        {
            return new System.Drawing.Size(value.Width, value.Height);
        }

        public static explicit operator System.Drawing.Point(Rect4i value)
        {
            return new System.Drawing.Point(value.Left, value.Top);
        }
        #endregion

        #region Other
        public bool Contains(int X, int Y)
        {
            return Left < X &&
                Right > X &&
                Top < Y &&
                Bottom > Y;
        }

        public bool Contains(Vector2i value)
        {
            return Left < value.X &&
                Right > value.X &&
                Top < value.Y &&
                Bottom > value.Y;
        }

        public bool Intersects(Rect4i other)
        {
            // Proof by contradiction
            // if any of the minimum(top,left) edges are beyond the others maximum(bottom,right) edges then no intersection can be occuring
            // additionally; if any of the maximum(bottom,right) edges are less than the others minimum(top,left) edges then no intersection can be occuring
            return Left < other.Right &&
                Right > other.Left &&
                Top < other.Bottom &&
                Bottom > other.Top;
        }


        public static bool Contains(int X, int Y, int Top, int Right, int Bottom, int Left)
        {
            return Left < X &&
                Right > X &&
                Top < Y &&
                Bottom > Y;
        }

        public static bool Contains(float X, float Y, float Top, float Right, float Bottom, float Left)
        {
            return Left < X &&
                Right > X &&
                Top < Y &&
                Bottom > Y;
        }

        public static bool Contains(double X, double Y, double Top, double Right, double Bottom, double Left)
        {
            return Left < X &&
                Right > X &&
                Top < Y &&
                Bottom > Y;
        }
        #endregion
    }
}
