using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Meshes;

namespace ThinGin.Core.Common.Geometry
{
    /// <summary>
    /// This structure just provides a convenient unified storage place for all of the data for a set of verticies.
    /// </summary>
    public class GeometryData : IDisposable
    {
        #region Values
        private int disposed = 0;
        #endregion

        #region Properties
        public ETopology Topology;
        public VertexLayout Layout;

        public List<List<int>> Indices;

        /// <summary>
        /// Stores all of the data for all attributes
        /// </summary>
        public List<List<DataChunk>> Attribute;

        /// <summary>
        /// Tracks the mimimum specified value for attributes
        /// </summary>
        public double[] minimums;
        /// <summary>
        /// Tracks the maximum specified value for attributes
        /// </summary>
        public double[] maximums;
        #endregion

        #region Accessors
        /// <summary> Number of verticies. </summary>
        public int Verticies => Attribute[0].Count;
        #endregion

        #region Constructors
        /// <summary>
        /// Performs a shallow clone of another geometry data instance
        /// </summary>
        /// <param name="other"></param>
        public GeometryData(GeometryData other)
        {
            Layout = other.Layout;
            Topology = other.Topology;

            minimums = new double[Layout.Count];
            maximums = new double[Layout.Count];

            Indices = new List<List<int>>(Layout.Count);
            Attribute = new List<List<DataChunk>>(Layout.Count);

            for (int i = 0; i < Layout.Count; i++)
            {
                minimums[i] = other.minimums[i];
                maximums[i] = other.maximums[i];
                Indices.Add(new List<int>(other.Indices[i]));
                Attribute.Add(new List<DataChunk>(other.Attribute[i]));
            }
        }


        public GeometryData(VertexLayout layout, ETopology topology)
        {
            Layout = layout;
            Topology = topology;

            minimums = new double[Layout.Count];
            maximums = new double[Layout.Count];

            Indices = new List<List<int>>();
            Attribute = new List<List<DataChunk>>();

            for (int i = 0; i < layout.Count; i++)
            {
                Indices.Add(new List<int>());
                Attribute.Add(new List<DataChunk>());
            }
        }
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (System.Threading.Interlocked.Exchange(ref disposed, 1) == 0)
            {
                Indices.Clear();
                Attribute.Clear();
            }
        }

        // ~GeometryData()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Compiler
        public byte[] Compile()
        {
            // The unified buffer size is the summation of multiplying the number of entries for each attribute by the size of its components
            int bufferSize = 0;
            for (var attrID = 0; attrID < Layout.Count; attrID++)
            {
                var attr = Layout[attrID];
                bufferSize += (attr.Size * Attribute[attrID].Count);
            }

            byte[] finalBuffer = new byte[bufferSize];

            for (var attrID = 0; attrID < Layout.Count; attrID++)
            {
                var attr = Layout[attrID];

                Parallel.For(0, Attribute[attrID].Count,
                    (int index) =>
                    {
                        var offset = attr.Offset + (attr.Stride * index);
                        System.Buffer.BlockCopy(Attribute[attrID][index].Data, 0, finalBuffer, offset, attr.Size);
                    });
            }

            return finalBuffer;
        }

        /// <summary>
        /// Minifys a given array of integer values down into the smallest value type that can hold all of the used values and then returns a single chunk holding all of the new indices
        /// </summary>
        /// <param name="Indices"></param>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        public DataChunk Compress_Indices(int[] Indices, int? maxIndex = null)
        {
            int maxIndice = 0;
            if (maxIndex.HasValue)
            {
                maxIndice = maxIndex.Value;
            }
            else
            {
                for (int i = Indices.Length - 1; i >= 0; i--)
                {
                    maxIndice = (maxIndice < Indices[i]) ? Indices[i] : maxIndice;
                }
            }

            // Okay here is the part where we compress our indices into a single DataChunk into the smallest value type possible!
            if (maxIndice <= Byte.MaxValue)// we can use 8-bits!
            {
                var idx = new Byte[Indices.Length];
                Parallel.For(0, Indices.Length, (int i) =>
                {
                    idx[i] = (Byte)Indices[i];
                });

                return new DataChunk(idx);
            }
            else if (maxIndice <= UInt16.MaxValue)
            {
                var idx = new UInt16[Indices.Length];
                Parallel.For(0, Indices.Length, (int i) =>
                {
                    idx[i] = (UInt16)Indices[i];
                });

                return new DataChunk(idx);
            }
            else// 32 whole entire bits :(
            {
                var idx = new UInt32[Indices.Length];
                Parallel.For(0, Indices.Length, (int i) =>
                {
                    idx[i] = (UInt32)Indices[i];
                });

                return new DataChunk(idx);
            }

            return null;
        }
        #endregion

        #region Optimizer
        public void Optimize()
        {

        }
        #endregion
    }
}
