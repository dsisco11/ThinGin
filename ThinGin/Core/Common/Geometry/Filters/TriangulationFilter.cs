
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Geometry.Filters
{
    /// <summary>
    /// Filters non-triangulated geometry into triangle primitives
    /// </summary>
    public class TriangulationFilter : IGeometryFilter
    {
        #region Static Instance
        public static IGeometryFilter Instance = new TriangulationFilter();
        #endregion

        #region Properties
        public readonly float Scaling = 1.0f;
        #endregion

        #region Constructors
        public TriangulationFilter()
        {
        }
        public TriangulationFilter(float Scaling)
        {
            this.Scaling = Scaling;
        }
        #endregion

        #region Processing
        public GeometryData Process(GeometryData Geometry, object Context)
        {
            Parallel.For(0, Geometry.Indices.Count, (int AttributeID) =>
            {
                var pattern = get_triangulation_pattern(Geometry.Topology);
                int[] newIndices = _triangulate_indices(Geometry.Topology, Geometry.Indices[AttributeID], pattern);
                Geometry.Indices[AttributeID] = new List<int>(newIndices);
            });

            // Remember to change to the new topology
            Geometry.Topology = ETopology.Triangles;
            return Geometry;
        }
        #endregion

        #region Triangle struct
        private struct Tri 
        { 
            public readonly int v0; 
            public readonly int v1; 
            public readonly int v2; 
            public Tri(int V0, int V1, int V2) 
            { 
                v0 = V0; 
                v1 = V1; 
                v2 = V2; 
            } 
        }
        #endregion


        /// <summary>
        /// Intakes the triangle indice index (the indices position within the array of indices) and returns the index it maps to for a given primitive type.
        /// Essentially this delegate just reorders the already existing geometry indices such that they represent a triangle primitive pattern
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Count">Total number of indices</param>
        /// <returns>Indice with relative transformation applied</returns>
        private delegate int IndiceReindexingDelegate(int Index, int Count);

        #region Triangulation Methods

        private int get_primitive_count(ETopology primitiveType, int indiceCount)
        {
            return primitiveType switch
            {
                ETopology.PointList => indiceCount,// n
                ETopology.LineList => indiceCount / 2,// n/2
                ETopology.LineLoop => indiceCount+1,// we add 1 more here to cause the system to add room for an additional triangle for the return line
                ETopology.LineStrip => Math.Max(0, indiceCount - 1),//  n-1
                ETopology.Triangles => indiceCount / 3,// n/3
                ETopology.TriangleStrip => Math.Max(0, indiceCount - 2),// n-2
                ETopology.TriangleFan => Math.Max(0, indiceCount - 2),// n-2
                ETopology.Quads => indiceCount / 4,// n/4
                ETopology.QuadStrip => Math.Max(0, indiceCount - 2) / 2,// (n-2)/2
                ETopology.Polygon => 1,// A polygon uses all of the given points to form a single face
                _ => throw new NotImplementedException(primitiveType.ToString())
            };
        }

        private int get_triangles_per_primitive(ETopology primitiveType, int indiceCount)
        {
            return primitiveType switch
            {
                ETopology.PointList => 1,
                ETopology.LineList => 1,
                ETopology.LineLoop => 1,
                ETopology.LineStrip => 1,
                ETopology.Triangles => 1,
                ETopology.TriangleStrip => 1,
                ETopology.TriangleFan => 1,
                ETopology.Quads => 2,
                ETopology.QuadStrip => 2,
                ETopology.Polygon => Math.Max(0, indiceCount - 2),
                _ => throw new NotImplementedException(primitiveType.ToString())
            };
        }

        /// <summary> a lookup table for mapping quad indices to triangles </summary>
        static readonly int[] quad_to_tri_table = new[] { 0, 1, 2, 0, 2, 3 };
        private IndiceReindexingDelegate get_triangulation_pattern(ETopology primitiveType)
        {
            return primitiveType switch
            {
                ETopology.PointList => (int n, int c) => (n / 3),
                ETopology.LineList => (int n, int c) => (n / 2),
                ETopology.LineLoop => (int n, int c) => (n / 2) * (1 - (n < c ? 1 : 0)),
                ETopology.LineStrip => (int n, int c) => (n),
                ETopology.Triangles => (int n, int c) => n,
                //EPrimitiveType.TriangleStrip => (int n, int c) => (n),
                //EPrimitiveType.TriangleFan => (int n, int c) => (n),
                ETopology.Quads => (int n, int c) => ((n / 6) * 4) + quad_to_tri_table[n % 6],
                ETopology.QuadStrip => (int n, int c) => ((n / 6) * 2) + quad_to_tri_table[n % 6],
                //EPrimitiveType.Polygon => (int n, int c) => (n),
                _ => throw new NotImplementedException(primitiveType.ToString())
            };
        }

        private int reindex(ETopology primitiveType, int n, int c)
        {
            return primitiveType switch
            {
                ETopology.PointList => (n / 3),
                ETopology.LineList => (n / 2),
                ETopology.LineLoop => (n / 2) * (1 - (n < c ? 1 : 0)),
                ETopology.LineStrip => (n),
                ETopology.Triangles => n,
                //EPrimitiveType.TriangleStrip => (n),
                //EPrimitiveType.TriangleFan => (n),
                ETopology.Quads => ((n / 6) * 4) + quad_to_tri_table[n % 6],
                ETopology.QuadStrip => ((n / 6) * 2) + quad_to_tri_table[n % 6],
                //EPrimitiveType.Polygon => (n),
                _ => throw new NotImplementedException(primitiveType.ToString())
            };
        }

        private int[] _triangulate_indices(ETopology Primitive, List<int> Indices, IndiceReindexingDelegate Pattern)
        {
            int primitiveCount = get_primitive_count(Primitive, Indices.Count);
            int trisPer = get_triangles_per_primitive(Primitive, Indices.Count);
            int triCount = primitiveCount * trisPer;
            var indices = new int[triCount * 3];

            for (int i = 0; i < indices.Length; i++)
            {
                //int idx = Pattern(i, indices.Length);
                int idx = reindex(Primitive, i, indices.Length);
                indices[i] = Indices[idx];
            }

            return indices;
        }

        #endregion
    }
}
