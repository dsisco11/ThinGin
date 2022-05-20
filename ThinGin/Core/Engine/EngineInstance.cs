
using System;
using System.Collections.Concurrent;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using System.Drawing;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine;
using ThinGin.Core.Rendering;
using ThinGin.Core.Common;
using ThinGin.Core.Common.Cameras;
using ThinGin.Core.Common.Units;
using ThinGin.Core.World;
using ThinGin.Core.RenderHardware;

namespace ThinGin.Core.Engine.Common.Core
{
    /// <summary>
    /// Provides abstracted access to generic rendering functionality, manages state to avoid redundant calls.
    /// </summary>
    public abstract class EngineInstance : IEngine
    {
        #region Values
        private int _initialized = 0;
        public readonly object Context;  // move to render manager
        private Rectangle _viewport = Rectangle.Empty;

        protected ConcurrentQueue<EngineJob> _job_polling_queue = new ConcurrentQueue<EngineJob>();
        protected ConcurrentQueue<Action> _deferred_action_queue = new ConcurrentQueue<Action>();

        protected IEngineBindable activeFramebuffer_Read = null;  // move to rhi_state object
        protected IEngineBindable activeFramebuffer_Write = null;  // move to rhi_state object


        // get rid of this, the camer ashould be a component thats registered with the world. Just add a Get_Camera(int num) method to the WorldManager
        private readonly Camera camera;

        private WorldManager world;
        public readonly RenderManager Renderer;

        private SpatialOrientation coords = new SpatialOrientation();// move into WorldManager
        #endregion

        #region Settings
        public bool AutoCompressTextures { get; set; } = false;
        public bool AutoGenerateMipMaps { get; set; } = false;
        #endregion

        #region Accessors
        /// <summary>
        /// <inheritdoc cref="IEngine.Camera"/>
        /// </summary>
        public Camera Camera => camera;
        public WorldManager World { get => world; set => world = value; }
        public SpatialOrientation Space { get => coords; set => coords = value; }

        /// <summary> Graphics platform implementation for the engine </summary>
        [Obsolete]
        public abstract IGraphicsImplementation Provider { get; }
        #endregion

        #region Events
        public event Action OnViewportResized;
        #endregion

        #region Constructors
        public EngineInstance(object Context)
        {
            this.Context = Context;
            Renderer = new RenderManager(this);

            TextureCache = new TextureCache();
            camera = new Camera(this);
        }
        #endregion

        #region Initialization
        public virtual void Initialize()
        {
        }
        #endregion

        #region Viewport
        public float Get_AspectRatio() => Math.Max(1f, (float)_viewport.Width) / Math.Max(1f, (float)_viewport.Height);
        public Rectangle Get_Viewport() => _viewport;
        public virtual void Set_Viewport(Rectangle area)
        {
            _viewport = area;
            OnViewportResized?.Invoke();
        }
        #endregion

        #region Error Handling
        /// <summary> <inheritdoc cref="IRHIDriver.ErrorCheck(out string)"/> </summary>
        public bool ErrorCheck(out string Message) => Renderer.RHI.ErrorCheck(out Message);
        #endregion

        #region Event Loop
        /// <summary>
        /// Performs frustrum culling for all cameras
        /// </summary>
        protected void InitViews()
        {

        }
        public virtual void Think()
        {
            ErrorCheck(out _);

            Renderer.Process();

            while (_deferred_action_queue.TryDequeue(out Action action))
            {
                action.Invoke();
            }

            _try_process_job_polling_queue();
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

        #region Frame Management
        public abstract void BeginFrame();
        public abstract void EndFrame();
        #endregion

        #region Binding
        /// <summary>
        /// <inheritdoc cref="IEngine.Bind(IEngineBindable)"/>
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
        /// <inheritdoc cref="IEngine.Unbind(IEngineBindable)"/>
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

        #region Capabilities
        /// <summary>
        /// <inheritdoc cref="IEngine.Enable(EEngineCap)"/>
        /// </summary>
        public abstract void Enable(EEngineCap Capability);

        /// <summary>
        /// <inheritdoc cref="IEngine.Disable(EEngineCap)"/>
        /// </summary>
        public abstract void Disable(EEngineCap Capability);


        /// <summary>
        /// <inheritdoc cref="IEngine.Enable(IEngineCapability)"/>
        /// </summary>
        public abstract void Enable(IEngineCapability Capability);

        /// <summary>
        /// <inheritdoc cref="IEngine.Disable(IEngineCapability)"/>
        /// </summary>
        public abstract void Disable(IEngineCapability Capability);
        #endregion

        #region Textures
        public abstract void Bind(ITexture Texture);

        public abstract void Unbind(ITexture Texture);
        #endregion

        #region Framebuffers

        public abstract void Bind_Framebuffer(GBuffer frameBuffer, ERHIAccess Mode);

        public abstract void Unbind_Framebuffer(GBuffer frameBuffer);

        /// <summary>
        /// Performs a blitting operation between the current read framebuffer into the current write framebuffer
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Dest"></param>
        /// <param name="Mask"></param>
        /// <param name="Filter"></param>
        public abstract void Blit(Rectangle Source, Rectangle Dest);// RHI -> CopyBufferRegion
        #endregion

    }
}
