using System;
using System.Collections.Generic;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Geometry;
using ThinGin.Core.Common.Data.Interfaces;
using ThinGin.Core.Common.Geometry.Filters;
using ThinGin.Core.Common.Interfaces;
using System.Threading.Tasks;

namespace ThinGin.Core.Common.Meshes
{
    /// <summary>
    /// Builds mesh data, including but not limited to: verticies, texturing coordinates, normals, vertex color, bounding box, & order indices
    /// </summary>
    public class MeshBuilder
    {
        #region Values
        private GeometryData _data;
        #endregion

        #region Properties
        public readonly bool Optimize = false;
        #endregion

        #region Accessors
        public GeometryData Data => _data;
        public VertexLayout Layout => _data.Layout;
        public ETopology Topology { get => _data.Topology; set => _data.Topology = value; }
        /// <summary>
        /// List of filters to be applied during compilation
        /// </summary>
        public List<IGeometryFilter> Filters = new List<IGeometryFilter>();
        #endregion


        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Layout"></param>
        /// <param name="PrimitiveType"></param>
        /// <param name="AutoOptimize">If true then a default index optimization geometry filter will be automatically added to the filters list in order to optimize the mesh.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MeshBuilder(VertexLayout Layout, ETopology PrimitiveType, bool AutoOptimize=true)
        {
            if (Layout is null)
            {
                throw new ArgumentNullException(nameof(Layout));
            }

            _data = new GeometryData(Layout, PrimitiveType);

            Optimize = AutoOptimize;
        }
        #endregion

        #region Compiling
        public byte[] Compile(IRenderEngine Engine, out DataChunk outIndices, out int VertexCount)
        {
            // Sanity check to ensure all of our data lines up in size, all stacks need to be the same length as the vertex stack because they are vertex attributes (like normals or color)
            int indexLen = _data.Indices[0].Count;
            int attrLen = _data.Attribute[0].Count;
            for (int attrIndex = 0; attrIndex < _data.Layout.Count; attrIndex++)
            {
                if (_data.Indices[attrIndex].Count != indexLen)
                {
                    throw new FormatException($"Vertex attribute indice count mismatch (Attribute Index: {attrIndex}), some of the vertex attribute indice array lengths are not equal! A rendering index list must be specified for each attribute in order to know their drawing order!");
                }
            }

            // Apply filters
            for (int i = Filters.Count - 1; i >= 0; i--)
            {
                _data = Filters[i].Process(_data, null);
            }

            // Optimize the vertex data by removing duplicates/unused verticies and generally minifying the data.
            if (Optimize)
            {
                _data = ReindexingFilter.Instance.Process(_data, null);
                _data = MinifyFilter.Instance.Process(_data, null);
            }

            /*
             * Priorities:
             *  ) Minify: use smallest value type possible. (eg; for color types use single unsigned byte backing value types and upload with 'normalize' set to true)
             *  ) Interleave: as many attributes together as possible without duplicating data
             *  ) Align to multiples of 4 bytes, most modern GPUs prefer this (allegedly)
             *  
             *  ) Attempt to use triangle strips...
             *  ) Using the primitive restart feature can enable easy usage of single buffers for meshes
             */

            // First we need to know a few things for each attributes dataset
            //  - What is the minimum value used
            //  - What is the maximum value used
            //  - If its a floating point type then:
            //      - Does any used value go out of the [0,1] or [-1,1] range?
            //      - What are the number of unique values used that are within the normalized range? ([0,1] or [-1,1])


            // Compute the most efficient buffer layout for the vertex data.
            //if (Engine.BufferSupport.IndexDivisor)
            //{
            //    Reconfigure_Layout();
            //}
            //else
            //{
                Lazy_Layout_Attributes();
            //}

            int maxIndice = 0;
            int PrimaryAttribId = _find_primary_attribute_id();
            var Indices = Data.Indices[PrimaryAttribId].ToArray();

            // === Interleave all byte data ===
            VertexCount = (int)maxIndice;
            outIndices = Data.Compress_Indices(Indices, maxIndice);

            return Data.Compile();
        }

        /// <summary>
        /// Establishes the final vertex layout ordering
        /// </summary>
        /// <returns></returns>
        //private VertexLayout Reconfigure_Layout()
        //{
        //    VertexLayout Layout = _data.Layout;
        //    // It is unlikely that all attributes will have equal numbers of items (which would allow for perfect interleaving)
        //    // In reality we will have to decide on an index divisor, and reorder/reindex each attribute indice list such that the correct access order for all attributes is represented by a single index list.
        //    // The concepts from the FastReindexer filter can allow us to transform one index list into another, that should be easy.
        //    // But then, if the attribute lists contain different amounts of items, then we will have to do one of the following:
        //    //  ) Duplicate attribute entries for the smaller lists (easy, but will waste lots of memory)
        //    //  ) Find an index divisor relationship between the different indice lists.
        //    //      - So essentially we can shuffle the smaller list around to get the smallest number of indices that dont satisfy a divisor relationship.
        //    //          And then for the relationship 'misses' we can just add in duplicated data at the end of the attributes data list!
        //    //      - The correct index divisor will likely be the number of sequential repeating indices.
        //    //          Example; [v:<0,1,2,3> | n:<1,1,1,1>] where 'n' is the smaller attribute list
        //    //          In this example, the divisor we should check first is 4. Because 'n' has associated the 4 consecutive 'v' values increasing 1 at a time (0,1,2,3) -
        //    //          with the same number (4 indices) of consecutive but unchanging indices (1,1,1,1).
        //    //      - So, the divisor relationship is always:
        //    //          big_list -> series indices increasing by 1 (so no skipped numbers)
        //    //              Maps to =>
        //    //          small_list -> series of consecutive same indice numbers
        //    //      - Each occurance of a relationship of this kind is merely a potential one, we are almost always going to have to make changes to the lists
        //    //          but the first step is to find a potential association and then scan all of the rest of the indices in both lists to make sure the relationship holds up.
        //    //          when the relationship is broken.... uhhh... we have to... idk if moving the indices will be correct...
        //    //          We might have to find the delta of how the relationship missed and then apply that to both lists to correct them?
        //    //          yea that sounds like the correct direction to head in.
        //    // And fallback to a non-interleaved format by just ordering each attributes data as consecutive uniform blocks of data for each attribute

        //    // Lastly ensure the stride of the vertex layout is a multiple of 4 to make the GPU happy (its worth even wasting space, because its such an absurd performance detriment if its not aligned correctly...)
        //}


        /// <summary>
        /// Computes a final layout and reorders attribute data by duplicating the data of smaller data sets
        /// </summary>
        private void Lazy_Layout_Attributes()
        {
            // Ok the primary list is going to be the one with the most items
            // Meaning it has the most items to be used and thus must have the greatest range of indices
            int PrimaryID = _find_primary_attribute_id();
            List<int> Indices = _data.Indices[PrimaryID];

            // Ok now we want to take all other attributes and transform them such that their data array matches the primary index list, thus duplicating their data
            for (int ID = 0; ID < _data.Layout.Count; ID++)
            {
                if (ID != PrimaryID)
                {
                    if (Indices.Count != _data.Indices[ID].Count)
                    {
                        throw new Exception($"Attribute indice count mismatch, all attributes must be given the same number of draw order indices!");
                    }


                    var attrData = new DataChunk[_data.Attribute[PrimaryID].Count];
                    // The indice from our primary attribute tells us where the data for this attribute SHOULD be as its the indice that the GPU will reference during draw calls.
                    Parallel.For(0, Indices.Count,
                        (int i) =>
                        {
                            int PrimaryIndice = Indices[i];// where it should be
                            int CurrentIndice = _data.Indices[ID][i];// where it is now.
                            attrData[PrimaryIndice] = _data.Attribute[ID][CurrentIndice];
                        });

                    _data.Attribute[ID] = new List<DataChunk>(attrData);
                    _data.Indices[ID] = Indices;
                }
            }

            // Generate the fully interleaved layout
            int offset = 0;
            var layoutSize = _data.Layout.Size;
            for (int i = 0; i < _data.Layout.Count; i++)
            {
                var attrib = _data.Layout[i];

                attrib.Stride = layoutSize;
                attrib.Offset = offset;
                offset += attrib.Size;
            }
        }

        private int _find_primary_attribute_id()
        {
            int id = 0;
            int count = 0;

            for (int i = 0; i < _data.Attribute.Count; i++)
            {
                if (count < _data.Attribute.Count)
                {
                    id = i;
                    count = _data.Attribute.Count;
                }
            }

            return id;
        }

        #endregion

        #region Pushing
        public unsafe void Push_Data(int AttributeIndex, IEnumerable<DataChunk> Data)
        {
            var AttributeDataList = _data.Attribute[AttributeIndex];
            _push_data(AttributeIndex, Data, Layout[AttributeIndex], ref AttributeDataList);
        }


        /// <summary>
        /// Pushes a series of indices onto all vertex attribute indice lists
        /// </summary>
        /// <param name="Indices"></param>
        public unsafe void Push_Indices(IEnumerable<int> Indices)
        {
            for (int i = 0; i < Data.Layout.Count; i++)
            {
                Data.Indices[i].AddRange(Indices);
            }
        }

        /// <summary>
        /// Pushes a series of indices onto the vertex attribute indice list specified by <paramref name="AttributeIndex"/>
        /// </summary>
        /// <param name="Indices"></param>
        public unsafe void Push_Indices(int AttributeIndex, IEnumerable<int> Indices)
        {
            Data.Indices[AttributeIndex].AddRange(Indices);
        }


        private unsafe void _push_data(int AttributeIndex, IEnumerable<DataChunk> Chunks, IAttributeDescriptor Descriptor, ref List<DataChunk> Destination)
        {
            if (Descriptor is null)
            {
                throw new InvalidOperationException($"Attempt to push data to mesh without a set memory layout descriptor!");
            }

            if (Destination is null)
            {
                throw new ArgumentNullException(nameof(Destination));
            }

            foreach (var chunk in Chunks)
            {
                if (chunk is null)
                {
                    throw new ArgumentNullException($"Cannot push null data to data array!");
                }

                if (chunk.Type != Descriptor.ValueType)
                {
                    throw new FormatException($"Data type mismatch: chunk data value type does not match the given memory layout descriptor!");
                }

                if (chunk.Count != Descriptor.Count)
                {
                    throw new FormatException($"Data size mismatch: the number of chunk elements does not match the given memory layout descriptor!");
                }

                if (chunk.Length != Descriptor.Size)
                {
                    throw new FormatException($"Data size mismatch: chunk data length does not match the given memory layout descriptor!");
                }

                Destination.Add(chunk);

                _data.minimums[AttributeIndex] = (_data.minimums[AttributeIndex] > chunk.Min) ? chunk.Min : _data.minimums[AttributeIndex];
                _data.maximums[AttributeIndex] = (_data.maximums[AttributeIndex] < chunk.Max) ? chunk.Max : _data.maximums[AttributeIndex];
            }
        }
        #endregion
    }
}
