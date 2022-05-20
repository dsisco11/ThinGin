using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware.Synchronization
{
    /// <summary>
    /// Generic GPU fence class. Granularity differs depending on backing RHI - ie it may only represent command buffer granularity. 
    /// RHI specific fences derive from this to implement real GPU->CPU fencing. 
    /// The default implementation always returns false for Poll until the next frame from the frame the fence was inserted because not all APIs have a GPU/CPU sync object, 
    /// we need to fake it.
    /// </summary>
    public abstract class RHIFenceObject : RHIResource
    {
        #region Values
        /// <summary>
        /// Caches the sync state of the primitive so we can stop bothering the GPU after the first time...
        /// </summary>
        protected bool _success = false;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public RHIFenceObject(RenderHardwareInterface rhi) : base(rhi)
        {
        }
        #endregion

        public bool Check()
        {
            if (!_success)
            {
                _success = Poll();
            }

            return _success;
        }

        #region Implementation
        protected abstract bool Poll();
        #endregion

        #region Disposal
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _success = true;
        }
        #endregion
    }
}
