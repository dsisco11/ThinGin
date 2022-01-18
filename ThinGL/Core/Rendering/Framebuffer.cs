
using System;
using System.Collections.Generic;
using System.Drawing;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Types;
using ThinGin.Core.Common.Engine.Types;

namespace ThinGin.Core.Rendering
{
    /// <summary>
    /// Redirect the rendering output to a frame buffer
    /// </summary>
    public abstract class FrameBuffer : EngineObject, IEngineBindable
    {
        #region Values
        protected int _handle = 0;
        /// <summary> 
        /// This is used by engine implementations to store contextual information about how to handle the resource.
        /// </summary>
        public object CustomContext { get; set; }
        /// <summary>
        /// TRUE when the FBO is actively bound
        /// </summary>
        protected bool BindState = false;
        private bool HasPendingAttachments = false;

        protected readonly Dictionary<int, List<WeakReference<IFrameAttachment>>> Slots = new Dictionary<int, List<WeakReference<IFrameAttachment>>>();
        #endregion

        #region Properties

        /// <summary>
        /// Handle to the buffer object on the GPU
        /// </summary>
        public int Handle { get => _handle; protected set => _handle = value; }
        public EBufferAccess AccessMode { get; protected set; } = EBufferAccess.ReadWrite;
        /// <summary> Size of the FrameBuffer </summary>
        public readonly Size Size;

        public List<IFrameAttachment> Attachments = new List<IFrameAttachment>(2);
        public Rgba ClearColorValue;
        #endregion

        #region Constructors
        /// <summary>
        /// Create the RenderBuffer
        /// </summary>
        /// <param name="size">Desired size</param>
        public FrameBuffer(IRenderEngine Engine, Size size) : base(Engine)
        {
            Size = size;
        }

        #endregion

        #region Attachments
        public void Attach(IFrameAttachment item)
        {
            Attachments.Add(item);
            if (BindState)
            {
                Process_Attachment(item);
            }
            else
            {
                HasPendingAttachments = true;
            }
        }

        protected void Process_Attachment(IFrameAttachment item)
        {
            // Framebuffer attachments come in different types, like colorbuffers, depthbuffers, or even stencilbuffers!
            // So IFrameAttachment object provide some sort of type identifier number for the base library to use,
            // this type number ideally will mean something to the implementor but for us its just a way to differentiate the different attachment categorys.
            // So now we will take that TypeID and group this attachment with the others of the same type, this way we can tell how it attaches to the framebuffer (eg; colorslot-1 vs colorslot-5)
            int Ty = item.TypeId;

            // If we arent tracking attachments of this type yet then start doing so
            if (!Slots.ContainsKey(item.TypeId))
            {
                Slots.Add(item.TypeId, new List<WeakReference<IFrameAttachment>>(1));
            }

            // Determine what slot to insert the attachment at
            int slotNo = Slots[item.TypeId].Count;
            Slots[item.TypeId].Add(new WeakReference<IFrameAttachment>(item));
            item.Attach(this, slotNo);
        }
        #endregion

        #region Logical
        /// <summary>
        /// Clears any and all applicable buffers
        /// </summary>
        public abstract void Clear();
        #endregion

        #region IEngineBindable
        /// <summary>
        /// Make the Render buffer current 
        /// </summary>
        public void Bind()
        {
            if (BindState)
            {
                System.Diagnostics.Trace.TraceWarning("Attempting to bind a framebuffer which is already bound!");
                return;
            }

            if (!EnsureReady())
            {
                System.Diagnostics.Trace.TraceWarning("[FRAMEBUFFER]::[BIND]: Attempt to bind framebuffer before it is ready!");
                return;
            }

            Engine.Bind_Framebuffer(this, AccessMode);

            if (HasPendingAttachments)
            {// Process and attach any registered but unattached objects
                foreach (IFrameAttachment item in Attachments)
                {
                    Process_Attachment(item);
                }

                HasPendingAttachments = false;
            }
        }

        /// <summary>
        /// Make the Render buffer current 
        /// </summary>
        public void Bind(EBufferAccess Access)
        {
            if (BindState)
            {
                System.Diagnostics.Trace.TraceWarning("Attempting to bind a framebuffer which is already bound!");
                return;
            }

            if (!EnsureReady())
            {
                System.Diagnostics.Trace.TraceWarning("[FRAMEBUFFER]::[BIND]: Attempt to bind framebuffer before it is ready!");
                return;
            }

            Engine.Bind_Framebuffer(this, Access);

            if (HasPendingAttachments)
            {// Process and attach any registered but unattached objects
                foreach (IFrameAttachment item in Attachments)
                {
                    Process_Attachment(item);
                }
                HasPendingAttachments = false;
            }
        }

        /// <summary>
        /// </summary>
        public void Unbind()
        {
            if (!BindState)
            {
                return;
            }

            Engine.Unbind_Framebuffer(this);
        }

        public void Bound() => BindState = true;
        public void Unbound() => BindState = false;

        #endregion

        #region Blitting
        /// <summary>
        /// Handles blitting process
        /// </summary>
        /// <param name="E"></param>
        /// <param name="area"></param>
        public abstract void Blit(Rectangle area);
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
        ///  Downloads the framebuffers raw pixel data from the graphics provider
        /// </summary>
        public abstract byte[,,] Download();
        #endregion

    }
}