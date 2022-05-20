using System;
using System.Runtime.CompilerServices;

namespace ThinGin.Core.Common
{
    /// <summary>
    /// Provides commonly needed mathematical utility functions.
    /// </summary>
    public static class MathE
    {
        #region Degrees vs Radians
        const float rtodf = 180.0f / MathF.PI;
        const float dtorf = MathF.PI / 180.0f;

        const double rtodd = 180.0d / MathF.PI;
        const double dtord = MathF.PI / 180.0d;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToRadians(float value) => value * dtorf;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToRadians(double value) => value * dtord;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToDegrees(float value) => value * rtodf;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDegrees(double value) => value * rtodd;
        #endregion


        #region Range Clamp
        // 8-Bit
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ClampRange(byte value, byte A, byte B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ClampRange(sbyte value, sbyte A, sbyte B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));
        
        // 16-Bit
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ClampRange(ushort value, ushort A, ushort B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ClampRange(short value, short A, short B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));

        // 32-Bit
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ClampRange(uint value, uint A, uint B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampRange(int value, int A, int B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));

        // 64-Bit
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ClampRange(ulong value, ulong A, ulong B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ClampRange(long value, long A, long B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));

        // Floating-Point
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ClampRange(float value, float A, float B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ClampRange(double value, double A, double B) => Math.Max(Math.Min(A, B), Math.Min(Math.Max(A, B), value));
        #endregion

        #region Floating point equality

        /// <summary>
        /// Determines the equality of two floating point values based on the delta between them compared to a more reasonable epsilon value than the default.
        /// <note>Note: This is a very simple form of floating point equality comparison, it is only suitable for the most general of cases, if you are doing anything precision related (such as scientific work) then use something else!</note>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>True If the delta between the values is less than <paramref name="epsilon"/> </returns>
        public static bool AlmostEquals(float left, float right)
        {
            const float epsilon = float.Epsilon * 1000;

            var absA = Math.Abs(left);
            var absB = Math.Abs(right);
            var diff = Math.Abs(left - right);

            if (left == right)
            { // shortcut, handles infinities
                return true;
            }
            else if (left == 0 || right == 0 || diff < epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }

        /// <summary>
        /// Determines the equality of two floating point values based on the delta between them compared to a more reasonable epsilon value than the default.
        /// <note>Note: This is a very simple form of floating point equality comparison, it is only suitable for the most general of cases, if you are doing anything precision related (such as scientific work) then use something else!</note>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>True If the delta between the values is less than <paramref name="epsilon"/> </returns>
        public static bool AlmostEquals(double left, double right)
        {
            const double epsilon = double.Epsilon * 1000;

            var absA = Math.Abs(left);
            var absB = Math.Abs(right);
            var diff = Math.Abs(left - right);

            if (left == right)
            { // shortcut, handles infinities
                return true;
            }
            else if (left == 0 || right == 0 || diff < epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }
        #endregion
    }
}
