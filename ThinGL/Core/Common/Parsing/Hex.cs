using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ThinGin.Core.Common.Parsing
{
    public static class Hex
    {
        #region Lookup Table
        static Encoding ascii_encoding = Encoding.GetEncoding("iso-8859-1");
        private static ReadOnlyMemory<uint> HexLUT = Generate_HexTable();
        /// <summary>
        /// Pre-Generates the hexadecimal value for all values within the 8-bit byte range, 
        /// so when converting a byte to hex we can just use a direct memory lookup table.
        /// </summary>
        /// <returns></returns>
        private static ReadOnlyMemory<uint> Generate_HexTable()
        {
            var table = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string hex = i.ToString("X2");// convert this byte value to a hex string
                var b1 = (uint)hex[0];
                var b2 = (uint)hex[1];
                table[i] = b1 + (b2 << 16);
            }

            return new Memory<uint>(table);
        }
        #endregion

        #region IsHexChar
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool IsHexChar(char c)
        {
            return c switch
            {
                '0' => true,
                '1' => true,
                '2' => true,
                '3' => true,
                '4' => true,
                '5' => true,
                '6' => true,
                '7' => true,
                '8' => true,
                '9' => true,

                'a' => true,
                'b' => true,
                'c' => true,
                'd' => true,
                'e' => true,
                'f' => true,

                'A' => true,
                'B' => true,
                'C' => true,
                'D' => true,
                'E' => true,
                'F' => true,
                _ => false,
            };
        }
        #endregion

        #region From
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static uint _hex_to_value(char hexChar)
        {
            switch (hexChar)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;

                case 'a': return 10;
                case 'b': return 11;
                case 'c': return 12;
                case 'd': return 13;
                case 'e': return 14;
                case 'f': return 15;

                case 'A': return 10;
                case 'B': return 11;
                case 'C': return 12;
                case 'D': return 13;
                case 'E': return 14;
                case 'F': return 15;

                default: throw new ArgumentException($"Illegal character({hexChar}) specified! not a valid hex-code character.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static Memory<byte> From(ReadOnlySpan<char> HexString)
        {
            // if the hex string starts with '0x' or just 'x' then discard that portion
            if (HexString[0] == 'x') HexString = HexString.Slice(1);
            if (HexString[0] == '0' && HexString[1] == 'x') HexString = HexString.Slice(2);

            // Hexadecimal strings must always be evenly divisible by 2 because each individual byte is represented by 2 hex characters
            if ((HexString.Length % 2) != 0) throw new FormatException("Invalid hexadecimal string (Not evenly divisible by 2)!");

            int bufLen = HexString.Length / 2;
            var buf = new byte[bufLen];

            for (int i = 0; i < bufLen; i++)
            {
                var b1 = _hex_to_value(HexString[0]);
                var b2 = _hex_to_value(HexString[1]);
                var x = b1 + (b2 >> 4);
                buf[i] = (byte)x;// Set the byte
                HexString = HexString.Slice(2);// Progress the buffer
            }
            return new Memory<byte>(buf);
        }
        #endregion

        #region To
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static string To(ReadOnlyMemory<byte> data)
        {
            int bufLen = data.Length * 2;
            var buf = new Span<char>(new char[bufLen]);

            for (int i = 0; i < data.Length; i++)
            {
                byte x = data.Span[i];
                var hexValue = HexLUT.Span[x];

                var n = i * 2;
                buf[n + 0] = (char)hexValue;
                buf[n + 1] = (char)(hexValue >> 16);
            }

            return buf.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string To(ReadOnlySpan<char> data)
        {
            var bytes = ascii_encoding.GetBytes(data.ToArray());
            return To(bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string To(ReadOnlyMemory<char> data)
        {
            var bytes = ascii_encoding.GetBytes(data.Span.ToArray());
            return To(bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHex(this ReadOnlySpan<char> data)
        {
            var bytes = ascii_encoding.GetBytes(data.ToArray());
            return To(bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHex(this ReadOnlyMemory<char> data)
        {
            var bytes = ascii_encoding.GetBytes(data.Span.ToArray());
            return To(bytes);
        }

        #endregion
    }
}
