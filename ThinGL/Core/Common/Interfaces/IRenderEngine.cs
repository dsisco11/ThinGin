using ThinGin.Core.Rendering;
using System.Drawing;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Engine;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Engine.Common;
using System;
using ThinGin.Core.Common.Cameras;
using ThinGin.Core.World;
using ThinGin.Core.Common.Units;

namespace ThinGin.Core.Common.Interfaces
{
    public interface IRenderEngine
    {
        #region Properties
        IGraphicsImplementation Provider { get; }
        EngineCompatabilityList Compatability { get; }
        #endregion

        #region Settings
        bool AutoCompressTextures { get; set; }
        bool AutoGenerateMipMaps { get; set; }
        #endregion

        #region Accessors
        /// <summary>
        /// A convenient default camera for you to use...
        /// </summary>
        Camera Camera { get; }
        WorldManager World { get; }
        SpatialOrientation Space { get; }
        #endregion

        #region Events
        event Action OnViewportResized;
        #endregion

        #region Initialization
        void Initialize();
        #endregion

        #region Viewport
        /// <summary>
        /// Returns the aspect ratio (width/height) for the viewport
        /// </summary>
        /// <returns></returns>
        public float Get_AspectRatio();
        public Rectangle Get_Viewport();
        public void Set_Viewport(Rectangle area);
        #endregion

        #region Extensions
        bool IsSupported(string extension);
        #endregion

        #region Object Management
        void Think();

        /// <summary>
        /// Queues the specified object for release on the engine side.
        /// </summary>
        /// <param name="obj"></param>
        void LazyRelease(EngineObject obj);

        /// <summary>
        /// Queues the specified object for lazy initialization by the engine, meaning that its initialization logic will be triggered when the engine next enters its processing loop.
        /// </summary>
        /// <param name="obj"></param>
        void LazyInit(EngineObject obj);

        /// <summary>
        /// Queues the specified resource object for a delayed update by the engine.
        /// </summary>
        /// <param name="obj"></param>
        void LazyUpdate(EngineObject obj);
        #endregion

        #region Lazy Delegates
        void LazyInvoke(Action @delegate);
        #endregion

        #region Jobs
        void FinishJob(EngineJob job);
        #endregion

        #region Frame Management
        void BeginFrame();
        void EndFrame();
        #endregion

        #region Textures
        TextureCache TextureCache { get; }
        /// <summary>
        /// Returns the default GPU pixel layout recommended by the engine implementation.
        /// </summary>
        PixelDescriptor Get_Default_Texture_Internal_Pixel_Layout();

        /// <summary>
        /// Returns whether the specified <paramref name="PixelLayout"/> is a pixel data arrangement the current engine supports.
        /// </summary>
        /// <param name="PixelLayout"></param>
        /// <returns></returns>
        bool IsSupported(PixelDescriptor PixelLayout);
        //IGpuTask Upload_Texture(TextureMetadata Metadata, ReadOnlyMemory<byte> pixelData, PixelDescriptor GpuLayout, ITexture Target, out int ID);

        #endregion

        #region Errors
        /// <summary>
        /// Checks the error state of the graphics implementation
        /// </summary>
        bool ErrorCheck(out string Message);
        #endregion

        #region Binding
        /// <summary>
        /// <para>Being 'bound' is in reference to the gpu state machine. Binding changes which object some set of gpu calls references, such as a mesh, texture, shader, or other object.</para>
        /// </summary>
        void Bind(IEngineBindable Bindable);

        /// <summary>
        /// <para>Being 'bound' is in reference to the gpu state machine. Binding changes which object some set of gpu calls references, such as a mesh, texture, shader, or other object.</para>
        /// </summary>
        void Unbind(IEngineBindable Bindable);
        #endregion

        #region Capabilities
        /// <summary>
        /// Enables the specified engine capability (if needed)
        /// </summary>
        void Enable(EEngineCap Capability);

        /// <summary>
        /// Disables the specified engine capability (if needed)
        /// </summary>
        void Disable(EEngineCap Capability);

        /// <summary>
        /// Enables the given engine capability (if needed)
        /// </summary>
        void Enable(IEngineCapability Capability);

        /// <summary>
        /// Disables the given engine capability (if needed)
        /// </summary>
        void Disable(IEngineCapability Capability);
        #endregion

        #region Texturing State
        void Bind(ITexture Texture);
        void Unbind(ITexture Texture);
        #endregion

        #region Framebuffers

        void Bind_Framebuffer(FrameBuffer frameBuffer, EBufferAccess BufferMode);
        void Unbind_Framebuffer(FrameBuffer frameBuffer);
        void Blit(Rectangle Source, Rectangle Dest);
        #endregion
    }
}