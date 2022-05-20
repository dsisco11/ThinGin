using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures;

using ThinGin.OpenGL.Common;

namespace ThinGin.OpenGL.GL3
{
    /// <summary>
    /// Provides an engine optimized to utilize a bare minimum of OpenGL 3.0
    /// </summary>
    public class GL3Engine : GLEngineBase
    {
        #region Platform
        public override IGraphicsImplementation Provider => Implementation.Instance;
        #endregion

        #region Constructors
        public GL3Engine(object Context) : base(Context)
        {
        }
        #endregion

        #region Texture Uploading
        // XXX: Need to alter the below functions with the 3.0 deprecations in mind (Docs: https://www.khronos.org/opengl/wiki/History_of_OpenGL#Deprecation_Model)

        public override PixelType Get_PixelType(PixelDescriptor Layout)
        {
            // First things first, lets detect standard (uniform) types
            if (!Layout.Flags.HasFlag(EPixelDefFlags.NonUniform))
            {
                if (Layout.Flags.HasFlag(EPixelDefFlags.HasFloats))
                {
                    throw new FormatException($"No supported OpenGL formats for non-uniform pixel layouts of floating-point components!");
                }

                bool bitsReversed = Layout.Flags.HasFlag(EPixelDefFlags.ReverseBits);
                bool isUnsigned = Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);
                int compCount = Layout.Components.Length;
                bool reverseLayout = BitConverter.IsLittleEndian ^ bitsReversed;
                // Okay this is going to be annoying, we are in non-uniform territory here.

                // First things first, in the specs all non-uniform formats are comprised of a minimum of 3 components.
                if (compCount < 3)
                {
                    throw new FormatException($"OpenGL has no supported non-uniform pixel layouts with less than 3 components!");
                }

                if (!isUnsigned)
                {
                    throw new FormatException($"OpenGL has no supported non-uniform pixel layouts operating on a signed numbers (All non-uniform layouts must use unsigned numbers)!");
                }

                // Ok so now we can safely create a reference to the first three components!
                var b1 = Layout.Components[0].BitDepth;
                var b2 = Layout.Components[1].BitDepth;
                var b3 = Layout.Components[2].BitDepth;

                // Switch on the whole pixel carryer datatype
                switch (Layout.BitDepth)
                {
                    case 8:
                        {
                            if (b1 == 3 && b3 == 2)
                            {
                                return reverseLayout ? PixelType.UnsignedByte332 : PixelType.UnsignedByte233Reversed;
                            }
                            else if (b1 == 2 && b3 == 3)
                            {
                                return reverseLayout ? PixelType.UnsignedByte233Reversed : PixelType.UnsignedByte332;
                            }

                            return isUnsigned ? PixelType.UnsignedByte : PixelType.Byte;
                        }
                    case 16:
                        {
                            if (compCount == 4)
                            {
                                if (b1 == 1 && b2 == 5 && b3 == 5)
                                {
                                    return reverseLayout ? PixelType.UnsignedShort1555Reversed : PixelType.UnsignedShort5551;
                                }
                                else if (b1 == 5 && b2 == 5 && b3 == 5) // There is only space left for a component of 1-bit, so for grace we assume it is.
                                {
                                    return reverseLayout ? PixelType.UnsignedShort5551 : PixelType.UnsignedShort1555Reversed;
                                }
                            }
                            else if (compCount == 3)
                            {
                                if (b1 == 5 && b2 == 6)
                                {
                                    return reverseLayout ? PixelType.UnsignedShort565Reversed : PixelType.UnsignedShort565;
                                }
                            }

                            return isUnsigned ? PixelType.UnsignedShort : PixelType.Short;
                        }
                    case 32:
                        {
                            if (compCount == 2)
                            {
                                if (b1 == 24 && b2 == 8)
                                {
                                    return PixelType.UnsignedInt248;
                                }
                            }
                            else if (compCount == 3)
                            {
                                throw new FormatException("OpenGL does not support non-uniform 32-bit pixel layouts with 3 components!");
                            }
                            else if (compCount == 4)
                            {
                                if (b1 == 10 && b2 == 10 && b3 == 10)
                                {
                                    return reverseLayout ? PixelType.UnsignedInt1010102 : PixelType.UnsignedInt2101010Reversed;
                                }
                                else if (b1 == 2 && b2 == 10 && b3 == 10)
                                {
                                    return reverseLayout ? PixelType.UnsignedInt2101010Reversed : PixelType.UnsignedInt1010102;
                                }
                                else if (b1 == 5 && b2 == 9 && b3 == 9)
                                {
                                    return PixelType.UnsignedInt5999Rev;
                                }
                            }
                            // Fail gracefully, the user will get garbage rendering but it wont cause corruption
                            return isUnsigned ? PixelType.UnsignedInt : PixelType.Int;
                        }
                    default:
                        {
                            return isUnsigned ? PixelType.UnsignedByte : PixelType.Byte;
                        }
                }

            }
            else
            {
                if ((Layout.Flags & EPixelDefFlags.HasBothNumericTypes) != EPixelDefFlags.HasBothNumericTypes)// Grab the values for the float/int flags by masking them, then compare that result to the masking value to see if all masked flags are present
                {
                    int bitsPerComp = Layout.Components[0].BitDepth;
                    // Okay so now we know that our components are all either floating-point or integers
                    if (Layout.Flags.HasFlag(EPixelDefFlags.HasFloats))
                    {
                        switch (bitsPerComp)
                        {
                            case 16: return PixelType.HalfFloat;
                            case 32: return PixelType.Float;
                            // To heck with the user, we are defaulting to full floats if they specified anything stupid
                            default: return PixelType.Float;
                        }
                    }
                    else// Using only whole numeric types & all components are the same size
                    {
                        bool bitsReversed = Layout.Flags.HasFlag(EPixelDefFlags.ReverseBits);
                        bool isUnsigned = Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);
                        int compCount = Layout.Components.Length;
                        bool reverseLayout = BitConverter.IsLittleEndian ^ bitsReversed;

                        if (compCount == 1 && Layout.Components[0].BitDepth == 1)
                            return PixelType.Bitmap;

                        switch (bitsPerComp)
                        {
                            case 32: return isUnsigned ? PixelType.UnsignedInt : PixelType.Int;
                            case 16: return isUnsigned ? PixelType.UnsignedShort : PixelType.Short;
                            case 4:// Not even sure if 4bit formats are supported anymore (Addendum: they are! people use them for fonts apparently, but alpha textures are claimed to be better)
                                {
                                    if (compCount != 4)
                                    {
                                        // This is the only UNIFORM short format, so we are forgiving and just lump all uniform formats of 4bit component depth into this category,
                                        // regardless of how many components they actually have
                                        System.Diagnostics.Trace.TraceWarning($"No supported OpenGL format detected for the described pixel layout, using closest match: 4 components of 4-bits each!");
                                    }

                                    return reverseLayout ? PixelType.UnsignedShort4444Reversed : PixelType.UnsignedShort4444;
                                }
                            case 8:
                            default:
                                {
                                    switch (compCount)
                                    {
                                        case 4:
                                            return reverseLayout ? PixelType.UnsignedInt8888Reversed : PixelType.UnsignedInt8888;
                                        default:
                                            {
                                                return isUnsigned ? PixelType.UnsignedByte : PixelType.Byte;
                                            }
                                    }
                                }

                        }
                    }
                }

                throw new FormatException("Cannot find a supported OpenGL pixel layout to match the one given!");
            }
        }

        public override PixelFormat Get_PixelFormat(PixelDescriptor Layout)
        {
            switch (Layout.Components.Length)
            {
                case 1:
                    {
                        if (Layout.Equals(ECixel.Red))
                            return Layout.PreserveValues ? PixelFormat.RedInteger : PixelFormat.Red;

                        if (Layout.Equals(ECixel.Green))
                            return Layout.PreserveValues ? PixelFormat.GreenInteger : PixelFormat.Green;

                        if (Layout.Equals(ECixel.Blue))
                            return Layout.PreserveValues ? PixelFormat.BlueInteger : PixelFormat.Blue;

                        if (Layout.Equals(ECixel.Alpha))
                            return Layout.PreserveValues ? PixelFormat.AlphaInteger : PixelFormat.Alpha;

                        if (Layout.Equals(ECixel.Raw))
                            return Layout.PreserveValues ? PixelFormat.RedInteger : PixelFormat.Red;
                    }
                    break;
                case 2:
                    {
                        if (Layout.Equals(ECixel.Red, ECixel.Green))
                            return Layout.PreserveValues ? PixelFormat.RgInteger : PixelFormat.Rg;

                        if (Layout.Equals(ECixel.Raw, ECixel.Raw))
                            return Layout.PreserveValues ? PixelFormat.RgInteger : PixelFormat.Rg;
                    }
                    break;
                case 3:
                    {
                        if (Layout.Equals(ECixel.Red, ECixel.Green, ECixel.Blue))
                            return Layout.PreserveValues ? PixelFormat.RgbInteger : PixelFormat.Rgb;

                        if (Layout.Equals(ECixel.Blue, ECixel.Green, ECixel.Red))
                            return Layout.PreserveValues ? PixelFormat.BgrInteger : PixelFormat.Bgr;
                    }
                    break;
                case 4:
                    {
                        if (Layout.Equals(ECixel.Alpha, ECixel.Blue, ECixel.Green, ECixel.Red))
                            return PixelFormat.AbgrExt;

                        if (Layout.Equals(ECixel.Blue, ECixel.Green, ECixel.Red, ECixel.Alpha))
                            return Layout.PreserveValues ? PixelFormat.BgraInteger : PixelFormat.Bgra;

                        if (Layout.Equals(ECixel.Red, ECixel.Green, ECixel.Blue, ECixel.Alpha))
                            return Layout.PreserveValues ? PixelFormat.RgbaInteger : PixelFormat.Rgba;
                    }
                    break;

            }

            throw new FormatException($"OpenGL does not support pixel order: ({Layout})");
        }

        public override PixelInternalFormat Get_Internal_PixelFormat(PixelDescriptor Layout, bool UseCompression)
        {
            if (UseCompression && IsSupported("ext_texture_compression_s3tc"))
            {
                int componentCount = Layout.Components.Length;
                switch (componentCount)
                {
                    case 3: return PixelInternalFormat.CompressedRgbS3tcDxt1Ext;
                    case 4: return PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
                }
            }

            // Can only have an entirely signed or unsigned format
            bool conflictingSigns = Layout.Flags.HasFlag(EPixelDefFlags.Signed) ^ Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);
            if (conflictingSigns)
            {
                if (Layout.Flags.HasFlag(EPixelDefFlags.NonUniform))
                {
                    int componentCount = Layout.Components.Length;
                    bool Unsigned = Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);

                    switch (componentCount)
                    {
                        case 4:
                            {
                                if (Layout.CheckBits(10, 10, 10, 2)) return Unsigned ? PixelInternalFormat.Rgb10A2ui : PixelInternalFormat.Rgb10A2;
                                if (Layout.CheckBits(5, 5, 5, 1)) return PixelInternalFormat.Rgb5A1;
                            }
                            break;
                        case 3:
                            {
                                if (Layout.CheckBits(9, 9, 9)) return PixelInternalFormat.Rgb9E5;
                                //if (Layout.CheckBits(5, 6, 5)) return PixelInternalFormat.Rgb565;
                            }
                            break;
                        case 2:
                            {
                                if (Layout.CheckBits(32, 8) && Layout.Flags.HasFlag(EPixelDefFlags.HasFloats)) return PixelInternalFormat.Depth32fStencil8;
                                if (Layout.CheckBits(24, 8)) return PixelInternalFormat.Depth24Stencil8;
                            }
                            break;
                        case 1:
                            {
                                if (Layout.CheckBits(32)) return Layout.Flags.HasFlag(EPixelDefFlags.HasFloats) ? PixelInternalFormat.DepthComponent32f : PixelInternalFormat.DepthComponent32;
                                if (Layout.CheckBits(24)) return PixelInternalFormat.DepthComponent24;
                                if (Layout.CheckBits(16)) return PixelInternalFormat.DepthComponent16;
                            }
                            break;
                    }
                }
                else
                {
                    bool Unsigned = Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);
                    bool IsRaw = Layout.Components[0].Type == ECixel.Raw;
                    int componentCount = Layout.Components.Length;
                    int Bits = Layout.Components[0].BitDepth;

                    switch (Bits)
                    {
                        case 32:
                            {
                                switch (componentCount)
                                {
                                    case 1: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.R32ui : PixelInternalFormat.R32i : PixelInternalFormat.R32f;
                                    case 2: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rg32ui : PixelInternalFormat.Rg32i : PixelInternalFormat.Rg32f;
                                    case 3: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgb32ui : PixelInternalFormat.Rgb32i : PixelInternalFormat.Rgb32f;
                                    case 4: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgba32ui : PixelInternalFormat.Rgba32i : PixelInternalFormat.Rgba32f;
                                }
                            }
                            break;
                        case 16:
                            {
                                switch (componentCount)
                                {
                                    case 1: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.R16ui : PixelInternalFormat.R16i : PixelInternalFormat.R16f;
                                    case 2: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rg16ui : PixelInternalFormat.Rg16i : PixelInternalFormat.Rg16f;
                                    case 3: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgb16ui : PixelInternalFormat.Rgb16i : PixelInternalFormat.Rgb16f;
                                    case 4: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgba16ui : PixelInternalFormat.Rgba16i : PixelInternalFormat.Rgba16f;
                                }
                            }
                            break;
                        case 8:
                            {
                                switch (componentCount)
                                {
                                    case 1: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.R8ui : PixelInternalFormat.R8i : PixelInternalFormat.R8;
                                    case 2: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rg8ui : PixelInternalFormat.Rg8i : PixelInternalFormat.Rg8;
                                    case 3: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgb8ui : PixelInternalFormat.Rgb8i : PixelInternalFormat.Rgb8;
                                    case 4: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgba8ui : PixelInternalFormat.Rgba8i : PixelInternalFormat.Rgba8;
                                }
                            }
                            break;
                        case 4:
                            {
                                switch (componentCount)
                                {
                                    case 3: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgb4 : PixelInternalFormat.Rgb4 : PixelInternalFormat.Rgb4;
                                    case 4: return Layout.PreserveValues ? Unsigned ? PixelInternalFormat.Rgba4 : PixelInternalFormat.Rgba4 : PixelInternalFormat.Rgba4;
                                }
                            }
                            break;
                    }
                }
            }

            throw new FormatException($"OpenGL does have support for a gpu pixel layout matching: {Layout}!");
        }

        public override InternalFormat Get_Compressed_Internal_PixelFormat(PixelDescriptor Layout)
        {
            if (IsSupported("ext_texture_compression_s3tc"))
            {
                int componentCount = Layout.Components.Length;
                switch (componentCount)
                {
                    case 3: return InternalFormat.CompressedRgbS3tcDxt1Ext;
                    case 4: return InternalFormat.CompressedRgbaS3tcDxt5Ext;
                }
            }

            // Can only have an entirely signed or unsigned format.
            bool conflictingSigns = Layout.Flags.HasFlag(EPixelDefFlags.Signed) ^ Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);
            if (conflictingSigns)
            {
                if (Layout.Flags.HasFlag(EPixelDefFlags.NonUniform))
                {
                    int componentCount = Layout.Components.Length;
                    bool Unsigned = Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);

                    switch (componentCount)
                    {
                        case 4:
                            {
                                if (Layout.CheckBits(10, 10, 10, 2)) return Unsigned ? InternalFormat.Rgb10A2ui : InternalFormat.Rgb10A2;
                                if (Layout.CheckBits(5, 5, 5, 1)) return InternalFormat.Rgb5A1;
                            }
                            break;
                        case 3:
                            {
                                if (Layout.CheckBits(9, 9, 9)) return InternalFormat.Rgb9E5;
                                //if (Layout.CheckBits(5, 6, 5)) return InternalFormat.Rgb565;
                            }
                            break;
                        case 2:
                            {
                                if (Layout.CheckBits(32, 8) && Layout.Flags.HasFlag(EPixelDefFlags.HasFloats)) return InternalFormat.Depth32fStencil8;
                                if (Layout.CheckBits(24, 8)) return InternalFormat.Depth24Stencil8;
                            }
                            break;
                        case 1:
                            {
                                if (Layout.CheckBits(32)) return Layout.Flags.HasFlag(EPixelDefFlags.HasFloats) ? InternalFormat.DepthComponent32f : InternalFormat.DepthComponent32f;
                                if (Layout.CheckBits(24)) return InternalFormat.DepthComponent24Arb;
                                if (Layout.CheckBits(16)) return InternalFormat.DepthComponent16;
                            }
                            break;
                    }
                }
                else
                {
                    bool Unsigned = Layout.Flags.HasFlag(EPixelDefFlags.Unsigned);
                    bool IsRaw = Layout.Components[0].Type == ECixel.Raw;
                    int componentCount = Layout.Components.Length;
                    int Bits = Layout.Components[0].BitDepth;

                    switch (Bits)
                    {
                        case 32:
                            {
                                switch (componentCount)
                                {
                                    case 1: return Layout.PreserveValues ? Unsigned ? InternalFormat.R32ui : InternalFormat.R32i : InternalFormat.R32f;
                                    case 2: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rg32ui : InternalFormat.Rg32i : InternalFormat.Rg32f;
                                    case 3: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgb32ui : InternalFormat.Rgb32i : InternalFormat.Rgb32ui;
                                    case 4: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgba32ui : InternalFormat.Rgba32i : InternalFormat.Rgba32f;
                                }
                            }
                            break;
                        case 16:
                            {
                                switch (componentCount)
                                {
                                    case 1: return Layout.PreserveValues ? Unsigned ? InternalFormat.R16ui : InternalFormat.R16i : InternalFormat.R16f;
                                    case 2: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rg16ui : InternalFormat.Rg16i : InternalFormat.Rg16f;
                                    case 3: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgb16ui : InternalFormat.Rgb16i : InternalFormat.Rgb16f;
                                    case 4: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgba16ui : InternalFormat.Rgba16i : InternalFormat.Rgba16f;
                                }
                            }
                            break;
                        case 8:
                            {
                                switch (componentCount)
                                {
                                    case 1: return Layout.PreserveValues ? Unsigned ? InternalFormat.R8ui : InternalFormat.R8i : InternalFormat.R8;
                                    case 2: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rg8ui : InternalFormat.Rg8i : InternalFormat.Rg8;
                                    case 3: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgb8ui : InternalFormat.Rgb8i : InternalFormat.Rgb8;
                                    case 4: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgba8ui : InternalFormat.Rgba8i : InternalFormat.Rgba8;
                                }
                            }
                            break;
                        case 4:
                            {
                                switch (componentCount)
                                {
                                    case 3: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgb4 : InternalFormat.Rgb4 : InternalFormat.Rgb4;
                                    case 4: return Layout.PreserveValues ? Unsigned ? InternalFormat.Rgba4 : InternalFormat.Rgba4 : InternalFormat.Rgba4;
                                }
                            }
                            break;
                    }
                }
            }

            throw new FormatException($"OpenGL does have support for a gpu pixel layout matching: {Layout}!");
        }
        #endregion
    }
}
