using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ThinGin.Core.Common.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Rgba
    {
        #region Properties
        public readonly byte Red;
        public readonly byte Green;
        public readonly byte Blue;
        public readonly byte Alpha;
        #endregion

        #region Constructors
        public Rgba(UInt32 packed)
        {
            var rgb = Rgba.Unpack(packed);
            Red = rgb[0];
            Green = rgb[1];
            Blue = rgb[2];
            Alpha = rgb[3];
        }

        public Rgba(byte red, byte green, byte blue, byte alpha)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }
        #endregion

        /// <summary>
        /// Views the 8-bit RGBA values at this objects address as a 32-bit integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt32 Pack()
        {
            unsafe
            {
                fixed (void* ptr = &this)
                {
                    return *(UInt32*)ptr;
                }
            }
        }

        /// <summary>
        /// Packs the specified RGBA values into an unsigned 32-bit integer comprised of four 8-bit values
        /// </summary>
        public static UInt32 Pack(int R, int G, int B, int A)
        {
            return (UInt32)((R << 0) + (G << 8) + (B << 16) + (A << 24));
        }


        /// <summary>
        /// Packs a set of 8-bit RGBA values into a 32-bit integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte[] Unpack(UInt32 packed)
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == 4);

            var bytes = new byte[4];
            fixed (byte* ptr = bytes)
                *(UInt32*)ptr = packed;

            return bytes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }


        #region Serialization
        /// <summary>
        /// Converts the color to a hexadecimal RGB color string
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToHexRGB()
        {
            return string.Concat("#",
                                 Red.ToString("X2", CultureInfo.InvariantCulture),
                                 Green.ToString("X2", CultureInfo.InvariantCulture),
                                 Blue.ToString("X2", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts the color to a hexadecimal RGBA color string
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToHexRGBA()
        {
            return string.Concat("#",
                                 Red.ToString("X2", CultureInfo.InvariantCulture),
                                 Green.ToString("X2", CultureInfo.InvariantCulture),
                                 Blue.ToString("X2", CultureInfo.InvariantCulture),
                                 Alpha.ToString("X2", CultureInfo.InvariantCulture));
        }

        public string Serialize()
        {
            if (Alpha < byte.MaxValue)
            {
                /* Return RGBA Hex code */
                return ToHexRGBA();
            }

            /* Return RGB Hex code */
            return ToHexRGB();
        }
        #endregion

    }
}
