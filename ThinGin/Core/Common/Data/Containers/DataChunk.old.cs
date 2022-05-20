#if false
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Numerics;

using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Data
{
    /* XXX: TODO:
     * - refactor to use ArrayPool and MemoryPool
     * - create compiler define flag for C#9 to utilize function pointers instead of switch blocks
     * - 
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
        private double min;
        private double max;
        #endregion

        #region Accessors
        public double Min => min;
        public double Max => max;

        /// <summary> Length (in bytes) of the data this chunk contains. </summary>
        public int Length => Data.Length;

        /// <summary> The value type of components represented by the bytes of the data chunk </summary>
        //public EValueType Type => Layout.ValueType;

        /// <summary> Number of components represented by the data in this chunk. </summary>
        public int Count => Layout.Count;
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
            min = (min > value) ? value : min;
            max = (max < value) ? value : max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _adjust_min_max(double value)
        {
            min = (min > value) ? value : min;
            max = (max < value) ? value : max;
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

        #region Numeric Structures
        public DataChunk(Vector2 value)
        {
            Layout = DataChunkDescriptor.FLOAT2;
            unsafe
            {
                float* dataPtr = &value.X;
                Data = GetBytes(dataPtr, 2);

                for (int i = 0; i < Layout.Count; i++)
                {
                    _adjust_min_max(dataPtr[i]);
                }
            }
        }

        public DataChunk(Vector3 value)
        {
            Layout = DataChunkDescriptor.FLOAT3;
            unsafe
            {
                float* dataPtr = &value.X;
                Data = GetBytes(dataPtr, 3);

                for (int i = 0; i < Layout.Count; i++)
                {
                    _adjust_min_max(dataPtr[i]);
                }
            }
        }

        public DataChunk(Vector4 value)
        {
            Layout = DataChunkDescriptor.FLOAT4;
            unsafe
            {
                float* dataPtr = &value.X;
                Data = GetBytes(dataPtr, 4);

                for (int i = 0; i < Layout.Count; i++)
                {
                    _adjust_min_max(dataPtr[i]);
                }
            }
        }


        public DataChunk(Matrix3x2 value)
        {
            Layout = DataChunkDescriptor.FLOAT6;
            unsafe
            {
                float* dataPtr = &value.M11;
                Data = GetBytes(dataPtr, 6);
            }
        }

        public DataChunk(Matrix4x4 value)
        {
            Layout = DataChunkDescriptor.FLOAT16;

            Data = new byte[Layout.Size];
            var sBuf = new Span<byte>(Data);

            BitConverter.TryWriteBytes(sBuf, value.M11);
            BitConverter.TryWriteBytes(sBuf, value.M12);
            BitConverter.TryWriteBytes(sBuf, value.M13);
            BitConverter.TryWriteBytes(sBuf, value.M14);

            BitConverter.TryWriteBytes(sBuf, value.M21);
            BitConverter.TryWriteBytes(sBuf, value.M22);
            BitConverter.TryWriteBytes(sBuf, value.M23);
            BitConverter.TryWriteBytes(sBuf, value.M24);
                
            BitConverter.TryWriteBytes(sBuf, value.M31);
            BitConverter.TryWriteBytes(sBuf, value.M32);
            BitConverter.TryWriteBytes(sBuf, value.M33);
            BitConverter.TryWriteBytes(sBuf, value.M34);
                
            BitConverter.TryWriteBytes(sBuf, value.M41);
            BitConverter.TryWriteBytes(sBuf, value.M42);
            BitConverter.TryWriteBytes(sBuf, value.M43);
            BitConverter.TryWriteBytes(sBuf, value.M44);
        }
        #endregion

        #region Byte

        public DataChunk(byte[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.BYTE1,
                2 => DataChunkDescriptor.BYTE2,
                3 => DataChunkDescriptor.BYTE3,
                4 => DataChunkDescriptor.BYTE4,
                _ => new DataChunkDescriptor(data.Length, EValueType.BYTE),
            };

            Data = data;

            min = max = Data[0];
            for (int i = 0; i < Data.Length; i++)
            {
                min = (min > Data[i]) ? Data[i] : min;
                max = (max < Data[i]) ? Data[i] : max;
            }
        }

        public DataChunk(byte X, byte Y)
        {
            Layout = DataChunkDescriptor.BYTE2;
            Data = new byte[] { X, Y };

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(byte X, byte Y, byte Z)
        {
            Layout = DataChunkDescriptor.BYTE3;
            Data = new byte[] { X, Y, Z };

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(byte X, byte Y, byte Z, byte W)
        {
            Layout = DataChunkDescriptor.BYTE4;
            Data = new byte[] { X, Y, Z, W };

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #region SByte

        public DataChunk(sbyte[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.SBYTE1,
                2 => DataChunkDescriptor.SBYTE2,
                3 => DataChunkDescriptor.SBYTE3,
                4 => DataChunkDescriptor.SBYTE4,
                _ => new DataChunkDescriptor(data.Length, EValueType.SBYTE),
            };

            ReadOnlySpan<sbyte> from = data;
            ReadOnlySpan<byte> to = MemoryMarshal.Cast<sbyte, byte>(from);
            Data = to.ToArray();

            min = max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                min = (min > data[i]) ? data[i] : min;
                max = (max < data[i]) ? data[i] : max;
            }
        }

        public DataChunk(sbyte X, sbyte Y)
        {
            Layout = DataChunkDescriptor.SBYTE2;
            Data = unchecked(new byte[] { (byte)X, (byte)Y });

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(sbyte X, sbyte Y, sbyte Z) 
        {
            Layout = DataChunkDescriptor.SBYTE3;
            Data = unchecked(new byte[] { (byte)X, (byte)Y, (byte)Z });

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(sbyte X, sbyte Y, sbyte Z, sbyte W) 
        {
            Layout = DataChunkDescriptor.SBYTE4;
            Data = unchecked(new byte[] { (byte)X, (byte)Y, (byte)Z, (byte)W });

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #region Float

        public DataChunk(float[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.FLOAT1,
                2 => DataChunkDescriptor.FLOAT2,
                3 => DataChunkDescriptor.FLOAT3,
                4 => DataChunkDescriptor.FLOAT4,
                6 => DataChunkDescriptor.FLOAT6,
                9 => DataChunkDescriptor.FLOAT9,
                16 => DataChunkDescriptor.FLOAT16,
                _ => new DataChunkDescriptor(data.Length, EValueType.FLOAT),
            };

            Data = GetBytes(data);

            min = max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                _adjust_min_max(data[i]);
            }
        }

        public DataChunk(float X, float Y) 
        {
            Layout = DataChunkDescriptor.FLOAT2;
            Data = GetBytes(X, Y);

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(float X, float Y, float Z) 
        {
            Layout = DataChunkDescriptor.FLOAT3;
            Data = GetBytes(X, Y, Z);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(float X, float Y, float Z, float W) 
        {
            Layout = DataChunkDescriptor.FLOAT4;
            Data = GetBytes(X, Y, Z, W);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #region Double

        public DataChunk(double[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.DOUBLE1,
                2 => DataChunkDescriptor.DOUBLE2,
                3 => DataChunkDescriptor.DOUBLE3,
                4 => DataChunkDescriptor.DOUBLE4,
                _ => new DataChunkDescriptor(data.Length, EValueType.DOUBLE),
            };

            Data = GetBytes(data);

            min = max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                _adjust_min_max(data[i]);
            }
        }

        public DataChunk(double X, double Y) 
        {
            Layout = DataChunkDescriptor.DOUBLE2;
            Data = GetBytes(X, Y);

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(double X, double Y, double Z) 
        {
            Layout = DataChunkDescriptor.DOUBLE3;
            Data = GetBytes(X, Y, Z);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(double X, double Y, double Z, double W) 
        {
            Layout = DataChunkDescriptor.DOUBLE4;
            Data = GetBytes(X, Y, Z, W);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #region Short

        public DataChunk(short[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.SHORT1,
                2 => DataChunkDescriptor.SHORT2,
                3 => DataChunkDescriptor.SHORT3,
                4 => DataChunkDescriptor.SHORT4,
                _ => new DataChunkDescriptor(data.Length, EValueType.SHORT),
            };

            Data = GetBytes(data);

            min = max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                _adjust_min_max(data[i]);
            }
        }

        public DataChunk(short X, short Y) 
        {
            Layout = DataChunkDescriptor.SHORT2;
            Data = GetBytes(X, Y);

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(short X, short Y, short Z) 
        {
            Layout = DataChunkDescriptor.SHORT3;
            Data = GetBytes(X, Y, Z);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(short X, short Y, short Z, short W) 
        {
            Layout = DataChunkDescriptor.SHORT4;
            Data = GetBytes(X, Y, Z, W);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #region Unsigned Short

        public DataChunk(ushort[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.USHORT1,
                2 => DataChunkDescriptor.USHORT2,
                3 => DataChunkDescriptor.USHORT3,
                4 => DataChunkDescriptor.USHORT4,
                _ => new DataChunkDescriptor(data.Length, EValueType.USHORT),
            };

            Data = GetBytes(data);

            min = max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                _adjust_min_max(data[i]);
            }
        }

        public DataChunk(ushort X, ushort Y) 
        {
            Layout = DataChunkDescriptor.USHORT2;
            Data = GetBytes(X, Y);

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(ushort X, ushort Y, ushort Z) 
        {
            Layout = DataChunkDescriptor.USHORT3;
            Data = GetBytes(X, Y, Z);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(ushort X, ushort Y, ushort Z, ushort W) 
        {
            Layout = DataChunkDescriptor.USHORT4;
            Data = GetBytes(X, Y, Z, W);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #region Int

        public DataChunk(int[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.INT1,
                2 => DataChunkDescriptor.INT2,
                3 => DataChunkDescriptor.INT3,
                4 => DataChunkDescriptor.INT4,
                _ => new DataChunkDescriptor(data.Length, EValueType.INT),
            };

            Data = GetBytes(data);

            min = max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                _adjust_min_max(data[i]);
            }
        }

        public DataChunk(int X, int Y) 
        {
            Layout = DataChunkDescriptor.INT2;
            Data = GetBytes(X, Y);

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(int X, int Y, int Z) 
        {
            Layout = DataChunkDescriptor.INT3;
            Data = GetBytes(X, Y, Z);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(int X, int Y, int Z, int W) 
        {
            Layout = DataChunkDescriptor.INT4;
            Data = GetBytes(X, Y, Z, W);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #region Unsigned Int

        public DataChunk(uint[] data)
        {
            Layout = data.Length switch
            {
                1 => DataChunkDescriptor.UINT1,
                2 => DataChunkDescriptor.UINT2,
                3 => DataChunkDescriptor.UINT3,
                4 => DataChunkDescriptor.UINT4,
                _ => new DataChunkDescriptor(data.Length, EValueType.UINT),
            };

            Data = GetBytes(data);

            min = max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                _adjust_min_max(data[i]);
            }
        }

        public DataChunk(uint X, uint Y) 
        {
            Layout = DataChunkDescriptor.UINT2;
            Data = GetBytes(X, Y);

            min = max = X;
            _adjust_min_max(Y);
        }

        public DataChunk(uint X, uint Y, uint Z) 
        {
            Layout = DataChunkDescriptor.UINT3;
            Data = GetBytes(X, Y, Z);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
        }

        public DataChunk(uint X, uint Y, uint Z, uint W) 
        {
            Layout = DataChunkDescriptor.UINT4;
            Data = GetBytes(X, Y, Z, W);

            min = max = X;
            _adjust_min_max(Y);
            _adjust_min_max(Z);
            _adjust_min_max(W);
        }
        #endregion

        #endregion

        #region Encoding
        #region FLOAT
        public static unsafe byte[] GetBytes(float* dataPtr, int Count)
        {
            const int SIZE = sizeof(float);
            var bufferLen = SIZE * Count;
            var buffer = new byte[bufferLen];

            fixed (void* bufferPtr = &buffer[0])
            {
                Buffer.MemoryCopy(dataPtr, bufferPtr, bufferLen, bufferLen);
            }

            return buffer;
        }

        public static unsafe byte[] GetBytes(float[] Data)
        {
            const int SIZE = sizeof(float);
            var buffer = new byte[SIZE * Data.Length];

            fixed (void* bufferPtr = &buffer[0])
            {
                fixed (void* dataPtr = &Data[0])
                {
                    Buffer.MemoryCopy(dataPtr, bufferPtr, buffer.LongLength, Data.LongLength * SIZE);
                }
            }

            return buffer;
        }

        public static unsafe byte[] GetBytes(float X, float Y)
        {
            const int SIZE = sizeof(float);
            var buffer = new byte[SIZE * 2];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(float X, float Y, float Z)
        {
            const int SIZE = sizeof(float);
            const int SIZE2 = SIZE * 2;

            var buffer = new byte[SIZE * 3];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(float X, float Y, float Z, float W)
        {
            const int SIZE = sizeof(float);
            const int SIZE2 = SIZE * 2;
            const int SIZE3 = SIZE * 3;
            const int SIZE4 = SIZE * 4;

            var buffer = new byte[SIZE4];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);
            var b3 = BitConverter.GetBytes(W);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);
            Buffer.BlockCopy(b3, 0, buffer, SIZE3, SIZE);

            return buffer;
        }
        #endregion

        #region DOUBLE

        public static unsafe byte[] GetBytes(double[] Data)
        {
            const int SIZE = sizeof(double);
            var buffer = new byte[SIZE * Data.Length];

            fixed (void* bufferPtr = &buffer[0])
            {
                fixed (void* dataPtr = &Data[0])
                {
                    Buffer.MemoryCopy(dataPtr, bufferPtr, buffer.LongLength, Data.LongLength * SIZE);
                }
            }

            return buffer;
        }

        public static unsafe byte[] GetBytes(double X, double Y)
        {
            const int SIZE = sizeof(double);
            var buffer = new byte[SIZE * 2];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(double X, double Y, double Z)
        {
            const int SIZE = sizeof(double);
            const int SIZE2 = SIZE * 2;

            var buffer = new byte[SIZE * 3];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(double X, double Y, double Z, double W)
        {
            const int SIZE = sizeof(double);
            const int SIZE2 = SIZE * 2;
            const int SIZE3 = SIZE * 3;
            const int SIZE4 = SIZE * 4;

            var buffer = new byte[SIZE4];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);
            var b3 = BitConverter.GetBytes(W);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);
            Buffer.BlockCopy(b3, 0, buffer, SIZE3, SIZE);

            return buffer;
        }
        #endregion

        #region SHORT

        public static unsafe byte[] GetBytes(short[] Data)
        {
            const int SIZE = sizeof(short);
            var buffer = new byte[SIZE * Data.Length];

            fixed (void* bufferPtr = &buffer[0])
            {
                fixed (void* dataPtr = &Data[0])
                {
                    Buffer.MemoryCopy(dataPtr, bufferPtr, buffer.LongLength, Data.LongLength * SIZE);
                }
            }

            return buffer;
        }

        public static unsafe byte[] GetBytes(short X, short Y)
        {
            const int SIZE = sizeof(short);
            var buffer = new byte[SIZE * 2];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(short X, short Y, short Z)
        {
            const int SIZE = sizeof(short);
            const int SIZE2 = SIZE * 2;

            var buffer = new byte[SIZE * 3];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(short X, short Y, short Z, short W)
        {
            const int SIZE = sizeof(short);
            const int SIZE2 = SIZE * 2;
            const int SIZE3 = SIZE * 3;
            const int SIZE4 = SIZE * 4;

            var buffer = new byte[SIZE4];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);
            var b3 = BitConverter.GetBytes(W);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);
            Buffer.BlockCopy(b3, 0, buffer, SIZE3, SIZE);

            return buffer;
        }
        #endregion

        #region USHORT

        public static unsafe byte[] GetBytes(ushort[] Data)
        {
            const int SIZE = sizeof(ushort);
            var buffer = new byte[SIZE * Data.Length];

            fixed (void* bufferPtr = &buffer[0])
            {
                fixed (void* dataPtr = &Data[0])
                {
                    Buffer.MemoryCopy(dataPtr, bufferPtr, buffer.LongLength, Data.LongLength * SIZE);
                }
            }

            return buffer;
        }

        public static unsafe byte[] GetBytes(ushort X, ushort Y)
        {
            const int SIZE = sizeof(ushort);
            var buffer = new byte[SIZE * 2];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(ushort X, ushort Y, ushort Z)
        {
            const int SIZE = sizeof(ushort);
            const int SIZE2 = SIZE * 2;

            var buffer = new byte[SIZE * 3];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(ushort X, ushort Y, ushort Z, ushort W)
        {
            const int SIZE = sizeof(ushort);
            const int SIZE2 = SIZE * 2;
            const int SIZE3 = SIZE * 3;
            const int SIZE4 = SIZE * 4;

            var buffer = new byte[SIZE4];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);
            var b3 = BitConverter.GetBytes(W);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);
            Buffer.BlockCopy(b3, 0, buffer, SIZE3, SIZE);

            return buffer;
        }
        #endregion


        #region INT

        public static unsafe byte[] GetBytes(int[] Data)
        {
            const int SIZE = sizeof(int);
            var buffer = new byte[SIZE * Data.Length];

            fixed (void* bufferPtr = &buffer[0])
            {
                fixed (void* dataPtr = &Data[0])
                {
                    Buffer.MemoryCopy(dataPtr, bufferPtr, buffer.LongLength, Data.LongLength * SIZE);
                }
            }

            return buffer;
        }

        public static unsafe byte[] GetBytes(int X, int Y)
        {
            const int SIZE = sizeof(int);
            var buffer = new byte[SIZE * 2];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(int X, int Y, int Z)
        {
            const int SIZE = sizeof(int);
            const int SIZE2 = SIZE * 2;

            var buffer = new byte[SIZE * 3];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(int X, int Y, int Z, int W)
        {
            const int SIZE = sizeof(int);
            const int SIZE2 = SIZE * 2;
            const int SIZE3 = SIZE * 3;
            const int SIZE4 = SIZE * 4;

            var buffer = new byte[SIZE4];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);
            var b3 = BitConverter.GetBytes(W);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);
            Buffer.BlockCopy(b3, 0, buffer, SIZE3, SIZE);

            return buffer;
        }
        #endregion

        #region UINT

        public static unsafe byte[] GetBytes(uint[] Data)
        {
            const int SIZE = sizeof(uint);
            var buffer = new byte[SIZE * Data.Length];

            fixed (void* bufferPtr = &buffer[0])
            {
                fixed (void* dataPtr = &Data[0])
                {
                    Buffer.MemoryCopy(dataPtr, bufferPtr, buffer.LongLength, Data.LongLength * SIZE);
                }
            }

            return buffer;
        }

        public static unsafe byte[] GetBytes(uint X, uint Y)
        {
            const int SIZE = sizeof(uint);
            var buffer = new byte[SIZE * 2];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(uint X, uint Y, uint Z)
        {
            const int SIZE = sizeof(uint);
            const int SIZE2 = SIZE * 2;

            var buffer = new byte[SIZE * 3];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);

            return buffer;
        }

        public static unsafe byte[] GetBytes(uint X, uint Y, uint Z, uint W)
        {
            const int SIZE = sizeof(uint);
            const int SIZE2 = SIZE * 2;
            const int SIZE3 = SIZE * 3;
            const int SIZE4 = SIZE * 4;

            var buffer = new byte[SIZE4];

            var b0 = BitConverter.GetBytes(X);
            var b1 = BitConverter.GetBytes(Y);
            var b2 = BitConverter.GetBytes(Z);
            var b3 = BitConverter.GetBytes(W);

            Buffer.BlockCopy(b0, 0, buffer, 0, SIZE);
            Buffer.BlockCopy(b1, 0, buffer, SIZE, SIZE);
            Buffer.BlockCopy(b2, 0, buffer, SIZE2, SIZE);
            Buffer.BlockCopy(b3, 0, buffer, SIZE3, SIZE);

            return buffer;
        }
        #endregion
        #endregion

        #region Referencing
        public unsafe byte* AsByte() { fixed (void* ptr = &Data[0]) { return (byte*)ptr; } }
        public unsafe sbyte* AsSByte() { fixed (void* ptr = &Data[0]) { return (sbyte*)ptr; } }

        public unsafe short* AsShort() { fixed (void* ptr = &Data[0]) { return (short*)ptr; } }
        public unsafe ushort* AsUShort() { fixed (void* ptr = &Data[0]) { return (ushort*)ptr; } }

        public unsafe int* AsInt() { fixed (void* ptr = &Data[0]) { return (int*)ptr; } }
        public unsafe uint* AsUInt() { fixed (void* ptr = &Data[0]) { return (uint*)ptr; } }

        public unsafe float* AsFloat() { fixed (void* ptr = &Data[0]) { return (float*)ptr; } }
        public unsafe double* AsDouble() { fixed (void* ptr = &Data[0]) { return (double*)ptr; } }
        #endregion

        #region Converting


        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public double[] ToDouble()
        {
            var ret = new double[Count];

            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (byte* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (byte* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return ret;
        }

        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public float[] ToFloat()
        {
            var ret = new float[Count];
            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (float)((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return ret;
        }

        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public byte[] ToByte()
        {
            var ret = new byte[Count];
            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (byte)((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (byte)((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (byte)((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (byte)((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (byte)((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (byte)((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (byte)((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }
            return ret;
        }

        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public sbyte[] ToSByte()
        {
            var ret = new sbyte[Count];
            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (sbyte)((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return ret;
        }

        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public short[] ToShort()
        {
            var ret = new short[Count];
            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (short)((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return ret;
        }

        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public ushort[] ToUShort()
        {
            var ret = new ushort[Count];
            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (ushort)((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return ret;
        }

        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public int[] ToInt()
        {
            var ret = new int[Count];
            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (int)((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (int)((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (int)((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return ret;
        }

        /// <summary>
        /// Casts the underlying array of values to an array of the specified value type, allocating data in the process.
        /// </summary>
        public uint[] ToUInt()
        {
            var ret = new uint[Count];
            unsafe
            {
                switch (Type)
                {
                    case EValueType.BYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((byte*)pIn)[i];
                        break;
                    case EValueType.SBYTE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (uint)((sbyte*)pIn)[i];
                        break;
                    case EValueType.SHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (uint)((short*)pIn)[i];
                        break;
                    case EValueType.USHORT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((ushort*)pIn)[i];
                        break;
                    case EValueType.FLOAT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (uint)((float*)pIn)[i];
                        break;
                    case EValueType.DOUBLE:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (uint)((double*)pIn)[i];
                        break;
                    case EValueType.INT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = (uint)((int*)pIn)[i];
                        break;
                    case EValueType.UINT:
                        fixed (void* pIn = &Data[0]) for (int i = 0; i < Count; i++) ret[i] = ((uint*)pIn)[i];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return ret;
        }

        #endregion

        #region Getters
        public unsafe byte GetByte(int index) { fixed (void* ptr = &Data[index * sizeof(byte)]) { return *(byte*)ptr; } }
        public unsafe sbyte GetSByte(int index) { fixed (void* ptr = &Data[index * sizeof(sbyte)]) { return *(sbyte*)ptr; } }

        public unsafe short GetShort(int index) { fixed (void* ptr = &Data[index * sizeof(short)]) { return *(short*)ptr; } }
        public unsafe ushort GetUShort(int index) { fixed (void* ptr = &Data[index * sizeof(ushort)]) { return *(ushort*)ptr; } }

        public unsafe int GetInt(int index) { fixed (void* ptr = &Data[index * sizeof(int)]) { return *(int*)ptr; } }
        public unsafe uint GetUInt(int index) { fixed (void* ptr = &Data[index * sizeof(uint)]) { return *(uint*)ptr; } }

        public unsafe float GetFloat(int index) { fixed (void* ptr = &Data[index * sizeof(float)]) { return *(float*)ptr; } }
        public unsafe double GetDouble(int index) { fixed (void* ptr = &Data[index * sizeof(double)]) { return *(double*)ptr; } }
        #endregion

        #region IComparable
        public unsafe int CompareTo(DataChunk other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Type != other.Type)
            {
                throw new ArgumentException($"Value type mismatch!");
            }

            int Cnt = Math.Min(Count, other.Count);

            switch (Type)
            {
                case EValueType.BYTE:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetByte(i);
                            var right = other.GetByte(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                case EValueType.SBYTE:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetSByte(i);
                            var right = other.GetSByte(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                case EValueType.SHORT:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetShort(i);
                            var right = other.GetShort(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                case EValueType.USHORT:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetUShort(i);
                            var right = other.GetUShort(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                case EValueType.INT:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetInt(i);
                            var right = other.GetInt(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                case EValueType.UINT:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetUInt(i);
                            var right = other.GetUInt(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                //case EValueType.FLOAT_HALF:
                //    break;
                case EValueType.FLOAT:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetFloat(i);
                            var right = other.GetFloat(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                case EValueType.DOUBLE:
                    {
                        for (int i = 0; i < Cnt; i++)
                        {
                            var left = GetDouble(i);
                            var right = other.GetDouble(i);

                            var c = left.CompareTo(right);
                            if (c != 0) return c;
                        }
                        return 0;
                    }
                default:
                    {
                        throw new NotImplementedException(Type.ToString());
                    }
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            switch (Type)
            {
                //case EValueType.FLOAT_HALF:
                case EValueType.FLOAT:
                case EValueType.DOUBLE:
                    {
                        return $"{Type.ToString()}<{string.Join(", ", ToDouble())}>";
                    }
                default:
                    {
                        return $"{Type.ToString()}<{string.Join(", ", ToInt())}>";
                    }
            }
        }
        #endregion
    }
}
#endif