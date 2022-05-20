using System;
using ThinGin.Core.Common.Interfaces;

using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Common.Textures;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Common.Engine;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.OpenGL.Common.Types;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.Textures
{
    /// <summary>
    ///  Stores the actual OpenGL texture instance and manages proper disposal once all users have released it.
    /// </summary>
    public abstract class GLTexture : Texture
    {
        #region Values
        private DataTransfer _transferTask = null;
        private byte[] _transient_pixel_data = null;
        #endregion

        #region Settings
        public TextureWrapMode WrapS = TextureWrapMode.Repeat;
        public TextureWrapMode WrapT = TextureWrapMode.Repeat;

        public TextureMinFilter MinFilter = TextureMinFilter.Linear;
        public TextureMagFilter MagFilter = TextureMagFilter.Linear;

        public TextureEnvMode EnvMode = TextureEnvMode.Blend;

        public bool UseCompression = false;
        public bool EnableMipMaps = false;
        #endregion

        #region Properties
        /// <summary> <inheritdoc cref="ITexture.handle"/> </summary>
        public override int Handle => _id;
        #endregion

        #region Accessors
        public abstract EnableCap Encap { get; }
        public abstract TextureTarget Target { get; }
        //public abstract TextureTarget Target { get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GPU texture
        /// </summary>
        public GLTexture(EngineInstance Engine, PixelDescriptor GpuLayout) : base(Engine, GpuLayout)
        {
            UseCompression = Engine.AutoCompressTextures;
            EnableMipMaps = Engine.AutoGenerateMipMaps;
        }
        #endregion

        #region Initialization
        public override bool TryLoad(TextureMetadata Metadata, byte[] RawPixels)
        {
            try
            {
                this.Metadata = Metadata;
                _transient_pixel_data = RawPixels;
                Invalidate();
            }
            catch
            {
                return false;
            }

            return true;
        }

        protected void Set_Params()
        {
            GL.BindTexture(Target, _id);

            GL.TexParameter(Target, TextureParameterName.GenerateMipmap, EnableMipMaps ? 1 : 0);
            if (Priority > 0)
            {
                GL.TexParameter(Target, TextureParameterName.TexturePriority, Priority);
            }

            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)EnvMode);

            // Texture Wrapping
            GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)WrapS);
            GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)WrapT);

            // Texture Filtering
            GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)MinFilter);
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)MagFilter);
        }
        #endregion

        #region Uploading
        protected abstract void Upload(byte[] RawPixels, int miplevel);
        protected abstract void UploadSub(byte[] RawPixels, int xoffset, int yoffset, int zoffset, int miplevel);
        #endregion

        #region GpuTask
        private DataTransfer _begin_upload_task(ReadOnlyMemory<byte> pixelData)
        {
            int Bpp = Metadata.Layout.BitDepth;
            int Width = Math.Max(Metadata.Width, 1);
            int Height = Math.Max(Metadata.Height, 1);
            int Depth = Math.Max(Metadata.Depth, 1);
            int dataLength = Bpp * Width * Height * Depth;

            // Initiate PBO upload process
            var PBO = new GLPixelBuffer(Engine, EPixelBufferMode.Unpack, dataLength);
            PBO.TryMap(Core.Common.Enums.EBufferAccess.Write, out IntPtr pboAddress);
            long pboLength = PBO.Length;

            // Create a synchronizer so we know when the DMA transfer is done
            var fence = new GLSyncPrimitive(Engine);
            var job = new DataTransfer(Engine, fence, PBO, pixelData);
            job.Start();

            Engine.ErrorCheck(out _);
            return job;
        }

        private void _on_data_transfer_complete()
        {
            IBufferObject pbo = _transferTask.Buffer;
            GL.Enable(Encap);
            pbo.Bind();
            GL.BindTexture(Target, _id);

            // Tell the GPU to attach the texture to the PBO data
            UploadSub(null, 0, 0, 0, 0);

            pbo.Unbind();
            GL.BindTexture(Target, 0);
            GL.Disable(Encap);
            // Release the object
            pbo.Dispose();

            _transferTask?.Dispose();
            _transferTask = null;
            _transient_pixel_data = null;
        }

        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => GL.GenTextures(1, out _id));
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(_id, GL.DeleteTexture);
        public override IEngineDelegate Get_Updater()
        {
            return new EngineDelegate(() =>
            {
                // If a previous task is still ongoing, then... cancel it...
                if (_transferTask is object)
                {
                    _transferTask.Cancel();
                    _transferTask = null;
                }

                // Start a transfer task for the new transient pixel data
                var DataPtr = new ReadOnlyMemory<byte>(_transient_pixel_data);

                _transferTask = _begin_upload_task(DataPtr);//Engine.Upload_Texture(Metadata, RawPixels, GpuLayout, this, out _id);
                //_transferTask.Set_Handle(_transient_pixel_data);
                _transferTask.Complete += _on_data_transfer_complete;

                // While the DMA transfer is ongoing we will continue on and create our texture
                // Create the texture to dump the data into on the GPU side
                Set_Params();
                Upload(null, 0);
            });
        }
        #endregion

    }
}
