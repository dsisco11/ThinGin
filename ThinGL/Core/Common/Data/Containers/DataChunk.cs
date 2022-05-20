#define USE_OLD_DATA_CHUNK
using System;
using System.Runtime.CompilerServices;
using System.Numerics;

using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Data
{
    /* XXX: TODO:
     * - refactor to use ArrayPool and MemoryPool
     * - create compiler define flag for C#9 to utilize function pointers instead of switch blocks
     * - Add platform agnostic text templating system for compilation process (See: https://github.com/mono/t4) (See: https://stackoverflow.com/questions/47691299/texttemplating-target-in-a-net-core-project)
     *  - OR: Move to using the new .net code generator objects
     */
    public partial class DataChunk : IComparable<DataChunk>
    {
        #region Values

        /// <summary> The value type of components represented by the bytes of the data chunk </summary>
        public readonly EValueType Type;// Note: theoretically by explicitly storing an immutable copy of our value type enum value, the runtime can better optimize our switch blocks...
        #endregion

        #region Properties
        public readonly byte[] Data;
        public readonly DataChunkDescriptor Layout = null;
        private int min;
        private int max;
        #endregion

        #region Accessors
        public int Min => min;
        public int Max => max;

        /// <summary> Length (in bytes) of the data this chunk contains. </summary>
        public int Length => Data.Length;

        /// <summary> Number of components represented by the data in this chunk. </summary>
        public int Count => Layout.Count;
        public ReadOnlySpan<byte> AsSpan() => new ReadOnlySpan<byte>(Data);
        #endregion

        #region MinMax

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _adjust_min_max(int value)
        {
            min = (min > value) ? value : min;
            max = (max < value) ? value : max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _adjust_min_max(float value)
        {
            min = (min > value) ? (int)value : min;
            max = (max < value) ? (int)value : max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _adjust_min_max(double value)
        {
            min = (min > value) ? (int)value : min;
            max = (max < value) ? (int)value : max;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a deep copy of the given data chunk
        /// </summary>
        /// <param name="other"></param>
        public DataChunk(DataChunk other)
        {
            Layout = other.Layout;
            min = other.min;
            max = other.max;

            Data = new byte[other.Data.Length];
            Buffer.BlockCopy(other.Data, 0, Data, 0, Data.Length);
        }

        public DataChunk(Vector2 value)
        {
            Layout = DataChunkDescriptor.FLOAT2;
            Type = Layout.ValueType;
            Data = new byte[Layout.Size];
            var sBuf = new Span<byte>(Data);

            BitConverter.TryWriteBytes(sBuf, value.X);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 1), value.Y);
        }

        public DataChunk(Vector3 value)
        {
            Layout = DataChunkDescriptor.FLOAT3;
            Type = Layout.ValueType;
            Data = new byte[Layout.Size];
            var sBuf = new Span<byte>(Data);

            BitConverter.TryWriteBytes(sBuf, value.X);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 1), value.Y);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 2), value.Z);
        }

        public DataChunk(Vector4 value)
        {
            Layout = DataChunkDescriptor.FLOAT4;
            Type = Layout.ValueType;
            Data = new byte[Layout.Size];
            var sBuf = new Span<byte>(Data);

            BitConverter.TryWriteBytes(sBuf, value.X);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 1), value.Y);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 2), value.Z);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 3), value.W);
        }


        public DataChunk(Matrix3x2 value)
        {
            Layout = DataChunkDescriptor.FLOAT6;
            Type = Layout.ValueType;
            Data = new byte[Layout.Size];
            var sBuf = new Span<byte>(Data);

            BitConverter.TryWriteBytes(sBuf, value.M11);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 1), value.M12);

            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 2), value.M21);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 3), value.M22);

            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 4), value.M31);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 5), value.M32);
        }

        public DataChunk(Matrix4x4 value)
        {
            Layout = DataChunkDescriptor.FLOAT16;
            Type = Layout.ValueType;
            Data = new byte[Layout.Size];
            var sBuf = new Span<byte>(Data);

            BitConverter.TryWriteBytes(sBuf, value.M11);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 1), value.M12);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 2), value.M13);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 3), value.M14);

            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 4), value.M21);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 5), value.M22);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 6), value.M23);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 7), value.M24);

            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 8), value.M31);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 9), value.M32);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 10), value.M33);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 11), value.M34);

            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 12), value.M41);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 13), value.M42);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 14), value.M43);
            BitConverter.TryWriteBytes(sBuf.Slice(SIZE_FLOAT * 15), value.M44);
        }
        #endregion

        #region Referencing
        public unsafe byte* AsByte() { fixed (byte* ptr = AsSpan()) { return (byte*)ptr; } }
        public unsafe sbyte* AsSByte() { fixed (byte* ptr = AsSpan()) { return (sbyte*)ptr; } }

        public unsafe short* AsShort() { fixed (byte* ptr = AsSpan()) { return (short*)ptr; } }
        public unsafe ushort* AsUShort() { fixed (byte* ptr = AsSpan()) { return (ushort*)ptr; } }

        public unsafe int* AsInt() { fixed (byte* ptr = AsSpan()) { return (int*)ptr; } }
        public unsafe uint* AsUInt() { fixed (byte* ptr = AsSpan()) { return (uint*)ptr; } }

        public unsafe float* AsFloat() { fixed (byte* ptr = AsSpan()) { return (float*)ptr; } }
        public unsafe double* AsDouble() { fixed (byte* ptr = AsSpan()) { return (double*)ptr; } }
        #endregion

        #region ToString
        private string _get_ptr_value_string()
        {
            var sb = new System.Text.StringBuilder();
            unsafe
            {
                switch (Type)
                {
                    case EValueType.UInt8:
                        {
                            byte* dptr = this.AsByte();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    case EValueType.Int8:
                        {
                            sbyte* dptr = this.AsSByte();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    case EValueType.Int16:
                        {
                            short* dptr = this.AsShort();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    case EValueType.UInt16:
                        {
                            ushort* dptr = this.AsUShort();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    case EValueType.Int32:
                        {
                            int* dptr = this.AsInt();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    case EValueType.UInt32:
                        {
                            uint* dptr = this.AsUInt();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    //case EValueType.FLOAT_HALF:
                    case EValueType.Float32:
                        {
                            float* dptr = this.AsFloat();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    case EValueType.DOUBLE:
                        {
                            double* dptr = this.AsDouble();
                            for (int i = 0; i < this.Count; i++)
                            {
                                string pfx = (i > 0) ? ", " : "";
                                sb.Append($"{pfx}{dptr[i]}");
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            switch (Type)
            {
                //case EValueType.FLOAT_HALF:
                case EValueType.Float32:
                case EValueType.DOUBLE:
                    {
                        return $"{Type}<{string.Join(", ", ToDouble())}>";
                    }
                default:
                    {
                        return $"{Type}<{string.Join(", ", ToInt())}>";
                    }
            }
        }

        public string ToStringFromPtr() => $"{Type}<{_get_ptr_value_string()}>";
        #endregion
    }
}
