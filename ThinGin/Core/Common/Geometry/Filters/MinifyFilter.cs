using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Geometry.Filters
{
    /// <summary>
    /// Analyses the range of utilized values for any attribute data and, when possible, reformats it to be smaller.
    /// </summary>
    public class MinifyFilter : IGeometryFilter
    {
        #region Static Instance
        public static IGeometryFilter Instance = new MinifyFilter(true);
        #endregion

        #region Properties
        public readonly bool RemoveUnused;
        #endregion

        #region Constructors
        public MinifyFilter(bool RemoveUnused)
        {
            this.RemoveUnused = RemoveUnused;
        }
        #endregion


        public GeometryData Process(GeometryData Geometry, object Context)
        {
            GeometryData newGeometry = new GeometryData(Geometry);

            Parallel.For(0, Geometry.Layout.Count,
                (int attrIndex) =>
                {
                    var newData = _perform_data_minification(attrIndex, Geometry);
                    if (newData is null)
                    {
                        newGeometry.Attribute[attrIndex] = Geometry.Attribute[attrIndex];
                    }
                    else
                    {
                        newGeometry.Attribute[attrIndex] = newData;
                    }
                });

            return newGeometry;
        }

        private List<DataChunk> _perform_data_minification(int AttributeIndex, GeometryData Geometry)
        {
            var Layout = Geometry.Layout[AttributeIndex];
            var min = Geometry.minimums[AttributeIndex];
            var max = Geometry.maximums[AttributeIndex];
            var absMin = Math.Abs(min);
            var absMax = Math.Abs(max);
            var upper = Math.Max(absMin, absMax);


            if (Layout.ValueType.HasFlag(EValueType.IsFloatingPoint))
            {
                // XXX: TODO: Implement floating point value bit-depth resolution logic to minify float values into fixed-point types that the GPU can normalize
                // For floats we want to reduce to a value type that is smaller but which still has enough resolution such that, once normalized, all used values can be recreated.

            }
            else
            {
                // Here we just check the min/max and convert to the smallest value type that can hold the range properly
                if (min >= 0)// Aiming for an unsigned type
                {
                    int BitsRequired = 0;
                    EValueType bitFlag = 0;
                    if (upper <= Byte.MaxValue) { BitsRequired = 8; bitFlag = EValueType.Is8BitType; }
                    else if (upper <= UInt16.MaxValue) { BitsRequired = 16; bitFlag = EValueType.Is16BitType; }
                    else if (upper <= UInt32.MaxValue) { BitsRequired = 32; bitFlag = EValueType.Is32BitType; }

                    // Note: we avoid wasting time with pointless conversions by returning null if the data is already using the same number of bits
                    if (Layout.ValueType.HasFlag(bitFlag)) 
                        return null;


                    if (BitsRequired == 8)
                    {
                        DataChunk[] newData = new DataChunk[Geometry.Attribute[AttributeIndex].Count];
                        Parallel.For(0, newData.Length,
                            (int i) =>
                            {
                                newData[i] = new DataChunk(Geometry.Attribute[AttributeIndex][i].ToByte());
                            });
                    }
                    else if (BitsRequired == 16)
                    {
                        DataChunk[] newData = new DataChunk[Geometry.Attribute[AttributeIndex].Count];
                        Parallel.For(0, newData.Length,
                            (int i) =>
                            {
                                newData[i] = new DataChunk(Geometry.Attribute[AttributeIndex][i].ToUShort());
                            });
                    }
                    else if (BitsRequired == 32)
                    {
                        DataChunk[] newData = new DataChunk[Geometry.Attribute[AttributeIndex].Count];
                        Parallel.For(0, newData.Length,
                            (int i) =>
                            {
                                newData[i] = new DataChunk(Geometry.Attribute[AttributeIndex][i].ToUInt());
                            });
                    }
                }
                else// We require a signed data type
                {
                    int BitsRequired = 0;
                    EValueType bitFlag = 0;
                    if (upper <= SByte.MaxValue) { BitsRequired = 8; bitFlag = EValueType.Is8BitType; }
                    else if (upper <= Int16.MaxValue) { BitsRequired = 16; bitFlag = EValueType.Is16BitType; }
                    else if (upper <= Int32.MaxValue) { BitsRequired = 32; bitFlag = EValueType.Is32BitType; }

                    // Note: we avoid wasting time with pointless conversions by returning null if the data is already using the same number of bits
                    if (Layout.ValueType.HasFlag(bitFlag))
                        return null;


                    if (BitsRequired == 8)
                    {
                        DataChunk[] newData = new DataChunk[Geometry.Attribute[AttributeIndex].Count];
                        Parallel.For(0, newData.Length,
                            (int i) =>
                            {
                                newData[i] = new DataChunk(Geometry.Attribute[AttributeIndex][i].ToSByte());
                            });
                    }
                    else if (BitsRequired == 16)
                    {
                        DataChunk[] newData = new DataChunk[Geometry.Attribute[AttributeIndex].Count];
                        Parallel.For(0, newData.Length,
                            (int i) =>
                            {
                                newData[i] = new DataChunk(Geometry.Attribute[AttributeIndex][i].ToShort());
                            });
                    }
                    else if (BitsRequired == 32)
                    {
                        DataChunk[] newData = new DataChunk[Geometry.Attribute[AttributeIndex].Count];
                        Parallel.For(0, newData.Length,
                            (int i) =>
                            {
                                newData[i] = new DataChunk(Geometry.Attribute[AttributeIndex][i].ToInt());
                            });
                    }
                }
            }

            return null;
        }
    }
}
