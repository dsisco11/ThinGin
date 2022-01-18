using System;
using System.Linq;

namespace ThinGin.Core.Common.Textures
{
    /// <summary>
    /// Describes the components properties of a pixel including; ordering, bit-depth, and datatype
    /// </summary>
    public struct PixelDescriptor
    {
        #region Static Definitions
        public static PixelDescriptor Rgba = new PixelDescriptor(new PixelComponent(EPixelComponent.Red, 8), new PixelComponent(EPixelComponent.Green, 8), new PixelComponent(EPixelComponent.Blue, 8), new PixelComponent(EPixelComponent.Alpha, 8));
        public static PixelDescriptor Rgb = new PixelDescriptor(new PixelComponent(EPixelComponent.Red, 8), new PixelComponent(EPixelComponent.Green, 8), new PixelComponent(EPixelComponent.Blue, 8));

        public static PixelDescriptor Bgra = new PixelDescriptor(new PixelComponent(EPixelComponent.Blue, 8), new PixelComponent(EPixelComponent.Green, 8), new PixelComponent(EPixelComponent.Red, 8), new PixelComponent(EPixelComponent.Alpha, 8));
        public static PixelDescriptor Bgr = new PixelDescriptor(new PixelComponent(EPixelComponent.Blue, 8), new PixelComponent(EPixelComponent.Green, 8), new PixelComponent(EPixelComponent.Red, 8));

        public static PixelDescriptor Float32 = new PixelDescriptor(new PixelComponent(EPixelComponent.Raw, 32));
        public static PixelDescriptor Float1616 = new PixelDescriptor(new PixelComponent(EPixelComponent.Raw, 16), new PixelComponent(EPixelComponent.Raw, 16));
        #endregion

        #region Properties
        /// <summary> All of the components that comprise this pixel format </summary>
        public readonly PixelComponent[] Components;

        /// <summary> Important flags which help describe the pixel format </summary>
        public readonly EPixelDefFlags Flags;

        /// <summary> Number of bits in the pixel </summary>
        public readonly int BitDepth;

        /// <summary> 
        /// If true then the pixels component values will not be reinterpreted and their values will be maintained.
        /// <para>In other words this prevents the GPU from reinterpreting the values into the floating point [0.0 - 1.0] range.</para>
        /// </summary>
        public readonly bool PreserveValues;
        #endregion

        #region Constructors
        public PixelDescriptor(params PixelComponent[] pixelComponents) : this(false, false, pixelComponents)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="hasReversedBits">Indicates whether the bits of the components are stored in the hilarious BigEndian format.</param>
        /// <param name="preserveValues">Hints to the GPU that the pixels component values are not to be reinterpreted into the floating point [0.0 - 1.0] range.</param>
        /// <param name="pixelComponents"></param>
        /// <exception cref="ArgumentException"></exception>
        public PixelDescriptor(bool hasReversedBits, bool preserveValues, params PixelComponent[] pixelComponents)
        {
            Components = pixelComponents;
            PreserveValues = preserveValues;
            Flags = 0x0;
            if (hasReversedBits)
            {
                Flags |= EPixelDefFlags.ReverseBits;
            }

            // Validate the structure
            // This means make sure that there are no duplicate components (except Raw) and that the data structure they describe makes sense
            // (the consecutive bitdepths of components must not increase, their bitdepths must be the same or they must be smaller than the previous components)
            if (pixelComponents.Length < 1)
            {
                throw new ArgumentException($"A {nameof(PixelDescriptor)} must contain atleast a single component!");
            }

            int totalDepth = 0;
            EPixelDefFlags flags = 0x0;

            int lastBits = pixelComponents[0].BitDepth;
            bool[] Present = new bool[6];


            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                totalDepth += component.BitDepth;

                if (component.IsSigned) 
                    flags |= EPixelDefFlags.Signed;
                else 
                    flags |= EPixelDefFlags.Unsigned;


                if (component.IsFloatingPoint) 
                    flags |= EPixelDefFlags.HasFloats;
                else 
                    flags |= EPixelDefFlags.HasIntegers;


                if (component.BitDepth != lastBits)
                {
                    if (component.BitDepth > lastBits)
                    {
                        throw new ArgumentException($"Each component of a {nameof(PixelDescriptor)} must have a bitdepth that is the equal to or less than the previous components!");
                    }

                    flags |= EPixelDefFlags.NonUniform;
                }

                lastBits = component.BitDepth;

                if (component.Type != EPixelComponent.Raw)
                {
                    // Okay so this component isnt a raw value, so we need to validate the all-or-none rule
                    if (Present[(int)EPixelComponent.Raw])
                    {
                        throw new ArgumentException($"This {nameof(PixelDescriptor)} has conflicting components, A pixel must have either all raw components or no raw components!");
                    }

                    if (Present[(int)component.Type])
                    {
                        throw new ArgumentException($"Invalid component decleration, this {nameof(PixelDescriptor)} already has a component of type {component.Type} defined!");
                    }
                }

                Present[(int)component.Type] = true;
            }

            Flags |= flags;
            BitDepth = totalDepth;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            string cStr = string.Join(",", from c in Components select c.ToString());
            return $"{nameof(PixelDescriptor)}[{cStr}]";
        }
        #endregion

        #region Bit checking
        public bool CheckBits(int c1)
        {
            return (Components[0].BitDepth == c1);
        }
        public bool CheckBits(int c1, int c2)
        {
            return (Components[0].BitDepth == c1
                    && Components[1].BitDepth == c2);
        }
        public bool CheckBits(int c1, int c2, int c3)
        {
            return (Components[0].BitDepth == c1
                    && Components[1].BitDepth == c2
                    && Components[2].BitDepth == c3);
        }
        public bool CheckBits(int c1, int c2, int c3, int c4)
        {
            return (Components[0].BitDepth == c1
                    && Components[1].BitDepth == c2
                    && Components[2].BitDepth == c3
                    && Components[3].BitDepth == c4);
        }
        #endregion

        #region Equality
        public bool Equals(EPixelComponent c1)
        {
            if (Components.Length != 1)
                return false;

            return (Components[0].Type == c1);
        }

        public bool Equals(EPixelComponent c1, EPixelComponent c2)
        {
            if (Components.Length != 2)
                return false;

            return (Components[0].Type == c1
                    && Components[1].Type == c2);
        }

        public bool Equals(EPixelComponent c1, EPixelComponent c2, EPixelComponent c3)
        {
            if (Components.Length != 3)
                return false;

            return (Components[0].Type == c1
                    && Components[1].Type == c2
                    && Components[2].Type == c3);
        }

        public bool Equals(EPixelComponent c1, EPixelComponent c2, EPixelComponent c3, EPixelComponent c4)
        {
            if (Components.Length != 4)
                return false;

            return (Components[0].Type == c1
                    && Components[1].Type == c2
                    && Components[2].Type == c3
                    && Components[3].Type == c4);
        }
        #endregion
    }

    public struct PixelComponent
    {
        #region Properties
        /// <summary>
        /// Indicates what the component represents colorwise
        /// </summary>
        public readonly EPixelComponent Type;
        /// <summary>
        /// Number of bits that make up the component
        /// </summary>
        public readonly int BitDepth;
        /// <summary>
        /// If True then the component will be either a half, single, or double floating point number
        /// </summary>
        public readonly bool IsFloatingPoint;

        public readonly bool IsSigned;
        #endregion

        #region Constructors
        public PixelComponent(EPixelComponent type, int bitDepth)
        {
            Type = type;
            BitDepth = bitDepth;
            IsSigned = false;
            IsFloatingPoint = false;
        }
        public PixelComponent(EPixelComponent type, int bitDepth, bool isSigned, bool isFloatingPoint)
        {
            Type = type;
            BitDepth = bitDepth;
            IsSigned = isSigned;
            IsFloatingPoint = isFloatingPoint;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            string signStr = IsSigned ? string.Empty : "u";
            string valueStr = IsFloatingPoint ? "f" : "i";
            return $"{Type}{BitDepth}{signStr}{valueStr}";
        }
        #endregion
    }


    public enum EPixelComponent 
    {
        /// <summary> 
        /// This value would be used when describing some type of non-standard or uncommon format. 
        /// Essentially it indicates that this component is just a value and doesnt represent color. 
        /// <para>Note: A pixel may contain NO raw components or ONLY raw components.</para>
        /// </summary>
        Raw = 0,
        /// <summary> This component contains the red-spectrum value </summary>
        Red,
        /// <summary> This component contains the green-spectrum value </summary>
        Green,
        /// <summary> This component contains the blue-spectrum value </summary>
        Blue,
        /// <summary> This component contains the alpha (transparency) value </summary>
        Alpha
    };

    [Flags]
    public enum EPixelDefFlags : int
    {
        /// <summary> Has integer components </summary>
        HasIntegers = 0x1,
        /// <summary> Has floating-point components </summary>
        HasFloats = 0x2,

        /// <summary> Provides a bitmask for HasIntegers & HasFloats </summary>
        HasBothNumericTypes = 0x3,

        /// <summary> Not all components have the same bit-depth </summary>
        NonUniform = 0x4,

        /// <summary> Has signed components </summary>
        Signed = 0x01,
        /// <summary> Has unsigned components </summary>
        Unsigned = 0x02,

        /// <summary> True if the pixels bits are in reverse order </summary>
        ReverseBits = 0x001,
    }
}
