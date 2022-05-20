using System;

namespace ThinGin.Core.Common.Enums
{

    /// <summary>
    /// Defines bitwise flags used to construct enum constants which indicate value types
    /// </summary>
    [Flags]
    public enum EBitFlag : byte
    {
        // Signed-ness
        /// <summary> Flag for unsigned value types </summary>
        Unsigned = 0x0,
        /// <summary> This flag just reserves the first bit for other purposes </summary>
        Reserved = 0x1,
        /// <summary> Flag for signed value types </summary>
        Signed = 0x2,

        // Normalized-ness
        /// <summary> Flag values that should be normalized into the range [0 - 1] </summary>
        Normalized = 0x4,

        // Float-ness
        /// <summary> Flag for floating point value types </summary>
        FloatingPoint = 0x8,

        // Bit-Depth
        /// <summary> Flag for value types encompassing 1 bit </summary>
        BitDepth1 = 0x10,
        /// <summary> Flag for value types encompassing 8 bits </summary>
        BitDepth8 = 0x20,
        /// <summary> Flag for value types encompassing 16 bits </summary>
        BitDepth16 = 0x40,
        /// <summary> Flag for value types encompassing 32 bits </summary>
        BitDepth32 = 0x80,
    }
}
