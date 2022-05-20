using ThinGin.Core.Common.Data.Interfaces;
using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Data
{
    /// <summary>
    /// Describes the layout of some small data structure in memory, comprised of multiple components/instances of a single value type.
    /// Describes how many components comprise the data structure and what value type said components are.
    /// </summary>
    public class AttributeDescriptor : IAttributeDescriptor
    {
        #region Values
        /// <summary> <inheritdoc cref="IDataAlignmentDescriptor.Size"/> </summary>
        public int Size { get; private set; } = 0;

        /// <summary> <inheritdoc cref="IDataDescriptor.Count"/> </summary>
        public int Count { get; private set; } = 0;


        /// <summary> <inheritdoc cref="IDataAlignmentDescriptor.Offset"/> </summary>
        public int Offset { get; set; } = 0;

        /// <summary> <inheritdoc cref="IDataAlignmentDescriptor.Stride"/> </summary>
        public int Stride { get; set; } = 0;

        /// <summary>
        /// The type of each component
        /// </summary>
        public EValueType ValueType { get; private set; } = EValueType.Null;

        /// <summary> <inheritdoc cref="IDataAlignmentDescriptor.Interleaved"/> </summary>
        public bool Interleaved { get; set; } = true;

        /// <summary> <inheritdoc cref="IAttributeDescriptor.Normalize"/> </summary>
        public bool Normalize { get; set; } = false;

        /// <summary> <inheritdoc cref="IAttributeDescriptor.IndexDivisor"/> </summary>
        public int IndexDivisor { get; set; } = 0;
        #endregion

        #region Accessors
        /// <summary> <inheritdoc cref="IDataDescriptor.ValueSize"/> </summary>
        public int ValueSize
        {
            get
            {// This value is meant for the gpu really, so even though an "int" for .net can technically be larger than 32-bits, for the GPU hardware its always 32-bit.
                return ValueType switch
                {
                    EValueType.Null => 0,
                    EValueType.UInt8 => 1,
                    EValueType.Int8 => 1,
                    EValueType.Float16 => 2,
                    EValueType.Float32 => 4,
                    EValueType.DOUBLE => 8,
                    EValueType.Int16 => 2,
                    EValueType.UInt16 => 2,
                    EValueType.Int32 => 4,
                    EValueType.UInt32 => 4,
                    _ => throw new System.NotImplementedException($"Logical handling not implemented for {nameof(EValueType)}.{System.Enum.GetName(typeof(EValueType), ValueType)}"),
                };
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a deep copy of the given descriptor
        /// </summary>
        public AttributeDescriptor(AttributeDescriptor other)
        {
            this.Count = other.Count;
            this.Offset = other.Offset;
            this.Stride = other.Stride;
            this.ValueType = other.ValueType;
            this.Interleaved = other.Interleaved;
            this.Size = this.Count * this.ValueSize;
        }


        /// <summary>
        /// Creates a new data layout descriptor
        /// </summary>
        public AttributeDescriptor()
        {
        }

        /// <summary>
        /// Creates a new data layout descriptor
        /// </summary>
        /// <param name="ValueType">Data type</param>
        public AttributeDescriptor(EValueType ValueType)
        {
            this.Count = 1;
            this.ValueType = ValueType;
            this.Size = this.Count * this.ValueSize;
        }

        /// <summary>
        /// Creates a new data layout descriptor
        /// </summary>
        /// <param name="Count">Number of components which comprise a single instance</param>
        /// <param name="ValueType">Data type</param>
        public AttributeDescriptor(int Count, EValueType ValueType)
        {
            this.Count = Count;
            this.ValueType = ValueType;
            this.Size = this.Count * this.ValueSize;
        }

        /// <summary>
        /// Creates a new data layout descriptor
        /// </summary>
        /// <param name="Count">Number of components which comprise a single instance</param>
        /// <param name="ValueType">Data type</param>
        public AttributeDescriptor(int Count, EValueType ValueType, int Offset, int Stride)
        {
            this.Count = Count;
            this.Offset = Offset;
            this.Stride = Stride;
            this.ValueType = ValueType;
            this.Size = this.Count * this.ValueSize;
        }
        #endregion

        #region Common Formats

        #region 8-bit Unsigned Integer
        public static AttributeDescriptor BYTE1 => new AttributeDescriptor(1, EValueType.UInt8);
        public static AttributeDescriptor BYTE2 => new AttributeDescriptor(2, EValueType.UInt8);
        public static AttributeDescriptor BYTE3 => new AttributeDescriptor(3, EValueType.UInt8);
        public static AttributeDescriptor BYTE4 => new AttributeDescriptor(4, EValueType.UInt8);
        #endregion

        #region 8-bit Signed Integer
        public static AttributeDescriptor SBYTE1 => new AttributeDescriptor(1, EValueType.Int8);
        public static AttributeDescriptor SBYTE2 => new AttributeDescriptor(2, EValueType.Int8);
        public static AttributeDescriptor SBYTE3 => new AttributeDescriptor(3, EValueType.Int8);
        public static AttributeDescriptor SBYTE4 => new AttributeDescriptor(4, EValueType.Int8);
        #endregion

        #region 16-bit Unsigned Integer
        public static AttributeDescriptor USHORT1 => new AttributeDescriptor(1, EValueType.UInt16);
        public static AttributeDescriptor USHORT2 => new AttributeDescriptor(2, EValueType.UInt16);
        public static AttributeDescriptor USHORT3 => new AttributeDescriptor(3, EValueType.UInt16);
        public static AttributeDescriptor USHORT4 => new AttributeDescriptor(4, EValueType.UInt16);
        #endregion

        #region 16-bit Signed Integer
        public static AttributeDescriptor SHORT1 => new AttributeDescriptor(1, EValueType.Int16);
        public static AttributeDescriptor SHORT2 => new AttributeDescriptor(2, EValueType.Int16);
        public static AttributeDescriptor SHORT3 => new AttributeDescriptor(3, EValueType.Int16);
        public static AttributeDescriptor SHORT4 => new AttributeDescriptor(4, EValueType.Int16);
        #endregion

        #region 32-bit Unsigned Integer
        public static AttributeDescriptor UINT1 => new AttributeDescriptor(1, EValueType.UInt32);
        public static AttributeDescriptor UINT2 => new AttributeDescriptor(2, EValueType.UInt32);
        public static AttributeDescriptor UINT3 => new AttributeDescriptor(3, EValueType.UInt32);
        public static AttributeDescriptor UINT4 => new AttributeDescriptor(4, EValueType.UInt32);
        #endregion

        #region 32-bit Signed Integer
        public static AttributeDescriptor INT1 => new AttributeDescriptor(1, EValueType.Int32);
        public static AttributeDescriptor INT2 => new AttributeDescriptor(2, EValueType.Int32);
        public static AttributeDescriptor INT3 => new AttributeDescriptor(3, EValueType.Int32);
        public static AttributeDescriptor INT4 => new AttributeDescriptor(4, EValueType.Int32);
        #endregion


        #region Float
        public static AttributeDescriptor FLOAT1 => new AttributeDescriptor(1, EValueType.Float32);
        public static AttributeDescriptor FLOAT2 => new AttributeDescriptor(2, EValueType.Float32);
        public static AttributeDescriptor FLOAT3 => new AttributeDescriptor(3, EValueType.Float32);
        public static AttributeDescriptor FLOAT4 => new AttributeDescriptor(4, EValueType.Float32);
        #endregion

        #region Double
        public static AttributeDescriptor DOUBLE1 => new AttributeDescriptor(1, EValueType.DOUBLE);
        public static AttributeDescriptor DOUBLE2 => new AttributeDescriptor(2, EValueType.DOUBLE);
        public static AttributeDescriptor DOUBLE3 => new AttributeDescriptor(3, EValueType.DOUBLE);
        public static AttributeDescriptor DOUBLE4 => new AttributeDescriptor(4, EValueType.DOUBLE);
        #endregion

        #endregion
    }
}
