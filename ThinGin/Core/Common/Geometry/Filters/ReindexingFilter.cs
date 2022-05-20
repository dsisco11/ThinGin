using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ThinGin.Core.Common.Data;

namespace ThinGin.Core.Common.Geometry.Filters
{
    public class ReindexingFilter : IGeometryFilter
    {
        #region Static Instance
        public static IGeometryFilter Instance = new ReindexingFilter(true);
        #endregion

        #region Properties
        public readonly bool RemoveUnused;
        #endregion

        #region Constructors
        public ReindexingFilter(bool RemoveUnused)
        {
            this.RemoveUnused = RemoveUnused;
        }
        #endregion


        public GeometryData Process(GeometryData Geometry, object Context)
        {/* Docs: https://arxiv.org/pdf/2109.09812.pdf */
            GeometryData newGeometry = new GeometryData(Geometry.Layout, Geometry.Topology);

            Parallel.For(0, Geometry.Layout.Count,
                (int attrIndex) =>
                {
                    _perform_data_reindexing(Geometry.Attribute[attrIndex], Geometry.Indices[attrIndex], out DataChunk[] newData, out int[] newIndices);
                    newGeometry.Indices[attrIndex] = new List<int>(newIndices);
                    newGeometry.Attribute[attrIndex] = new List<DataChunk>(newData);

                });

            return newGeometry;
        }

        private void _perform_data_reindexing(List<DataChunk> ChunkList, List<int> Indices, out DataChunk[] outData, out int[] outIndices)
        {/* Docs: https://arxiv.org/pdf/2109.09812.pdf */

            int IndiceCount = Indices.Count;
            DataChunk[] Data = ChunkList.ToArray();
            bool[] isUsed;

            if (RemoveUnused)
            {
                // === 2.1 ===
                // Removing unused data.
                // if a piece of data is not referenced in the index list,
                // then replace it with the item from the first index in the list and said item will be auto removed as a 'duplicate'

                // First, flag all used items
                isUsed = new bool[IndiceCount];
                Parallel.For(0, Indices.Count,
                    (int i) =>
                    {
                        isUsed[Indices[i]] = true;
                    });

                // Second, replace items not flagged as used with a common item to trigger removal later
                int commonIndex = Indices[0];
                Parallel.For(0, Data.Length,
                    (int i) =>
                    {
                        if (!isUsed[i])
                        {
                            Data[i] = Data[commonIndex];
                        }
                    });
            }

            // === 2.2 ===
            // Make each reference to a duplicated item point to the same instance of that item
            int[] orgIdx = new int[Data.Length];// Tracks the original index of each chunk within the initial unsorted array
            Parallel.For(0, orgIdx.Length,
                (int i) =>
                {
                    orgIdx[i] = i;
                });

            // Sort indice list according to item comparison order
            Array.Sort(Data, orgIdx);

            // Detecting duplicates
            var nodup = new int[Data.Length];
            nodup[0] = 1;// First element has to be 1...

            // Note: we save some computation here by knowing that the result of our comparison can only ever be 0 or 1,
            //  so we store it directly as this is precisely the value we need in the next steps
            // We are going to create an array that tells us which items are the same as their predecessor
            Parallel.For(1, nodup.Length, 
                (int i) => {
                    nodup[i] = (Data[i].CompareTo(Data[i - 1]) == 0) ? 0 : 1;
                    //nodup[i] = Data[orgIdx[i]].CompareTo(Data[orgIdx[i - 1]]);
                });

            // Now, perform, a postfix sum over the nodup array to get a new index array
            var newIdx = new int[nodup.Length];
            for (int i = 1; i < newIdx.Length; i++)
            {
                newIdx[i] = newIdx[i - 1] + nodup[i];
                //newIdx[i] = newIdx[i - 1] + nodup[i] - 1;
            }

            // == 2.3 ==
            // Constructing the new item array

            // The last index will always be the highest number here
            int newN = newIdx[newIdx.Length - 1] + 1;

            // Compile the new array of unduped chunks
            var newData = new DataChunk[newN];
            Parallel.For(0, newIdx.Length,
                (int i) => {
                    newData[newIdx[i]] = Data[i];
                });

            // === 2.4 ===
            // Constructing the new index array
            int[] perm = new int[orgIdx.Length];

            Parallel.For(0, perm.Length,
                (int i) => {
                    perm[orgIdx[i]] = i;
                });

            // With this, we can now update every old index i to the value newIdx[perm[i]]
            int[] newIndices = new int[Indices.Count];
            Parallel.For(0, newIndices.Length,
                (int i) => {
                    var indice = Indices[i];
                    //var originalIndex = orgIdx[indice];
                    newIndices[i] = newIdx[perm[indice]];
                });
            // To explain why this works: The first lookup (i→perm[i])
            // translates from the original vertex index i to where
            // this vertex would have been after sorting; the second
            // (perm[i]→newIdx[perm[i]]) translates from the position after sorting to the position after compacting(ie, after
            // removing the duplicates)—which is where the vertex at i has
            // ended up after both steps.

            outData = newData;
            outIndices = newIndices;
        }
    }
}
