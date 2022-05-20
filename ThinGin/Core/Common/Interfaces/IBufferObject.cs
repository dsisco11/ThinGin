using System;

using ThinGin.Core.Exceptions;
using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// Implements functionality for interacting with OpenGL pixelbuffers for Async data transfer via DMA(Direct memory access)
    /// </summary>
    public interface IBufferObject : IDisposable
    {
        #region Properties
        /// <summary>
        /// Size of this buffer in bytes 
        /// </summary>
        int Length { get; }
        /// <summary>
        /// DMA memory address for the buffer
        /// </summary>
        IntPtr Address { get; }
        #endregion

        #region Binding
        /// <summary>
        /// Swaps to the next buffer object and binds it
        /// </summary>
        void Bind();

        /// <summary>
        /// Unbinds the buffer object
        /// </summary>
        void Unbind();
        #endregion

        #region Direct Memory Access
        /// <summary>
        /// Attempts to get a Direct Memory Access address where the client can access the buffers data from
        /// </summary>
        /// <param name="dmaAddress"></param>
        /// <returns>Success</returns>
        /// <exception cref="ThinGinException"></exception>
        bool TryMap(EBufferAccess Access, out IntPtr dmaAddress);

        /// <summary>
        /// Releases the buffer from client address space
        /// </summary>
        /// <returns></returns>
        bool TryUnmap();
        #endregion
    }
}
