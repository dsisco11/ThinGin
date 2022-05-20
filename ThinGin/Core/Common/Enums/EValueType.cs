using System;

namespace ThinGin.Core.Common.Enums
{
    /// <summary>
    /// Describes the representation of a value according to its bit-count and signed-ness aswell as if it is floating point or not
    /// </summary>
    [Flags]
    public enum EValueType : byte
    {
        Null = 0,
        /// <summary> 1-Bit Boolean </summary>
        Bool1 = (EBitFlag.BitDepth1),

        /// <summary> 8-Bit Unsigned Integer </summary>
        UInt8 = (EBitFlag.BitDepth8),
        /// <summary> 8-Bit Unsigned Integer </summary>
        UInt8N = (UInt8 | EBitFlag.Normalized),

        /// <summary> 8-Bit Signed Integer </summary>
        Int8 = (UInt8 | EBitFlag.Signed),
        /// <summary> 8-Bit Signed Integer </summary>
        Int8N = (Int8 | EBitFlag.Normalized),


        /// <summary> 16-Bit Unsigned Integer </summary>
        UInt16 = (EBitFlag.BitDepth16),
        /// <summary> 16-Bit Unsigned Integer </summary>
        UInt16N = (UInt16 | EBitFlag.Normalized),

        /// <summary> 16-Bit Signed Integer </summary>
        Int16 = (UInt16 | EBitFlag.Signed),
        /// <summary> 16-Bit Signed Integer </summary>
        Int16N = (Int16 | EBitFlag.Normalized),


        /// <summary> 32-Bit Unsigned Integer </summary>
        UInt32 = (EBitFlag.BitDepth32),
        /// <summary> 32-Bit Unsigned Integer </summary>
        UInt32N = (UInt32 | EBitFlag.Normalized),

        /// <summary> 32-Bit Signed Integer </summary>
        Int32 = (UInt32 | EBitFlag.Signed),
        /// <summary> 32-Bit Signed Integer </summary>
        Int32N = (Int32 | EBitFlag.Normalized),


        /// <summary> 16-Bit Floating-point Integer </summary>
        Float16 = (EBitFlag.BitDepth16 | EBitFlag.Signed),
        /// <summary> 16-Bit Floating-point Integer </summary>
        Float16N = (Float16 | EBitFlag.Normalized),

        /// <summary> 24-Bit Floating-point Integer </summary>
        Float24 = (EBitFlag.BitDepth8 | EBitFlag.BitDepth16 | EBitFlag.Signed),
        /// <summary> 24-Bit Floating-point Integer </summary>
        Float24N = (Float24 | EBitFlag.Normalized),

        /// <summary> 32-Bit Floating-point Integer </summary>
        Float32 = (EBitFlag.BitDepth32 | EBitFlag.Signed),
        /// <summary> 32-Bit Floating-point Integer </summary>
        Float32N = (Float32 | EBitFlag.Normalized),

    }
}
