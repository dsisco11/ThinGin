using System;

namespace ThinGin.Core.Common.Enums
{
    [Flags]
    public enum EPixelFormat : uint
    {
        // Notes: Each bit enum value only occupies 7 bytes, so by tightly packing them we can claim the first 4 bits of the number for our own little flags
        // Offsets
        Channel1 = 4,
        Channel2 = 11,
        Channel3 = 18,
        Channel4 = 25,

        // === Masks ===
        FlagsMask = 0xF,
        BitMask = 0xEF,
        /// <summary> Masks component number 1 </summary>
        ChannelMask1 = 0xEF >> 4,
        /// <summary> Masks component number 2 </summary>
        ChannelMask2 = 0xEF >> 11,
        /// <summary> Masks component number 3 </summary>
        ChannelMask3 = 0xEF >> 18,
        /// <summary> Masks component number 4 </summary>
        ChannelMask4 = 0xEF >> 25,

        // === Flags ===
        /// <summary> The value components are ordered in reverse, eg; BGRA vs RGBA </summary>
        Flag_Reversed = 0x1,
        /// <summary> The value components are ordered with alpha leading, eg; ARGB vs RGBA </summary>
        Flag_AlphaFirst = 0x2,
        /// <summary> The value components are stored as an integer but hardware converted to a normalized floating point value </summary>
        Flag_Normalized = 0x4,
        /// <summary> 
        /// The pixel values are gamma corrected. (aka sRGB)
        /// <note>For textures this indicates that the texture is being uploaded with gamma correction already applied. </note>
        /// <note>For GBuffers this indicates that gamma correction should be applied to the final values. </note>
        /// </summary>
        Flag_GammaCorrected = 0x8,

        // === Formats ===
        PF_Unknown = 0x0,

        PF_Stencil = EValueType.Bool1 >> (int)Channel1,
        PF_D_24F = EValueType.Float24 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2,
        /// <summary> Also known as the "Depth + Stencil" format, contains a single 24bit float and an additional 8bit unsigned integer. </summary>
        PF_DS_24_8 = EValueType.Float24 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2,

        // Unsigned 8-Bit Integer
        PF_R_8 = EValueType.UInt8 >> (int)Channel1,
        PF_RG_8 = EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2,
        PF_RGB_8 = EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2 | EValueType.UInt8 >> (int)Channel3,
        PF_RGBA_8 = EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2 | EValueType.UInt8 >> (int)Channel3 | EValueType.UInt8 >> (int)Channel4,
        PF_ARGB_8 = Flag_AlphaFirst | EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2 | EValueType.UInt8 >> (int)Channel3 | EValueType.UInt8 >> (int)Channel4,

        PF_GR_8 = Flag_Reversed | EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2,
        PF_BGR_8 = Flag_Reversed | EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2 | EValueType.UInt8 >> (int)Channel3,
        PF_BGRA_8 = Flag_Reversed | EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2 | EValueType.UInt8 >> (int)Channel3 | EValueType.UInt8 >> (int)Channel4,
        PF_ABGR_8 = Flag_Reversed | Flag_AlphaFirst | EValueType.UInt8 >> (int)Channel1 | EValueType.UInt8 >> (int)Channel2 | EValueType.UInt8 >> (int)Channel3 | EValueType.UInt8 >> (int)Channel4,

        // Signed 8-Bit Integer
        PF_R_8S = EValueType.Int8 >> (int)Channel1,
        PF_RG_8S = EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2,
        PF_RGB_8S = EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2 | EValueType.Int8 >> (int)Channel3,
        PF_RGBA_8S = EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2 | EValueType.Int8 >> (int)Channel3 | EValueType.Int8 >> (int)Channel4,
        PF_ARGB_8S = Flag_AlphaFirst | EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2 | EValueType.Int8 >> (int)Channel3 | EValueType.Int8 >> (int)Channel4,

        PF_GR_8S = Flag_Reversed | EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2,
        PF_BGR_8S = Flag_Reversed | EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2 | EValueType.Int8 >> (int)Channel3,
        PF_BGRA_8S = Flag_Reversed | EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2 | EValueType.Int8 >> (int)Channel3 | EValueType.Int8 >> (int)Channel4,
        PF_ABGR_8S = Flag_Reversed | Flag_AlphaFirst | EValueType.Int8 >> (int)Channel1 | EValueType.Int8 >> (int)Channel2 | EValueType.Int8 >> (int)Channel3 | EValueType.Int8 >> (int)Channel4,


        // Unsigned 16-Bit Integer
        PF_R_16 = EValueType.UInt16 >> (int)Channel1,
        PF_RG_16 = EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2,
        PF_RGB_16 = EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2 | EValueType.UInt16 >> (int)Channel3,
        PF_RGBA_16 = EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2 | EValueType.UInt16 >> (int)Channel3 | EValueType.UInt16 >> (int)Channel4,
        PF_ARGB_16 = Flag_AlphaFirst | EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2 | EValueType.UInt16 >> (int)Channel3 | EValueType.UInt16 >> (int)Channel4,

        PF_GR_16 = Flag_Reversed | EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2,
        PF_BGR_16 = Flag_Reversed | EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2 | EValueType.UInt16 >> (int)Channel3,
        PF_BGRA_16 = Flag_Reversed | EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2 | EValueType.UInt16 >> (int)Channel3 | EValueType.UInt16 >> (int)Channel4,
        PF_ABGR_16 = Flag_Reversed | Flag_AlphaFirst | EValueType.UInt16 >> (int)Channel1 | EValueType.UInt16 >> (int)Channel2 | EValueType.UInt16 >> (int)Channel3 | EValueType.UInt16 >> (int)Channel4,

        // Signed 16-Bit Integer
        PF_R_16S = EValueType.Int16 >> (int)Channel1,
        PF_RG_16S = EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2,
        PF_RGB_16S = EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2 | EValueType.Int16 >> (int)Channel3,
        PF_RGBA_16S = EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2 | EValueType.Int16 >> (int)Channel3 | EValueType.Int16 >> (int)Channel4,
        PF_ARGB_16S = Flag_AlphaFirst | EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2 | EValueType.Int16 >> (int)Channel3 | EValueType.Int16 >> (int)Channel4,

        PF_GR_16S = Flag_Reversed | EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2,
        PF_BGR_16S = Flag_Reversed | EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2 | EValueType.Int16 >> (int)Channel3,
        PF_BGRA_16S = Flag_Reversed | EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2 | EValueType.Int16 >> (int)Channel3 | EValueType.Int16 >> (int)Channel4,
        PF_ABGR_16S = Flag_Reversed | Flag_AlphaFirst | EValueType.Int16 >> (int)Channel1 | EValueType.Int16 >> (int)Channel2 | EValueType.Int16 >> (int)Channel3 | EValueType.Int16 >> (int)Channel4,


        // Unsigned 32-Bit Integer
        PF_R_32 = EValueType.UInt32 >> (int)Channel1,
        PF_RG_32 = EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2,
        PF_RGB_32 = EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2 | EValueType.UInt32 >> (int)Channel3,
        PF_RGBA_32 = EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2 | EValueType.UInt32 >> (int)Channel3 | EValueType.UInt32 >> (int)Channel4,
        PF_ARGB_32 = Flag_AlphaFirst | EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2 | EValueType.UInt32 >> (int)Channel3 | EValueType.UInt32 >> (int)Channel4,

        PF_GR_32 = Flag_Reversed | EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2,
        PF_BGR_32 = Flag_Reversed | EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2 | EValueType.UInt32 >> (int)Channel3,
        PF_BGRA_32 = Flag_Reversed | EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2 | EValueType.UInt32 >> (int)Channel3 | EValueType.UInt32 >> (int)Channel4,
        PF_ABGR_32 = Flag_Reversed | Flag_AlphaFirst | EValueType.UInt32 >> (int)Channel1 | EValueType.UInt32 >> (int)Channel2 | EValueType.UInt32 >> (int)Channel3 | EValueType.UInt32 >> (int)Channel4,

        // Signed 32-Bit Integer
        PF_R_32S = EValueType.Int32 >> (int)Channel1,
        PF_RG_32S = EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2,
        PF_RGB_32S = EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2 | EValueType.Int32 >> (int)Channel3,
        PF_RGBA_32S = EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2 | EValueType.Int32 >> (int)Channel3 | EValueType.Int32 >> (int)Channel4,
        PF_ARGB_32S = Flag_AlphaFirst | EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2 | EValueType.Int32 >> (int)Channel3 | EValueType.Int32 >> (int)Channel4,

        PF_GR_32S = Flag_Reversed | EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2,
        PF_BGR_32S = Flag_Reversed | EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2 | EValueType.Int32 >> (int)Channel3,
        PF_BGRA_32S = Flag_Reversed | EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2 | EValueType.Int32 >> (int)Channel3 | EValueType.Int32 >> (int)Channel4,
        PF_ABGR_32S = Flag_Reversed | Flag_AlphaFirst | EValueType.Int32 >> (int)Channel1 | EValueType.Int32 >> (int)Channel2 | EValueType.Int32 >> (int)Channel3 | EValueType.Int32 >> (int)Channel4,


        // 16-Bit Floating Point
        PF_R_16F = EValueType.Float16 >> (int)Channel1,
        PF_RG_16F = EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2,
        PF_RGB_16F = EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2 | EValueType.Float16 >> (int)Channel3,
        PF_RGBA_16F = EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2 | EValueType.Float16 >> (int)Channel3 | EValueType.Float16 >> (int)Channel4,
        PF_ARGB_16F = Flag_AlphaFirst | EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2 | EValueType.Float16 >> (int)Channel3 | EValueType.Float16 >> (int)Channel4,

        PF_GR_16F = Flag_Reversed | EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2,
        PF_BGR_16F = Flag_Reversed | EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2 | EValueType.Float16 >> (int)Channel3,
        PF_BGRA_16F = Flag_Reversed | EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2 | EValueType.Float16 >> (int)Channel3 | EValueType.Float16 >> (int)Channel4,
        PF_ABGR_16F = Flag_Reversed | Flag_AlphaFirst | EValueType.Float16 >> (int)Channel1 | EValueType.Float16 >> (int)Channel2 | EValueType.Float16 >> (int)Channel3 | EValueType.Float16 >> (int)Channel4,


        // 32-Bit Floating Point
        PF_R_32F = EValueType.Float32 >> (int)Channel1,
        PF_RG_32F = EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2,
        PF_RGB_32F = EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2 | EValueType.Float32 >> (int)Channel3,
        PF_RGBA_32F = EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2 | EValueType.Float32 >> (int)Channel3 | EValueType.Float32 >> (int)Channel4,
        PF_ARGB_32F = Flag_AlphaFirst | EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2 | EValueType.Float32 >> (int)Channel3 | EValueType.Float32 >> (int)Channel4,

        PF_GR_32F = Flag_Reversed | EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2,
        PF_BGR_32F = Flag_Reversed | EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2 | EValueType.Float32 >> (int)Channel3,
        PF_BGRA_32F = Flag_Reversed | EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2 | EValueType.Float32 >> (int)Channel3 | EValueType.Float32 >> (int)Channel4,
        PF_ABGR_32F = Flag_Reversed | Flag_AlphaFirst | EValueType.Float32 >> (int)Channel1 | EValueType.Float32 >> (int)Channel2 | EValueType.Float32 >> (int)Channel3 | EValueType.Float32 >> (int)Channel4,

    }
}
