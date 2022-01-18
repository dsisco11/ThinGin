using System;

using ThinGin.Core.Exceptions;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Engine.Types;

namespace ThinGin.Core.Common.Types
{
    /// <summary>
    /// Provides a generic buffer object
    /// </summary>
    public abstract class BufferObject : EngineObject, IBufferObject, IEngineBindable
    {
        #region Values
        protected int _handle = 0;
        protected int _length = 0;
        protected IntPtr _address = IntPtr.Zero;
        protected bool _bind_state = false;
        #endregion

        #region Properties
        /// <summary> <inheritdoc cref="IBufferObject.Handle"/> </summary>
        public int Handle => _handle;

        /// <summary> <inheritdoc cref="IBufferObject.Length"/> </summary>
        public virtual int Length => _length;

        /// <summary> <inheritdoc cref="IBufferObject.Address"/> </summary>
        public IntPtr Address => _address;
        #endregion

        #region Constructors
        public BufferObject(IRenderEngine Engine) : base(Engine)
        {
        }
        #endregion

        #region Binding
        /// <summary>
        /// <inheritdoc cref="IBufferObject.Bind"/>
        /// </summary>
        /// <returns>Buffer handle</returns>
        public abstract void Bind();

        /// <summary>
        /// <inheritdoc cref="IBufferObject.Unbind"/>
        /// </summary>
        public abstract void Unbind();

        public void Bound()
        {
            _bind_state = true;
        }

        public void Unbound()
        {
            _bind_state = false;
        }
        #endregion

        #region Direct Memory Access
        /// <summary>
        /// Attempts to get a Direct Memory Access address where the client can access the buffers data from
        /// </summary>
        /// <param name="dmaAddress"></param>
        /// <returns>Success</returns>
        /// <exception cref="ThinGinException"></exception>
        public abstract bool TryMap(EBufferAccess Access, out IntPtr dmaAddress);
        public abstract bool TryUnmap();
        #endregion
    }
}
