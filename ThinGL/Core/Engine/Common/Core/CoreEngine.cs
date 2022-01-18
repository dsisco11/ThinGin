
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using System.Drawing;
using ThinGin.Core.Common.Textures;
using System.Linq;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Rendering;
using ThinGin.Core.Common;
using ThinGin.Core.Common.Cameras;
using ThinGin.Core.Common.Units;
using ThinGin.Core.World;

namespace ThinGin.Core.Engine.Common.Core
{
    /// <summary>
    /// Provides abstracted access to generic rendering functionality, manages state to avoid redundant calls.
    /// </summary>
    public abstract class CoreEngine : IRenderEngine
    {
        #region Values
        private int _initialized = 0;
        public readonly object Context;
        protected HashSet<string> APIExtensions = null;
        private Rectangle _viewport = Rectangle.Empty;

        protected ConcurrentQueue<EngineObject> _update_queue = new ConcurrentQueue<EngineObject>();
        protected ConcurrentQueue<EngineObject> _release_queue = new ConcurrentQueue<EngineObject>();
        protected ConcurrentQueue<EngineObject> _initialization_queue = new ConcurrentQueue<EngineObject>();
        protected ConcurrentQueue<EngineJob> _job_polling_queue = new ConcurrentQueue<EngineJob>();
        protected ConcurrentQueue<Action> _deferred_action_queue = new ConcurrentQueue<Action>();

        protected IEngineBindable activeFramebuffer_Read = null;
        protected IEngineBindable activeFramebuffer_Write = null;

        public Texture _default_texture = null;


        private readonly Camera camera;
        private SpatialOrientation coords = new SpatialOrientation();
        private WorldManager world;

        public EngineCompatabilityList Compatability { get; protected set; } = null;
        #endregion

        #region Settings
        public bool AutoCompressTextures { get; set; } = false;
        public bool AutoGenerateMipMaps { get; set; } = false;
        #endregion

        #region Accessors
        /// <summary>
        /// <inheritdoc cref="IRenderEngine.Camera"/>
        /// </summary>
        public Camera Camera => camera;
        public WorldManager World { get => world; set => world = value; }
        public SpatialOrientation Space { get => coords; set => coords = value; }

        /// <summary> Graphics platform implementation for the engine </summary>
        public abstract IGraphicsImplementation Provider { get; }
        public TextureCache TextureCache { get; private set; }


        /// <summary>
        /// The default texture is used in scenarios wherein a missing or otherwise invalid texture is referenced.
        /// <para>By default this texture is a Source-esq magenta checkerboard pattern, however this can be overriden by simply setting this Default value reference to some other texture.</para>
        /// </summary>
        public Texture Default_Texture
        {
            get
            {
                if (_default_texture is null)
                {
                    _default_texture = Provider.Textures.Create(this, PixelDescriptor.Rgb);
                    _default_texture.TryLoad(new TextureMetadata(PixelDescriptor.Rgb, width: 1, height: 1), new byte[] { 255, 105, 180 });
                }

                return _default_texture;
            }
            set => _default_texture = value;
        }
        #endregion

        #region Events
        public event Action OnViewportResized;
        #endregion

        #region Constructors
        public CoreEngine(object Context)
        {
            this.Context = Context;
            TextureCache = new TextureCache();
            camera = new Camera(this);
        }
        #endregion

        #region Initialization
        public virtual void Initialize()
        {
            if (System.Threading.Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                var extList = from str in Populate_Extensions() select str.ToLowerInvariant();
                APIExtensions = new HashSet<string>(extList);
            }
        }
        #endregion

        #region Viewport
        public float Get_AspectRatio() => MathE.Max(1, (float)_viewport.Width) / MathE.Max(1, (float)_viewport.Height);
        public Rectangle Get_Viewport() => _viewport;
        public virtual void Set_Viewport(Rectangle area)
        {
            _viewport = area;
            OnViewportResized?.Invoke();
        }
        #endregion

        #region Extensions
        protected abstract ICollection<string> Populate_Extensions();

        /// <summary>
        /// Checks for OpenGL extension support
        /// </summary>
        /// <param name="extension"></param>
        /// <returns><see langword="true"/> if extension supported</returns>
        public virtual bool IsSupported(string extension)
        {
            if (APIExtensions is null)
            {// Fetch the extensions
                var extList = from str in Populate_Extensions() select str.ToLowerInvariant();
                APIExtensions = new HashSet<string>(extList);
            }

            return APIExtensions.Contains(extension.ToLowerInvariant());
        }

        public abstract bool IsSupported(PixelDescriptor pixelDescriptor);
        #endregion

        #region State Machine
        #endregion

        #region Error Handling
        /// <summary>
        /// Checks for errors reported by the graphics driver.
        /// </summary>
        public abstract bool ErrorCheck(out string Message);
        #endregion

        #region Event Loop
        public virtual void Think()
        {
            ErrorCheck(out _);

            while (_initialization_queue.TryDequeue(out EngineObject obj))
            {
                obj.TryInitialize();
            }

            while (_update_queue.TryDequeue(out EngineObject obj))
            {
                obj.TryUpdate();
            }

            while (_release_queue.TryDequeue(out EngineObject obj))
            {
                obj.TryRelease();
            }

            while (_deferred_action_queue.TryDequeue(out Action action))
            {
                action.Invoke();
            }

            _try_process_job_polling_queue();
        }
        #endregion

        #region Object Management
        public void LazyInit(EngineObject obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _initialization_queue.Enqueue(obj);
        }

        public void LazyRelease(EngineObject obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _release_queue.Enqueue(obj);
        }

        public void LazyUpdate(EngineObject obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _update_queue.Enqueue(obj);
        }

        private bool _try_process_object_queue(ref ConcurrentQueue<EngineObject> queue, Action<EngineObject> Callback)
        {
            while (queue.TryDequeue(out var obj))
            {
                Callback.Invoke(obj);
            }

            return true;
        }
        #endregion

        #region Deferred Actions
        public void LazyInvoke(Action action)
        {
            _deferred_action_queue.Enqueue(action);
        }

        private bool _try_process_delegate_queue(ref ConcurrentQueue<IEngineDelegate> queue)
        {
            while (queue.TryDequeue(out var @delegate))
            {
                @delegate?.TryInvoke();
            }

            return true;
        }
        #endregion

        #region Job Management
        public void FinishJob(EngineJob job)
        {
            if (job is null)
            {
                throw new ArgumentNullException(nameof(job));
            }

            _job_polling_queue.Enqueue(job);
        }

        private void _try_process_job_polling_queue()
        {
            var continuityQueue = new ConcurrentQueue<EngineJob>(_job_polling_queue);

            // Process the job queue
            while (_job_polling_queue.TryDequeue(out EngineJob job))
            {
                if (!job.Poll())
                {
                    continuityQueue.Enqueue(job);
                }
                else
                {
                    job.Finish();
                }
            }

            _job_polling_queue = continuityQueue;
        }
        #endregion

        #region Texture Uploading
        public virtual PixelDescriptor Get_Default_Texture_Internal_Pixel_Layout() => PixelDescriptor.Rgba;
        //public abstract IGpuTask Upload_Texture(TextureMetadata Metadata, ReadOnlyMemory<byte> pixelData, PixelDescriptor GpuLayout, ITexture Target, out int ID);
        #endregion

        #region Frame Management
        public abstract void BeginFrame();
        public abstract void EndFrame();
        #endregion

        #region Binding
        /// <summary>
        /// <inheritdoc cref="IRenderEngine.Bind(IEngineBindable)"/>
        /// </summary>
        public virtual void Bind(IEngineBindable Bindable)
        {
            if (Bindable is object)
            {
                Bindable.Bind();
                Bindable.Bound();
                ErrorCheck(out _);
            }
        }

        /// <summary>
        /// <inheritdoc cref="IRenderEngine.Unbind(IEngineBindable)"/>
        /// </summary>
        public virtual void Unbind(IEngineBindable Bindable)
        {
            if (Bindable is object)
            {
                Bindable.Unbind();
                Bindable.Unbound();
                ErrorCheck(out _);
            }
        }
        #endregion

        #region Capabilies

        /// <summary>
        /// <inheritdoc cref="IRenderEngine.Enable(EEngineCap)"/>
        /// </summary>
        public abstract void Enable(EEngineCap Capability);

        /// <summary>
        /// <inheritdoc cref="IRenderEngine.Disable(EEngineCap)"/>
        /// </summary>
        public abstract void Disable(EEngineCap Capability);


        /// <summary>
        /// <inheritdoc cref="IRenderEngine.Enable(IEngineCapability)"/>
        /// </summary>
        public abstract void Enable(IEngineCapability Capability);

        /// <summary>
        /// <inheritdoc cref="IRenderEngine.Disable(IEngineCapability)"/>
        /// </summary>
        public abstract void Disable(IEngineCapability Capability);
        #endregion

        #region Textures
        public abstract void Bind(ITexture Texture);

        public abstract void Unbind(ITexture Texture);
        #endregion

        #region Framebuffers

        public abstract void Bind_Framebuffer(FrameBuffer frameBuffer, EBufferAccess Mode);

        public abstract void Unbind_Framebuffer(FrameBuffer frameBuffer);

        /// <summary>
        /// Performs a blitting operation between the current read framebuffer into the current write framebuffer
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Dest"></param>
        /// <param name="Mask"></param>
        /// <param name="Filter"></param>
        public abstract void Blit(Rectangle Source, Rectangle Dest);
        #endregion

    }
}
