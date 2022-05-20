
using System;
using System.Drawing;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;

using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Rendering;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.Framebuffers
{
    /// <summary>
    /// Redirect the rendering output to a frame buffer
    /// </summary>
    /// http://www.gamedev.net/reference/articles/article2331.asp
    /// http://troylawlor.com/tutorial-fbo.html
    /// http://www.gamedev.net/community/forums/topic.asp?topic_id=364174
    /// 
    /// http://www.seas.upenn.edu/~cis665/fbo.htm#error2
    /// 
    /// 
    /// http://www.opengl.org/registry/specs/ARB/wgl_render_texture.txt
    /// http://oss.sgi.com/projects/ogl-sample/registry/EXT/framebuffer_object.txt
    /// 
    /// 
    /// http://www.coder-studio.com/index.php?page=tutoriaux_aff&code=c_13
    /// 
    /// 
    /// http://ogltotd.blogspot.com/2006/12/render-to-texture.html
    /// http://www.opentk.com/doc/graphics/frame-buffer-objects
    /// 
    /// http://www.songho.ca/opengl/gl_fbo.html
    /// http://www.songho.ca/opengl/gl_pbo.html
    /// 
    public class GLFrameBuffer : GBuffer
    {
        #region STATIC
        /// <summary>
        /// Maximum allowed size
        /// </summary>
        static public Size Get_MaxSize()
        {
            GL.GetRenderbufferParameter(RenderbufferTarget.Renderbuffer, RenderbufferParameterName.RenderbufferWidth, out int width);
            GL.GetRenderbufferParameter(RenderbufferTarget.Renderbuffer, RenderbufferParameterName.RenderbufferHeight, out int height);

            return new Size(width, height);
        }
        #endregion

        #region Values
        private Mesh _cached_blit_quad = null;
        #endregion

        #region Accessors
        public float ClearDepthValue;
        public int ClearStencilValue;
        #endregion

        #region Slot Accessors
        protected bool HasColor => Slots.ContainsKey((int)EFramebufferAttachmentType.ColorBuffer);
        protected bool HasDepth => Slots.ContainsKey((int)EFramebufferAttachmentType.DepthBuffer);
        protected bool HasStencil => Slots.ContainsKey((int)EFramebufferAttachmentType.StencilBuffer);

        protected WeakReference<IGBufferAttachment> ColorSlotRef => Slots[(int)EFramebufferAttachmentType.ColorBuffer].Count > 0 ? Slots[(int)EFramebufferAttachmentType.ColorBuffer][0] : null;
        protected WeakReference<IGBufferAttachment> DepthSlotRef => Slots[(int)EFramebufferAttachmentType.DepthBuffer].Count > 0 ? Slots[(int)EFramebufferAttachmentType.DepthBuffer][0] : null;
        protected WeakReference<IGBufferAttachment> StencilSlotRef => Slots[(int)EFramebufferAttachmentType.StencilBuffer].Count > 0 ? Slots[(int)EFramebufferAttachmentType.StencilBuffer][0] : null;


        protected IGBufferAttachment ColorSlot => ColorSlotRef?.TryGetTarget(out IGBufferAttachment attachment) ?? false ? attachment : null;
        protected IGBufferAttachment DepthSlot => DepthSlotRef?.TryGetTarget(out IGBufferAttachment attachment) ?? false ? attachment : null;
        protected IGBufferAttachment StencilSlot => StencilSlotRef?.TryGetTarget(out IGBufferAttachment attachment) ?? false ? attachment : null;
        #endregion

        #region Constructors
        public GLFrameBuffer(EngineInstance Engine, Size size) : base(Engine, size)
        {
        }

        /// <summary>
        /// Create the RenderBuffer
        /// </summary>
        /// <param name="size">Desired size</param>
        public GLFrameBuffer(EngineInstance Engine, Size size, FrameBufferOptions Options) : base(Engine, size)
        {// XXX: TODO: Consider doing lazy creation of this to be more flexible surrounding OpenGL contexts

            if (!Engine.IsSupported("ext_framebuffer_object")) throw new Exception("Framebuffer objects are not supported by the graphics driver!");

            if (Options.EnableRead && Options.EnableWrite) AccessMode = EBufferAccess.ReadWrite;
            else if (Options.EnableWrite) AccessMode = EBufferAccess.Write;
            else if (Options.EnableRead) AccessMode = EBufferAccess.Read;


            if (Options.ColorBuffer)
            {
                if (Options.EnableMSAA)
                {
                    IGBufferAttachment attach = new GLRenderBufferAttachment(Engine, this, RenderbufferStorage.Rgba8, new RenderBufferOptions(true, Options.MSAA_Samples));
                    Attachments.Add(attach);
                }
                else
                {
                    IGBufferAttachment attach = new GLTextureBufferAttachment(Engine, this);
                    Attachments.Add(attach);
                }
            }


            if (Options.DepthBuffer && Options.StencilBuffer && !Options.SeperateDepthStencil)
            {// We want both depth and stencil buffers and we dont want the seperated
                IGBufferAttachment attach = new GLRenderBufferAttachment(Engine, this, RenderbufferStorage.DepthStencil, new RenderBufferOptions());
                //attach.TypeId = EFramebufferAttachmentType.ColorBuffer;
                Attachments.Add(attach);
            }
            else
            {
                if (Options.StencilBuffer)
                {
                    IGBufferAttachment attach = new GLRenderBufferAttachment(Engine, this, RenderbufferStorage.StencilIndex8, new RenderBufferOptions());
                    //attach.TypeId = EFramebufferAttachmentType.StencilBuffer;
                    Attachments.Add(attach);
                }

                if (Options.DepthBuffer)
                {
                    IGBufferAttachment attach = new GLRenderBufferAttachment(Engine, this, RenderbufferStorage.DepthComponent32, new RenderBufferOptions());
                    //attach.TypeId = EFramebufferAttachmentType.DepthBuffer;
                    Attachments.Add(attach);
                }
            }
        }
        #endregion

        #region Logical
        /// <summary>
        /// Clears any and all applicable buffers
        /// </summary>
        public override void Clear()
        {
            //GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            if (HasColor)
            {
                unsafe
                {
                    uint packed = ClearColorValue.Pack();
                    GL.ClearBuffer(ClearBuffer.Color, 0, &packed);
                }

                //GL.ClearColor(_clearColorValue.R, _clearColorValue.G, _clearColorValue.B, _clearColorValue.A);
                //GL.Clear(ClearBufferMask.ColorBufferBit);
            }

            if (HasDepth || HasStencil)
            {
                GL.ClearBuffer(ClearBufferCombined.DepthStencil, 0, 0f, 0);
            }

            if (Engine.ErrorCheck(out string errMsg))
            {
                System.Diagnostics.Trace.TraceError(errMsg);
            }
        }
        #endregion

        #region Errors
        /// <summary>
        /// State of the frame buffer
        /// </summary>
        public FramebufferErrorCode Get_Status()
        {
            switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: The framebuffer is complete and valid for rendering.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteAttachmentExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: There are no attachments.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: Attachments are of different size. All attachments must have the same width and height.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: The color attachments have different format. All color attachments must have the same format.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferUnsupportedExt:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: This particular FBO configuration is not supported by the implementation.");
                        break;
                    }
                default:
                    {
                        System.Diagnostics.Trace.TraceError("FBO: Status unknown. (yes, this is really bad.)");
                        break;
                    }
            }

            return GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
        }
        #endregion

        #region Blitting
        /// <summary>
        /// Handles blitting process
        /// </summary>
        /// <param name="E"></param>
        /// <param name="area"></param>
        public override void Blit(Rectangle area)
        {
            if (ColorSlot is GLTextureBufferAttachment textureBuffer)
            {
                if (BindState)
                {
                    Unbind();
                }

                if (_cached_blit_quad is null || !_cached_blit_quad.Bounds.Equals(area))
                {
                    _cached_blit_quad = Mesh.Create_Textured_Quad(Engine, area);
                }

                // Get the texture that the framebuffer draws to
                ITexture drawTexture = textureBuffer.Handle;

                // Bind it
                Engine.Bind(drawTexture);

                _cached_blit_quad.Render();
                return;
            }
            //else if (this.Color_Slot is RenderBuffer renderBuffer)
            //{

            // bind the framebuffer in read mode
            Bind(EBufferAccess.Read);
            //E.Bind_Framebuffer(this.Framebuffer, FramebufferTarget.ReadFramebuffer);

            // blit the framebuffer to the screen!
            Engine.Blit(area, new Rectangle(0, 0, area.Width, area.Height));

            // unbind the framebuffer
            Unbind();

            //}
        }
        #endregion

        #region Pixel Downloading

#if INCLUDE_BITMAP
        /// <summary>
        ///  Downloads the framebuffer from the FBO
        /// </summary>
        /// <param name="flip_y"></param>
        /// <returns></returns>
        public Bitmap Download_As_Bitmap(bool flip_y = true)
        {
            var W = this.Size.Width;
            var H = this.Size.Height;
            Bitmap LOCK_BITMAP = null;
            var doUnbind = !this.BindState;

            if (!this.BindState)
            {
                this.Engine.Bind_Framebuffer(this, FramebufferTarget.ReadFramebuffer);
                //GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            }

            if (this.Color_Slot is TextureBufferAttachment textureBuffer)
            {
                // Get the texture that the framebuffer draws to
                ITexture drawTexture = textureBuffer.Handle;
                W = drawTexture.Size.Width;
                H = drawTexture.Size.Height;

                LOCK_BITMAP = new Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                System.Drawing.Imaging.BitmapData data = LOCK_BITMAP.LockBits(new Rectangle(0, 0, W, H), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.PushAttrib(AttribMask.EnableBit);
                GL.Enable(EnableCap.Texture2D);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, drawTexture.Handle);

                GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                GL.Finish();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.PopAttrib();

                LOCK_BITMAP.UnlockBits(data);
            }
            else if (this.Color_Slot is RenderBufferAttachment renderBuffer)
            {
                LOCK_BITMAP = new Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                System.Drawing.Imaging.BitmapData data = LOCK_BITMAP.LockBits(new Rectangle(0, 0, W, H), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.ReadPixels(0, 0, W, H, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                GL.Finish();
                LOCK_BITMAP.UnlockBits(data);
            }

            if (doUnbind)
            {
                GL.ReadBuffer(ReadBufferMode.Front);
                this.Engine.Unbind_Framebuffer(this);
            }

            if (flip_y)
            {
                LOCK_BITMAP?.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

            return LOCK_BITMAP;
        }
#endif

        /// <summary>
        /// <inheritdoc cref="GBuffer.Download"/>
        /// </summary>
        public override byte[,,] Download()
        {
            var W = Size.Width;
            var H = Size.Height;
            var doUnbind = !BindState;
            byte[,,] pixelBuffer = new byte[W, H, 4];
            int bufferLen = pixelBuffer.Length;

            if (!BindState)
            {
                Engine.Bind_Framebuffer(this, EBufferAccess.Read);
                //GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            }

            var Color_Slot = ColorSlot;
            if (Color_Slot is GLTextureBufferAttachment textureBuffer)
            {
                // Get the texture that the framebuffer draws to
                ITexture drawTexture = textureBuffer.Handle;

                GL.PushAttrib(AttribMask.EnableBit);
                GL.Enable(EnableCap.Texture2D);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, drawTexture.Handle);

                GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Bgra, PixelType.UnsignedByte, pixelBuffer);

                GL.Finish();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.PopAttrib();
            }
            else if (Color_Slot is GLRenderBufferAttachment renderBuffer)
            {
                GL.ReadPixels(0, 0, W, H, PixelFormat.Bgra, PixelType.UnsignedByte, pixelBuffer);
                GL.Finish();
            }

            if (doUnbind)
            {
                GL.ReadBuffer(ReadBufferMode.Front);
                Engine.Unbind_Framebuffer(this);
            }

            return pixelBuffer;
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => { GL.Ext.GenFramebuffers(1, out _handle); });
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(Handle, GL.Ext.DeleteFramebuffer);
        #endregion

    }
}