using System;

using ThinGin.Core.Common.Data;

using Xunit;

namespace UnitTests
{
    public class DataChunkTests
    {
        #region Buffer Integrity
        [Theory]
        [InlineData(new byte[] { 0, 0, 0, 0, 0 })]
        [InlineData(new byte[] { 1, 1, 1, 1, 1 })]
        [InlineData(new byte[] { 0, 2, 4, 8, 16 })]
        [InlineData(new byte[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_Byte(byte[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<byte>(data);

            Assert.True( dSpan.SequenceEqual(cSpan) );
        }


        [Theory]
        [InlineData(new sbyte[] { 0, 0, 0, 0, 0 })]
        [InlineData(new sbyte[] { 1, 1, 1, 1, 1 })]
        [InlineData(new sbyte[] { 0, 2, 4, 8, 16 })]
        [InlineData(new sbyte[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_SByte(sbyte[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<sbyte>(data);

            unsafe
            {
                fixed (void* cPtr = cSpan)
                {
                    fixed (void* dPtr = dSpan)
                    {
                        var c = (sbyte*)cPtr;
                        var d = (sbyte*)dPtr;

                        for (var i = 0; i < data.Length; i++)
                        {
                            Assert.True(c[i] == d[i]);
                        }
                    }
                }
            }
        }


        [Theory]
        [InlineData(new short[] { 0, 0, 0, 0, 0 })]
        [InlineData(new short[] { 1, 1, 1, 1, 1 })]
        [InlineData(new short[] { 0, 2, 4, 8, 16 })]
        [InlineData(new short[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_Short(short[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<short>(data);

            unsafe
            {
                fixed (void* cPtr = cSpan)
                {
                    fixed (void* dPtr = dSpan)
                    {
                        var c = (short*)cPtr;
                        var d = (short*)dPtr;

                        for (var i = 0; i < data.Length; i++)
                        {
                            Assert.True(c[i] == d[i]);
                        }
                    }
                }
            }
        }


        [Theory]
        [InlineData(new ushort[] { 0, 0, 0, 0, 0 })]
        [InlineData(new ushort[] { 1, 1, 1, 1, 1 })]
        [InlineData(new ushort[] { 0, 2, 4, 8, 16 })]
        [InlineData(new ushort[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_UShort(ushort[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<ushort>(data);

            unsafe
            {
                fixed (void* cPtr = cSpan)
                {
                    fixed (void* dPtr = dSpan)
                    {
                        var c = (ushort*)cPtr;
                        var d = (ushort*)dPtr;

                        for (var i = 0; i < data.Length; i++)
                        {
                            Assert.True(c[i] == d[i]);
                        }
                    }
                }
            }
        }


        [Theory]
        [InlineData(new int[] { 0, 0, 0, 0, 0 })]
        [InlineData(new int[] { 1, 1, 1, 1, 1 })]
        [InlineData(new int[] { 0, 2, 4, 8, 16 })]
        [InlineData(new int[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_Int(int[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<int>(data);

            unsafe
            {
                fixed (void* cPtr = cSpan)
                {
                    fixed (void* dPtr = dSpan)
                    {
                        var c = (int*)cPtr;
                        var d = (int*)dPtr;

                        for (var i = 0; i < data.Length; i++)
                        {
                            Assert.True(c[i] == d[i]);
                        }
                    }
                }
            }
        }


        [Theory]
        [InlineData(new uint[] { 0, 0, 0, 0, 0 })]
        [InlineData(new uint[] { 1, 1, 1, 1, 1 })]
        [InlineData(new uint[] { 0, 2, 4, 8, 16 })]
        [InlineData(new uint[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_UInt(uint[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<uint>(data);

            unsafe
            {
                fixed (void* cPtr = cSpan)
                {
                    fixed (void* dPtr = dSpan)
                    {
                        var c = (uint*)cPtr;
                        var d = (uint*)dPtr;

                        for (var i = 0; i < data.Length; i++)
                        {
                            Assert.True(c[i] == d[i]);
                        }
                    }
                }
            }
        }


        [Theory]
        [InlineData(new float[] { 0, 0, 0, 0, 0 })]
        [InlineData(new float[] { 1, 1, 1, 1, 1 })]
        [InlineData(new float[] { 0, 2, 4, 8, 16 })]
        [InlineData(new float[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_Float(float[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<float>(data);

            unsafe
            {
                fixed (void* cPtr = cSpan)
                {
                    fixed (void* dPtr = dSpan)
                    {
                        var c = (float*)cPtr;
                        var d = (float*)dPtr;

                        for (var i = 0; i < data.Length; i++)
                        {
                            Assert.True(c[i] == d[i]);
                        }
                    }
                }
            }
        }


        [Theory]
        [InlineData(new double[] { 0, 0, 0, 0, 0 })]
        [InlineData(new double[] { 1, 1, 1, 1, 1 })]
        [InlineData(new double[] { 0, 2, 4, 8, 16 })]
        [InlineData(new double[] { 56, 87, 4, 0, 125 })]
        public void Buffer_IntegrityTest_Double(double[] data)
        {
            var chunk = new DataChunk(data);
            var cSpan = chunk.AsSpan();
            var dSpan = new ReadOnlySpan<double>(data);

            unsafe
            {
                fixed (void* cPtr = cSpan)
                {
                    fixed (void* dPtr = dSpan)
                    {
                        var c = (double*)cPtr;
                        var d = (double*)dPtr;

                        for (var i = 0; i < data.Length; i++)
                        {
                            Assert.True(c[i] == d[i]);
                        }
                    }
                }
            }
        }
        #endregion


        #region Copy Integrity
        [Theory]
        [InlineData(new byte[] { 0, 0, 0, 0, 0 })]
        [InlineData(new byte[] { 1, 1, 1, 1, 1 })]
        [InlineData(new byte[] { 0, 2, 4, 8, 16 })]
        [InlineData(new byte[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_Byte(byte[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToByte();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }


        [Theory]
        [InlineData(new sbyte[] { 0, 0, 0, 0, 0 })]
        [InlineData(new sbyte[] { 1, 1, 1, 1, 1 })]
        [InlineData(new sbyte[] { 0, 2, 4, 8, 16 })]
        [InlineData(new sbyte[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_SByte(sbyte[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToSByte();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }


        [Theory]
        [InlineData(new short[] { 0, 0, 0, 0, 0 })]
        [InlineData(new short[] { 1, 1, 1, 1, 1 })]
        [InlineData(new short[] { 0, 2, 4, 8, 16 })]
        [InlineData(new short[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_Short(short[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToShort();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }


        [Theory]
        [InlineData(new ushort[] { 0, 0, 0, 0, 0 })]
        [InlineData(new ushort[] { 1, 1, 1, 1, 1 })]
        [InlineData(new ushort[] { 0, 2, 4, 8, 16 })]
        [InlineData(new ushort[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_UShort(ushort[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToUShort();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }


        [Theory]
        [InlineData(new int[] { 0, 0, 0, 0, 0 })]
        [InlineData(new int[] { 1, 1, 1, 1, 1 })]
        [InlineData(new int[] { 0, 2, 4, 8, 16 })]
        [InlineData(new int[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_Int(int[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToInt();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }


        [Theory]
        [InlineData(new uint[] { 0, 0, 0, 0, 0 })]
        [InlineData(new uint[] { 1, 1, 1, 1, 1 })]
        [InlineData(new uint[] { 0, 2, 4, 8, 16 })]
        [InlineData(new uint[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_UInt(uint[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToUInt();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }


        [Theory]
        [InlineData(new float[] { 0, 0, 0, 0, 0 })]
        [InlineData(new float[] { 1, 1, 1, 1, 1 })]
        [InlineData(new float[] { 0, 2, 4, 8, 16 })]
        [InlineData(new float[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_Float(float[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToFloat();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }


        [Theory]
        [InlineData(new double[] { 0, 0, 0, 0, 0 })]
        [InlineData(new double[] { 1, 1, 1, 1, 1 })]
        [InlineData(new double[] { 0, 2, 4, 8, 16 })]
        [InlineData(new double[] { 56, 87, 4, 0, 125 })]
        public void Copy_IntegrityTest_Double(double[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToDouble();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }
        #endregion

        #region Casting
        [Theory]
        [InlineData(new float[] { 0, 0, 0, 0, 0 })]
        [InlineData(new float[] { 1, 1, 1, 1, 1 })]
        [InlineData(new float[] { 0, 2, 4, 8, 16 })]
        [InlineData(new float[] { 56, 87, 4, 0, 125 })]
        public void Cast_IntegrityTest_Float_To_Double(float[] data)
        {
            var chunk = new DataChunk(data);
            var cData = chunk.ToDouble();

            for (int i = 0; i < data.Length; i++)
            {
                Assert.True(data[i] == cData[i]);
            }
        }
        #endregion
    }
}
