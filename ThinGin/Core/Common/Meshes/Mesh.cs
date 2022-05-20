using System;
using System.Drawing;
using System.Threading;
using ThinGin.Core.Common.Types;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Geometry.Filters;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.Shaders;

namespace ThinGin.Core.Common.Meshes
{
    /// <summary>
    /// Holds and manages the lifespan of mesh data
    /// </summary>
    public class Mesh : IDisposable
    {
        #region Values
        private int _disposedValue = 0;
        /// <summary>
        /// Keep track of which engine we belong to
        /// </summary>
        private WeakReference<EngineInstance> _engineRef = new WeakReference<EngineInstance>(null);

        // Mesh data
        private byte[] _dataBuffer = null;
        private DataChunk _indicies = null;
        private VertexLayout _layout;
        // Renderer
        private IMeshRenderer _renderer = null;
        private Shader _shader = null;
        #endregion

        #region Properties
        public Shader Shader => _shader;
        public BoundingDimensions Bounds;
        public IMeshRenderer Renderer { get; protected set; } = null;
        public ETopology Topology = ETopology.TriangleList;

        /// <summary>
        /// Vertex count
        /// </summary>
        public int Count { get; protected set; } = 0;
        #endregion

        #region Accessors
        /// <summary>
        /// The <see cref="EngineInstance"/> which has ownership of this resource.
        /// </summary>
        protected EngineInstance Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        public byte[] Data => _dataBuffer;
        /// <summary>
        /// Whether or not the mesh has an indexing list for optimization.
        /// <para>Note: The mesh-builder system determines this internally by scanning the vertex data given to it if auto-optimizing is enabled.</para>
        /// </summary>
        public bool Indexed => _indicies is object;
        public DataChunk Indices => _indicies;
        public VertexLayout Layout => _layout;
        #endregion

        #region Constructors
        private Mesh(EngineInstance engine, Shader Shader)
        {
            if (engine is null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            _shader = Shader;
            _engineRef.SetTarget(engine);
        }

        public Mesh(EngineInstance engine, Shader Shader, byte[] Data, VertexLayout Layout, int Count, ETopology Topology, DataChunk Indicies = null) : this(engine, Shader)
        {
            _dataBuffer = Data ?? throw new ArgumentNullException(nameof(Data));

            _indicies = Indicies;
            _layout = Layout;
            this.Count = Count;
            this.Topology = Topology;
        }
        public Mesh(EngineInstance engine, Shader Shader, MeshBuilder Builder) : this(engine, Shader)
        {
            Update(Builder);
        }
        #endregion

        #region IDispose
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref _disposedValue, 1) == 0)
            {
                if (_dataBuffer is object)
                {
                    _dataBuffer = null;
                }

                if (_renderer is object)
                {
                    _renderer.Dispose();
                    _renderer = null;
                }

                if (_indicies is object)
                {
                    _indicies = null;
                }
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Updating
        /// <summary>
        /// Updates the mesh with new data
        /// </summary>
        /// <param name="Builder">Object contaiing the new data</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Update(MeshBuilder Builder)
        {
            if (Builder is null)
            {
                throw new ArgumentNullException(nameof(Builder));
            }
            _dataBuffer = Builder.Compile(Engine, out _indicies, out int vertexCount);
            Count = vertexCount;
            _layout = Builder.Layout;
            Topology = Builder.Topology;

            // Invalidate the rendering handler so it updates to the new data
            if (_renderer is object)
            {
                _renderer.Invalidate();
            }
        }
        #endregion

        #region Rendering
        public void Render() => Render(Topology);

        public void Render(ETopology topology)
        {
            if (_renderer is null)
            {
                if (_dataBuffer is null)
                {
                    throw new Exception($"Cannot upload mesh to GPU, {nameof(_dataBuffer)} is null!");
                }
                _renderer = Engine.Provider.Renderers.Create(Engine, this);
            }

            // Render the actual mesh
            _renderer.Render(topology);
        }

        #endregion

        #region Creation
        public static Mesh Create_Textured_Quad(EngineInstance Engine, Rectangle Bounds)
        {
            var layout = new VertexLayout(Position: new AttributeDescriptor(2, EValueType.Int32),
                                              UV: new AttributeDescriptor(2, EValueType.Int16));
            var meshBuilder = new MeshBuilder(layout, ETopology.TriangleList);
            meshBuilder.Filters.Add(ReindexingFilter.Instance);

            meshBuilder.Push_Data(0, new[] {
                    new DataChunk(Bounds.Left, Bounds.Top),
                    new DataChunk(Bounds.Left, Bounds.Bottom),
                    new DataChunk(Bounds.Right, Bounds.Bottom),

                    new DataChunk(Bounds.Right, Bounds.Bottom),
                    new DataChunk(Bounds.Right, Bounds.Top),
                    new DataChunk(Bounds.Left, Bounds.Top),
                        });

            meshBuilder.Push_Data(1, new[] {
                        new DataChunk((short)0, (short)1),
                        new DataChunk((short)0, (short)0),
                        new DataChunk((short)1, (short)0),

                        new DataChunk((short)1, (short)0),
                        new DataChunk((short)1, (short)1),
                        new DataChunk((short)0, (short)1),
                            });
            // TODO: Add default shader for unlit textured quads
            return new Mesh(Engine, meshBuilder);
        }
        public static Mesh Create_Textured_Quad(EngineInstance Engine, RectangleF Bounds)
        {
            var layout = new VertexLayout(Position: new AttributeDescriptor(2, EValueType.Int32),
                                              UV: new AttributeDescriptor(2, EValueType.Int16));
            var meshBuilder = new MeshBuilder(layout, ETopology.TriangleList);
            meshBuilder.Push_Data(0, new[] {
                    new DataChunk(Bounds.Left, Bounds.Top),
                    new DataChunk(Bounds.Left, Bounds.Bottom),
                    new DataChunk(Bounds.Right, Bounds.Bottom),

                    new DataChunk(Bounds.Right, Bounds.Bottom),
                    new DataChunk(Bounds.Right, Bounds.Top),
                    new DataChunk(Bounds.Left, Bounds.Top),
                        });

            meshBuilder.Push_Data(1, new[] {
                        new DataChunk((short)0, (short)1),
                        new DataChunk((short)0, (short)0),
                        new DataChunk((short)1, (short)0),

                        new DataChunk((short)1, (short)0),
                        new DataChunk((short)1, (short)1),
                        new DataChunk((short)0, (short)1),
                            });

            // TODO: Add default shader for unlit textured quads
            return new Mesh(Engine, meshBuilder);
        }
        #endregion
    }
}
