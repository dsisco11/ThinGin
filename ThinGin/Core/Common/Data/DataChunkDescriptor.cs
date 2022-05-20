using ThinGin.Core.Common.Data.Interfaces;
using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Data
{
    /// <summary>
    /// Describes the layout of some small data structure in memory, comprised of multiple components/instances of a single value type.
    /// Describes how many components comprise the data structure and what value type said components are.
    /// </summary>
    public class DataChunkDescriptor : IDataDescriptor
    {
        #region Values
        /// <summary>
        /// Size (in bytes) of the entire structure
        /// </summary>
        public readonly int _size = 0;

        /// <summary>
        /// Number of components within the structure
        /// </summary>
        public readonly int _count = 0;

        /// <summary>
        /// The type of each component
        /// </summary>
        public readonly EValueType _value_type = EValueType.Null;
        #endregion

        #region Accessors
        public int Size => _size;
        public int Count => _count;
        public EValueType ValueType => _value_type;

        /// <summary>
        /// Size (in bytes) of the value type used by the components of the structure
        /// </summary>
        public int ValueSize
        {
            get
            {// This value is meant for the gpu really, so even though an "int" for .net can technically be larger than 32-bits, for the GPU hardware its always 32-bit.
                return _value_type switch
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
                    _ => throw new System.NotImplementedException($"Logical handling not implemented for {nameof(EValueType)}.{System.Enum.GetName(typeof(EValueType), _value_type)}"),
                };
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a deep copy of the given descriptor
        /// </summary>
        public DataChunkDescriptor(DataChunkDescriptor other)
        {
            this._count = other._count;
            this._value_type = other._value_type;
            this._size = this._count * this.ValueSize;
        }

        /// <summary>
        /// Creates a new descriptor for a chunk of data
        /// </summary>
        /// <param name="ValueType">Data type</param>
        public DataChunkDescriptor(EValueType ValueType) : this (1, ValueType)
        {
        }

        /// <summary>
        /// Creates a new descriptor for a chunk of data
        /// </summary>
        /// <param name="Count">Number of components which comprise a single instance</param>
        /// <param name="ValueType">Data type</param>
        public DataChunkDescriptor(int Count, EValueType ValueType)
        {
            this._count = Count;
            this._value_type = ValueType;
            this._size = this._count * this.ValueSize;
        }
        #endregion


        #region Static Instances

        #region Unsigned Byte
        /// <summary> 1x signed 8-bit integer </summary>
        public static DataChunkDescriptor BYTE1 = new DataChunkDescriptor(1, EValueType.UInt8);
        /// <summary> 2x signed 8-bit integer </summary>
        public static DataChunkDescriptor BYTE2 = new DataChunkDescriptor(2, EValueType.UInt8);
        /// <summary> 3x signed 8-bit integer </summary>
        public static DataChunkDescriptor BYTE3 = new DataChunkDescriptor(3, EValueType.UInt8);
        /// <summary> 4x signed 8-bit integer </summary>
        public static DataChunkDescriptor BYTE4 = new DataChunkDescriptor(4, EValueType.UInt8);
        #endregion

        #region Signed Byte
        /// <summary> 1x unsigned 8-bit integer </summary>
        public static DataChunkDescriptor SBYTE1 = new DataChunkDescriptor(1, EValueType.Int8);
        /// <summary> 2x unsigned 8-bit integer </summary>
        public static DataChunkDescriptor SBYTE2 = new DataChunkDescriptor(2, EValueType.Int8);
        /// <summary> 3x unsigned 8-bit integer </summary>
        public static DataChunkDescriptor SBYTE3 = new DataChunkDescriptor(3, EValueType.Int8);
        /// <summary> 4x unsigned 8-bit integer </summary>
        public static DataChunkDescriptor SBYTE4 = new DataChunkDescriptor(4, EValueType.Int8);
        #endregion

        #region Unsigned Short
        /// <summary> 1x signed 16-bit integer </summary>
        public static DataChunkDescriptor USHORT1 = new DataChunkDescriptor(1, EValueType.UInt16);
        /// <summary> 2x signed 16-bit integer </summary>
        public static DataChunkDescriptor USHORT2 = new DataChunkDescriptor(2, EValueType.UInt16);
        /// <summary> 3x signed 16-bit integer </summary>
        public static DataChunkDescriptor USHORT3 = new DataChunkDescriptor(3, EValueType.UInt16);
        /// <summary> 4x signed 16-bit integer </summary>
        public static DataChunkDescriptor USHORT4 = new DataChunkDescriptor(4, EValueType.UInt16);
        #endregion

        #region Signed Short
        /// <summary> 1x unsigned 16-bit integer </summary>
        public static DataChunkDescriptor SHORT1 = new DataChunkDescriptor(1, EValueType.Int16);
        /// <summary> 2x unsigned 16-bit integer </summary>
        public static DataChunkDescriptor SHORT2 = new DataChunkDescriptor(2, EValueType.Int16);
        /// <summary> 3x unsigned 16-bit integer </summary>
        public static DataChunkDescriptor SHORT3 = new DataChunkDescriptor(3, EValueType.Int16);
        /// <summary> 4x unsigned 16-bit integer </summary>
        public static DataChunkDescriptor SHORT4 = new DataChunkDescriptor(4, EValueType.Int16);
        #endregion

        #region Signed Integer
        /// <summary> 1x signed 32-bit integer </summary>
        public static DataChunkDescriptor INT1 = new DataChunkDescriptor(1, EValueType.Int32);
        /// <summary> 2x signed 32-bit integer </summary>
        public static DataChunkDescriptor INT2 = new DataChunkDescriptor(2, EValueType.Int32);
        /// <summary> 3x signed 32-bit integer </summary>
        public static DataChunkDescriptor INT3 = new DataChunkDescriptor(3, EValueType.Int32);
        /// <summary> 4x signed 32-bit integer </summary>
        public static DataChunkDescriptor INT4 = new DataChunkDescriptor(4, EValueType.Int32);
        #endregion

        #region Unsigned Integer
        /// <summary> 1x unsigned 32-bit integer </summary>
        public static DataChunkDescriptor UINT1 = new DataChunkDescriptor(1, EValueType.UInt32);
        /// <summary> 2x unsigned 32-bit integer </summary>
        public static DataChunkDescriptor UINT2 = new DataChunkDescriptor(2, EValueType.UInt32);
        /// <summary> 3x unsigned 32-bit integer </summary>
        public static DataChunkDescriptor UINT3 = new DataChunkDescriptor(3, EValueType.UInt32);
        /// <summary> 4x unsigned 32-bit integer </summary>
        public static DataChunkDescriptor UINT4 = new DataChunkDescriptor(4, EValueType.UInt32);
        #endregion


        #region Float
        /// <summary> 1x floating point number </summary>
        public static DataChunkDescriptor FLOAT1 = new DataChunkDescriptor(1, EValueType.Float32);
        /// <summary> 2x floating point number </summary>
        public static DataChunkDescriptor FLOAT2 = new DataChunkDescriptor(2, EValueType.Float32);
        /// <summary> 3x floating point number </summary>
        public static DataChunkDescriptor FLOAT3 = new DataChunkDescriptor(3, EValueType.Float32);
        /// <summary> 4x floating point number </summary>
        public static DataChunkDescriptor FLOAT4 = new DataChunkDescriptor(4, EValueType.Float32);
        /// <summary> 6x floating point number </summary>
        public static DataChunkDescriptor FLOAT6 = new DataChunkDescriptor(6, EValueType.Float32);
        /// <summary> 9x floating point number </summary>
        public static DataChunkDescriptor FLOAT9 = new DataChunkDescriptor(9, EValueType.Float32);
        /// <summary> 16x floating point number </summary>
        public static DataChunkDescriptor FLOAT16 = new DataChunkDescriptor(16, EValueType.Float32);
        #endregion

        #region Double
        /// <summary> 1x double floating point number </summary>
        public static DataChunkDescriptor DOUBLE1 = new DataChunkDescriptor(1, EValueType.DOUBLE);
        /// <summary> 2x double floating point number </summary>
        public static DataChunkDescriptor DOUBLE2 = new DataChunkDescriptor(2, EValueType.DOUBLE);
        /// <summary> 3x double floating point number </summary>
        public static DataChunkDescriptor DOUBLE3 = new DataChunkDescriptor(3, EValueType.DOUBLE);
        /// <summary> 4x double floating point number </summary>
        public static DataChunkDescriptor DOUBLE4 = new DataChunkDescriptor(4, EValueType.DOUBLE);
        #endregion

        #endregion
    }
}
