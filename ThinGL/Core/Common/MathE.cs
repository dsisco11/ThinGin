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

        #region Min
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int A, int B) => (A < B ? A : B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long A, long B) => (A < B ? A : B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float A, float B) => (A < B ? A : B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double A, double B) => (A < B ? A : B);
        #endregion

        #region Max
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int A, int B) => (A > B ? A : B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Max(long A, long B) => (A > B ? A : B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float A, float B) => (A > B ? A : B);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double A, double B) => (A > B ? A : B);
        #endregion

        #region Clamp
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max) => Max(min, Min(max, value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long value, long min, long max) => Max(min, Min(max, value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max) => Max(min, Min(max, value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(double value, double min, double max) => Max(min, Min(max, value));
        #endregion


        #region Range Clamp
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RangeClamp(int value, int a, int b) { var min = (a < b ? a : b); var max = (a < b ? b : a); return Max(min, Min(max, value)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RangeClamp(long value, long a, long b) { var min = (a < b ? a : b); var max = (a < b ? b : a); return Max(min, Min(max, value)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RangeClamp(float value, float a, float b) { var min = (a < b ? a : b); var max = (a < b ? b : a); return Max(min, Min(max, value)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RangeClamp(double value, double a, double b) { var min = (a < b ? a : b); var max = (a < b ? b : a); return Max(min, Min(max, value)); }
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
