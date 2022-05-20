using System;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Rendering;

using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.Framebuffers
{
    internal class GLTextureBufferAttachment : GObject, IGBufferAttachment
    {
        #region Values
        protected readonly WeakReference<GBuffer> _ownerRef;
        protected int Slot;
        #endregion

        #region Properties
        public readonly TextureHandle Handle;
        public bool IsAttached { get; private set; }
        public int TypeId { get; private set; }
        #endregion

        #region Accessors
        public GBuffer Owner => _ownerRef.TryGetTarget(out GBuffer outOwner) ? outOwner : null;
        #endregion

        #region Constructors

        public GLTextureBufferAttachment(EngineInstance Engine, GBuffer Owner) : base(Engine)
        {
            if (Owner is null)
            {
                throw new ArgumentNullException(nameof(Owner));
            }

            _ownerRef = new WeakReference<GBuffer>(Owner);
            IsAttached = false;
            Handle = Engine.Provider.Textures.Create_Handle(Engine);
        }
        #endregion

        #region Attaching

        public void Attach(GBuffer frameBuffer, int slot)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);

            if (EnsureReady())
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)slot, TextureTarget.Texture2D, Handle.Handle, 0);

                Slot = slot;
                IsAttached = true;
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
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
                if (Handle.EnsureReady())
                {
                    GL.BindTexture(TextureTarget.Texture2D, Handle.Handle);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, Owner.Size.Width, Owner.Size.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

                    GL.BindTexture(TextureTarget.Texture2D, 0);// Unbind to prevent unintentional modifications
                    //GL.Disable(EnableCap.Texture2D);
                }
            });
        }
        public override IEngineDelegate Get_Releaser() => null;
        #endregion
    }
}
