using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Rendering;

using OpenTK.Graphics.OpenGL;
using System;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;

namespace ThinGin.OpenGL.Common.Framebuffers
{
    public class GLRenderBufferAttachment : GObject, IGBufferAttachment
    {
        #region Values
        private int Handle = 0;
        protected WeakReference<GBuffer> _ownerRef;
        protected int Slot;
        #endregion

        #region Properties
        public readonly RenderBufferOptions Options;
        public readonly RenderbufferStorage Storage;

        public bool IsAttached { get; private set; }
        public int TypeId { get; private set; }
        #endregion

        #region Accessors
        public GBuffer Owner => _ownerRef.TryGetTarget(out GBuffer outOwner) ? outOwner : null;
        #endregion

        #region Constructors
        public GLRenderBufferAttachment(IEngine Engine, GBuffer Owner, RenderbufferStorage Storage, RenderBufferOptions Options) : base(Engine)
        {
            _ownerRef = new WeakReference<GBuffer>(Owner);
            this.Storage = Storage;
            this.Options = Options;
        }
        #endregion

        #region Attaching

        public void Attach(GBuffer frameBuffer, int Slot)
        {
            this.Slot = Slot;
            if (Handle == 0)
            {
                Handle = GL.GenRenderbuffer();
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);

                var size = Owner.Size;
                if (Options.MSAA)
                {
                    GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, Options.Samples, Storage, size.Width, size.Height);
                }
                else
                {
                    GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, Storage, size.Width, size.Height);
                }

                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)Slot, TextureTarget.Texture2D, Handle, 0);
            IsAttached = true;
        }

        public void Detach(GBuffer frameBuffer)
        {
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)Slot, TextureTarget.Texture2D, 0, 0);
            Slot = 0;
            IsAttached = false;
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer()
        {
            return new EngineDelegate(() => 
            { 
                GL.GenRenderbuffers(1, out Handle);
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Handle);

                var size = Owner.Size;
                if (Options.MSAA)
                {
                    GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, Options.Samples, Storage, size.Width, size.Height);
                }
                else
                {
                    GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, Storage, size.Width, size.Height);
                }

                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            });
        }
        public override IEngineDelegate Get_Releaser()
        {
            return new EngineReleaser<int>(Handle, OpenTK.Graphics.OpenGL.GL.DeleteRenderbuffer);
        }
        #endregion
    }
}
