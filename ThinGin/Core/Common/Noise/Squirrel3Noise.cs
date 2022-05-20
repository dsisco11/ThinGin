using System.Runtime.CompilerServices;

namespace ThinGin.Core.Common.Noise
{
    public class Squirrel3Noise : INoiseProvider
    {
        const float UIntMaxF = uint.MaxValue;

        #region Coordinate Crushers
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Crush_Coordinate(int posX, int posY)
        {
            const int PRIME_NUMBER = 198491317;// Large prime number with non boring bits
            return unchecked(posX + PRIME_NUMBER * posY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Crush_Coordinate(int posX, int posY, int posZ)
        {
            const int PRIME1 = 198491317;// Large prime number with non boring bits
            const int PRIME2 = 6542989;// Large prime number with non boring bits

            return unchecked(posX + PRIME1 * posY + PRIME2 * posZ);
        }
        #endregion

        #region Sampling Unsigned Integers
        /// <summary>
        /// Extremely good and fast hash function
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint SampleUInt(uint seed, int position)
        {// Source: https://www.youtube.com/watch?v=LWFzPP8ZbdU
            const uint BIT_NOISE1 = 0xB5297A4D;// 0b0110'1000'1110'0011'0001'1101'1010'0100;
            const uint BIT_NOISE2 = 0x68E31DA4;// 0b1011'0101'0010'1001'0111'1010'0100'1101;
            const uint BIT_NOISE3 = 0x1B56C4E9;// 0b0001'1011'0101'0110'1100'0100'1110'1001;

            unchecked
            {
                uint mangled = (uint)position;
                mangled *= BIT_NOISE1;
                mangled += seed;
                mangled ^= mangled >> 8;
                mangled += BIT_NOISE2;
                mangled ^= mangled << 8;
                mangled *= BIT_NOISE3;
                mangled ^= mangled >> 8;

                return mangled;
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint SampleUInt(uint seed, int posX, int posY)
        {
            var position = Crush_Coordinate(posX, posY);
            return SampleUInt(seed, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint SampleUInt(uint seed, int posX, int posY, int posZ)
        {
            var position = Crush_Coordinate(posX, posY, posZ);
            return SampleUInt(seed, position);
        }
        #endregion

        #region Sampling Floats

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float SampleF(uint seed, int position) => SampleUInt(seed, position) / UIntMaxF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float SampleF(uint seed, int posX, int posY) => SampleUInt(seed, posX, posY) / UIntMaxF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float SampleF(uint seed, int posX, int posY, int posZ) => SampleUInt(seed, posX, posY, posZ) / UIntMaxF;
        #endregion
    }
}
