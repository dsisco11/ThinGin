using System;
using System.Threading.Tasks;

using ThinGin.Core.Common.Data;
using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Synchronization;

namespace ThinGin.Core.RenderHardware.DMA
{
    /// <summary>
    /// Facilitates interacting with Direct Memory Access to a hardware device
    /// </summary>
    public class DMAInterface : IDisposable
    {
        #region Properties
        private int disposedValue = 0;
        private IntPtr address = IntPtr.Zero;
        private IRHIDriver driver = null;
        private RHIHandle handle = null;
        private RHIFenceObject fence = null;
        #endregion

        #region Constructors
        public DMAInterface(IRHIDriver driver, RHIHandle handle)
        {
            this.driver = driver;
            this.handle = handle;
        }
        #endregion

        #region Utility
        /// <summary>
        /// Returns number of bytes that make up the data
        /// </summary>
        /// <returns></returns>
        public UInt32 GetSize()
        {
        }
        #endregion

        #region Writing
        public bool TryWriteData(ReadOnlySpan<byte> Data)
        {
        }

        public bool TryWriteData(DataChunk Data)
        {
        }
        #endregion

        #region  Reading
        public bool TryReadData(out byte[] Data)
        {
        }
        #endregion

        #region Allocation
        /// <summary>
        /// Checks our flags to see if the hardware has finished preparing our DMA buffer destination
        /// </summary>
        public void HasAllocationCompleted()
        {
        }

        /// <summary>
        /// Cancels the allocation process and frees any related resources
        /// </summary>
        public void CancelAllocation()
        {
        }

        /// <summary>
        /// Blocks the calling thread until allocation has completed
        /// </summary>
        public async Task WaitAllocation()
        {
        }
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (System.Threading.Interlocked.Exchange(ref disposedValue, 1) == 0)
            {
                CancelAllocation();
            }
        }

        ~DMAInterface()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
