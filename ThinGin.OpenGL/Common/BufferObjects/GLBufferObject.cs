using System;

using ThinGin.Core.Common.Interfaces;

using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;

namespace ThinGin.OpenGL.Common
{
    /// <summary>
    /// </summary>
    public abstract class GLBufferObject : DataBufferObject
    {
        #region Values
        private int _uploaded = 0;
        protected BufferUsageHint _usage = BufferUsageHint.StaticDraw;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        public byte[] Data => _data;
        public BufferUsageHint Usage => _usage;
        public abstract BufferTarget Target { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// </summary>
        public GLBufferObject(IRenderEngine Engine) : base(Engine)
        {
        }
        #endregion

        #region Binding
        /// <summary>
        /// <inheritdoc cref="IBufferObject.Bind"/>
        /// </summary>
        /// <returns>Buffer handle</returns>
        public override void Bind()
        {
            EnsureReady();
            GL.BindBuffer(Target, _handle);
        }

        /// <summary>
        /// <inheritdoc cref="IBufferObject.Unbind"/>
        /// </summary>
        public override void Unbind()
        {
            GL.BindBuffer(Target, 0);
        }
        #endregion

        #region Direct Memory Access
        public override bool TryMap(EBufferAccess Access, out IntPtr dmaAddress)
        {
            if (_address != IntPtr.Zero)
            {
                dmaAddress = _address;
                return true;
            }

            BufferAccess accessMode = Access switch
            {
                EBufferAccess.Read => BufferAccess.ReadOnly,
                EBufferAccess.Write => BufferAccess.WriteOnly,
                EBufferAccess.ReadWrite => BufferAccess.ReadWrite,
                _ => throw new NotImplementedException(Access.ToString())
            };

            Bind();
            _address = GL.MapBuffer(Target, accessMode);
            Unbind();

            dmaAddress = _address;
            if (_address == IntPtr.Zero)
            {// XXX: TODO: Implement handling for GL_OUT_OF_MEMORY_ERROR and other such scenarios
                return false;
            }

            return true;
        }

        public override bool TryUnmap()
        {
            bool res = GL.UnmapBuffer(Target);
            if (res)
            {
                _address = IntPtr.Zero;
            }
            return res;
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer()
        {
            return new EngineDelegate(() =>
            {
                _handle = GL.GenBuffer();
                if (!_bind_state) GL.BindBuffer(Target, _handle);
                //GL.BufferData(Target, _length, IntPtr.Zero, _usage);

                if (!_bind_state) GL.BindBuffer(Target, 0);// If the buffer is not supposed to be bound right now then unbind it!

                Engine.ErrorCheck(out _);
            });
        }

        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(_handle, GL.DeleteBuffer);

        public override IEngineDelegate Get_Updater() => new EngineDelegate(() =>
        {
            if (System.Threading.Interlocked.Increment(ref _uploaded) > 1)
            {
                //_usage = BufferUsageHint.DynamicDraw;// If we change the buffers data after first upload then we switch to dynamic mode under the assumption that if they changed it once, they probably intend to do it again!
            }
            unsafe
            {
                if (_handle != 0 && Data is object)
                {
                    if (!_bind_state) GL.BindBuffer(Target, _handle);

                    GL.BufferData(Target, _length, ref Data[0], _usage);
                    
                    if (!_bind_state) GL.BindBuffer(Target, 0);// If the buffer is not supposed to be bound right now then unbind it!
                    Engine.ErrorCheck(out _);
                }
            }
        });
        #endregion
    }
}
