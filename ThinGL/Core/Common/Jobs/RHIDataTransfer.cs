using System;
using System.Runtime.InteropServices;

using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Engine
{
    /// <summary>
    /// <inheritdoc cref="IEngineAsyncJob"/>
    /// </summary>
    public class RHIDataTransfer : EngineJob, IDisposable
    {
        #region Values
        private GCHandle gcHandle;
        private int _disposed;
        #endregion

        #region Properties
        public GpuSyncPrimitive SyncPrimitive { get; private set; }
        /// <summary>
        /// <inheritdoc cref="IEngineAsyncJob.Handle"/>
        /// </summary>
        public ReadOnlyMemory<byte> Data { get; private set; }

        /// <summary>
        /// The relevant GPU buffer wrapper abstraction facilitating the transfer
        /// </summary>
        public IBufferObject Buffer { get; private set; }
        #endregion

        #region Constructors
        public RHIDataTransfer(IEngine Engine, GpuSyncPrimitive SyncPrimitive, IBufferObject Buffer, ReadOnlyMemory<byte> Data) : base(Engine)
        {
            this.SyncPrimitive = SyncPrimitive ?? throw new ArgumentNullException(nameof(SyncPrimitive));
            this.Buffer = Buffer ?? throw new ArgumentNullException(nameof(Buffer));

            this.Data = Data;
            gcHandle = GCHandle.Alloc(this.Data, GCHandleType.Pinned);
        }
        #endregion

        public override bool TryConfirm()
        {
            return SyncPrimitive.Check();
        }

        protected override unsafe void Run()
        {
            var dest = Buffer.Address.ToPointer();
            using (var pixelPtr = Data.Pin())
            {
                TokenSource.Token.ThrowIfCancellationRequested();
                System.Buffer.MemoryCopy(pixelPtr.Pointer, dest, Data.Length, Data.Length);
            }
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (System.Threading.Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                gcHandle.Free();

                Buffer = null;

                SyncPrimitive?.Dispose();
                SyncPrimitive = null;
            }
        }

        ~RHIDataTransfer()
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
