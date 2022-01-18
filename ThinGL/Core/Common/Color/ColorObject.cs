using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Types
{
    /// <summary>
    ///  Represents an abstraction of color data and provides the ability to perform complex actions on said data
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public abstract class ColorObject<T> : IColorObject
    {
        const float fbyteMax = byte.MaxValue;

        public abstract T From(uint n);
        public abstract T From(Vector4 vector);


        #region Scalars
        public abstract Vector4 GetVector();
        public abstract void SetVector(Vector4 RGBA);
        #endregion

        #region Conversion
        public abstract uint AsInteger();
        #endregion

        #region Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T operator *(ColorObject<T> left, IColorObject right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));
            Contract.EndContractBlock();

            return left.From(left.GetVector() * right.GetVector());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T operator /(ColorObject<T> left, IColorObject right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));
            Contract.EndContractBlock();

            return left.From(left.GetVector() / right.GetVector());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T operator +(ColorObject<T> left, IColorObject right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));
            Contract.EndContractBlock();

            return left.From(left.GetVector() + right.GetVector());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T operator -(ColorObject<T> left, IColorObject right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));
            Contract.EndContractBlock();

            return left.From(left.GetVector() - right.GetVector());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Multiply(ColorObject<T> left, IColorObject right)
        {
            return left * right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Divide(ColorObject<T> left, IColorObject right)
        {
            return left / right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Add(ColorObject<T> left, IColorObject right)
        {
            return left + right;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Subtract(ColorObject<T> left, IColorObject right)
        {
            return left - right;
        }
        #endregion

        #region Mixing
        /// <summary>
        /// <inheritdoc cref="IColorObject.Mix(IColorObject, float)"/>
        /// </summary>
        public T Mix(IColorObject right, float blendFactor)
        {// Linear Interp:  (x * (1.0 - i) + y * i);
            if (right is null) throw new ArgumentNullException(nameof(right));
            Contract.EndContractBlock();

            var RetVal = GetVector() * (1f - blendFactor) + right.GetVector() * blendFactor;
            RetVal *= fbyteMax;
            return From(RetVal);
        }

        /// <summary>
        /// <inheritdoc cref="IColorObject.MixAlpha(float)"/>
        /// </summary>
        public T MixAlpha(float Factor)
        {
            var RetVal = GetVector();
            RetVal.W *= Factor;
            return From(RetVal);
        }
        #endregion


        #region Equality
        public override bool Equals(object obj)
        {
            if (obj is IColorObject C)
            {
                return C.AsInteger() == AsInteger();
            }
            return false;
        }

        public override int GetHashCode()
        {
            return unchecked((int)AsInteger());
        }

        public static bool operator ==(ColorObject<T> left, IColorObject right)
        {
            if ((left is null) ^ (right is null))// One is null but not both
                return false;

            return left?.Equals(right) ?? false;
        }

        public static bool operator !=(ColorObject<T> left, IColorObject right)
        {
            if ((left is null) ^ (right is null))// One is null but not both
                return true;

            return !(left?.Equals(right)) ?? false;
        }
        #endregion

    }
}
