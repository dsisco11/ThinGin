
using System;
using System.Collections.Generic;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Rendering;
using ThinGin.Core.Exceptions;
using ThinGin.OpenGL.Common.Textures;
using ThinGin.OpenGL.Common.Engine;
using ThinGin.Core.Common.Types;
using System.Numerics;
using System.Diagnostics;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.Common.Engine.Delegates;

namespace ThinGin.OpenGL.Common
{
    /// <summary>
    /// <inheritdoc cref="EngineInstance"/>
    /// </summary>
    public abstract class GLEngineBase : EngineInstance
    {
        #region Properties
        protected StateMachine State;
        private Version glVersion = null;
        #endregion

        #region Constructors
        public GLEngineBase(object Context) : base(Context)
        {
            State = new StateMachine();
            Initialize();
        }
        #endregion

        #region Accessors
        #endregion

        #region Initialization
        public override void Initialize()
        {
            base.Initialize();
            var major = GL.GetInteger(GetPName.MajorVersion);
            var minor = GL.GetInteger(GetPName.MinorVersion);
            glVersion = new Version(major, minor);

            Compatability = new GLCompatability();

            GL.ClearDepth(1f);
            GL.ClearStencil(0);
            GL.ClearColor(0f, 0f, 0f, 0f);

            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.DepthTest);

            GL.CullFace(CullFaceMode.FrontAndBack);
            GL.FrontFace(FrontFaceDirection.Ccw);

            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PolygonSmooth);

            GL.Color4(1f, 1f, 1f, 1f);
        }
        #endregion

        #region Viewport
        public override void Set_Viewport(Rectangle area)
        {
            base.Set_Viewport(area);
            LazyInvoke(() => { GL.Viewport(Get_Viewport()); });
        }
        #endregion


        #region Extensions
        protected override ICollection<string> Populate_Extensions()
        {
            var count = GL.GetInteger(GetPName.NumExtensions);
            var Extensions = new List<string>(count);
            for (var i = 0; i < count; i++)
            {
                var ext = GL.GetString(StringNameIndexed.Extensions, i);

                ext = ext.ToLowerInvariant();
                if (ext.StartsWith("gl_")) ext = ext.Substring(3);
                if (ext.StartsWith("glx_")) ext = ext.Substring(4);

                Extensions.Add(ext);
            }
            return Extensions;
        }

        public override bool IsSupported(PixelDescriptor pixelDescriptor)
        {
            try
            {
                Get_PixelType(pixelDescriptor);
                Get_PixelFormat(pixelDescriptor);
                return true;
            }
            catch(Exceptions.OpenGLUnsupportedException)
            {
                return false;
            }
        }
        #endregion

        #region State Machine
        #endregion

        #region Error Handling
        /// <summary><inheritdoc cref="IEngine.ErrorCheck(out string)"/> </summary>
        public override bool ErrorCheck(out string Message)
        {
#if DEBUG
            ErrorCode glErr = GL.GetError();
            if (glErr != ErrorCode.NoError)
            {
                Message = glErr.ToString();
                var stack = new StackTrace(skipFrames: 1, fNeedFileInfo: true);

                string div = "====================================";
                System.Diagnostics.Trace.TraceError(div);
                System.Diagnostics.Trace.TraceError($"[OPENGL]::[ERROR]: { glErr }");
                System.Diagnostics.Trace.TraceError(stack.ToString());
                System.Diagnostics.Trace.TraceError(div);
                return true;
            }
#endif
            Message = String.Empty;
            return false;
        }
        #endregion

        #region Texture Uploading
        public abstract PixelType Get_PixelType(PixelDescriptor Layout);
        public abstract PixelFormat Get_PixelFormat(PixelDescriptor Layout);
        public abstract PixelInternalFormat Get_Internal_PixelFormat(PixelDescriptor Layout, bool UseCompression);
        public abstract InternalFormat Get_Compressed_Internal_PixelFormat(PixelDescriptor Layout);
        #endregion

        #region Frame Management
        public override void BeginFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        }

        public override void EndFrame()
        {
        }
        #endregion

        #region Capabilities

        /// <summary>
        /// <inheritdoc cref="IEngine.Enable(EEngineCap)"/>
        /// </summary>
        public override void Enable(EEngineCap cap)
        {
            if (State.Try_Update(cap, true))
            {
                EnableCap glCap = Bridge.Translate(cap);
                GL.Enable(glCap);
            }
        }

        /// <summary>
        /// <inheritdoc cref="IEngine.Disable(EEngineCap)"/>
        /// </summary>
        public override void Disable(EEngineCap cap)
        {
            if (State.Try_Update(cap, false))
            {
                EnableCap glCap = Bridge.Translate(cap);
                GL.Disable(glCap);
            }
        }
        #endregion

        #region Capability Providers
        /// <summary>
        /// <inheritdoc cref="IEngine.Enable(IEngineCapability)"/>
        /// </summary>
        public override void Enable(IEngineCapability Capability)
        {
            Capability?.Enable();
        }

        /// <summary>
        /// <inheritdoc cref="IEngine.Disable(IEngineCapability)"/>
        /// </summary>
        public override void Disable(IEngineCapability Capability)
        {
            Capability?.Disable();
        }
        #endregion

        #region Texturing
        /// <summary>
        /// <inheritdoc cref="IEngine.Bind(ITexture)"/>
        /// </summary>
        public override void Bind(ITexture Texture)
        {
            if (Texture is GLTexture glTex)
            {
                GL.Enable(glTex.Encap);
                GL.BindTexture(glTex.Target, glTex.Handle);
            }
            else
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, Texture.Handle);
            }
        }

        /// <summary>
        /// <inheritdoc cref="IEngine.Unbind(ITexture)"/>
        /// </summary>
        public override void Unbind(ITexture Texture)
        {
            if (Texture is GLTexture glTex)
            {
                GL.BindTexture(glTex.Target, 0);
                GL.Disable(glTex.Encap);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Enable(EnableCap.Texture2D);
            }
        }
        #endregion

        #region Framebuffers
        public override void Bind_Framebuffer(GBuffer frameBuffer, EBufferAccess Mode)
        {
            switch (Mode)
            {
                case EBufferAccess.Read:
                    {
                        if (activeFramebuffer_Read is object && !ReferenceEquals(frameBuffer, activeFramebuffer_Read))
                        {// if the calling buffer is not the one currently bound, then unbind and notify the currently bound buffer
                            activeFramebuffer_Read.Unbound();
                        }

                        activeFramebuffer_Read = frameBuffer;
                        GL.Ext.BindFramebuffer(FramebufferTarget.ReadFramebuffer, frameBuffer.Handle);
                        (frameBuffer as IEngineBindable).Bound();
                    }
                    break;
                case EBufferAccess.Write:
                    {
                        if (activeFramebuffer_Write is object && !ReferenceEquals(frameBuffer, activeFramebuffer_Write))
                        {// if the calling buffer is not the one currently bound, then unbind and notify the currently bound buffer
                            activeFramebuffer_Write.Unbound();
                        }

                        activeFramebuffer_Write = frameBuffer;
                        GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, frameBuffer.Handle);
                        //GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

                        (frameBuffer as IEngineBindable).Bound();
                    }
                    break;
                case EBufferAccess.ReadWrite:
                    {
                        if (activeFramebuffer_Read is object && !ReferenceEquals(frameBuffer, activeFramebuffer_Read))
                        {// if the calling buffer is not the one currently bound, then unbind and notify the currently bound buffer
                            activeFramebuffer_Read.Unbound();
                        }

                        if (activeFramebuffer_Write is object && !ReferenceEquals(frameBuffer, activeFramebuffer_Write))
                        {// if the calling buffer is not the one currently bound, then unbind and notify the currently bound buffer
                            activeFramebuffer_Write.Unbound();
                        }

                        activeFramebuffer_Read = frameBuffer;
                        activeFramebuffer_Write = frameBuffer;

                        GL.Ext.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer.Handle);
                        //GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

                        (frameBuffer as IEngineBindable).Bound();
                    }
                    break;
                default:
                    {
                        throw new ThinGinException($"Attempt to bind framebuffer into unknown buffer mode! (Mode: {Mode})");
                    }
            }
        }

        public override void Unbind_Framebuffer(GBuffer frameBuffer)
        {
            if (ReferenceEquals(frameBuffer, activeFramebuffer_Read) &&
                ReferenceEquals(frameBuffer, activeFramebuffer_Write))
            {
                GL.Ext.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                //GL.DrawBuffer(DrawBufferMode.Back);
                (frameBuffer as IEngineBindable).Unbound();
                activeFramebuffer_Read = activeFramebuffer_Write = null;
            }
            else if (ReferenceEquals(frameBuffer, activeFramebuffer_Write))
            {
                GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
                //GL.DrawBuffer(DrawBufferMode.Back);
                (frameBuffer as IEngineBindable).Unbound();
                activeFramebuffer_Write = null;
            }
            else if (ReferenceEquals(frameBuffer, activeFramebuffer_Read))
            {
                GL.Ext.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
                (frameBuffer as IEngineBindable).Unbound();
                activeFramebuffer_Read = null;
            }
            else
            {
                throw new ThinGinStateException("Attempt to unbind framebuffer object that is not bound (State management error)");
            }
        }

        /// <summary>
        /// Performs a blitting operation between the current read framebuffer into the current write framebuffer
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Dest"></param>
        /// <param name="Mask"></param>
        /// <param name="Filter"></param>
        public override void Blit(Rectangle Source, Rectangle Dest)
        {
            GL.Ext.BlitFramebuffer(Source.Left, Source.Top, Source.Right, Source.Bottom,
                Dest.Left, Dest.Top, Dest.Right, Dest.Bottom,
                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Linear);
        }
        #endregion

    }
}
