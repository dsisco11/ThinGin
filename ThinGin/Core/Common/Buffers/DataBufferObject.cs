using ThinGin.Core.Common.Data.Interfaces;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Types;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common
{
    /// <summary>
    /// Manages a generic buffer of a specified data layout which can have data uploaded into it.
    /// </summary>
    public abstract class DataBufferObject : BufferObject
    {
        #region Values
        protected byte[] _data = null;
        protected IDataDescriptor _descriptor = null;
        #endregion

        #region Accessors
        /// <summary>
        /// <inheritdoc cref="IBufferObject.Length"/>
        /// </summary>
        public override int Length => _data?.Length ?? 0;

        /// <summary>
        /// Number of elements represented by the buffer data.
        /// </summary>
        public int Count => Length / (_descriptor?.Count ?? 1);
        #endregion

        #region Constructors

        public DataBufferObject(EngineInstance Engine) : base(Engine)
        {
        }
        #endregion

        #region Uploading
        public virtual void Upload(byte[] Data, IDataDescriptor Descriptor)
        {
            _descriptor = Descriptor;

            if (Data is object)
            {
                _data = Data;
                _length = Data.Length;
            }
            else
            {
                _data = new byte[1];
                _length = 0;
            }

            Invalidate();
        }
        #endregion
    }
}
