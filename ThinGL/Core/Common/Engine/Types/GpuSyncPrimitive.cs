
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Engine.Types
{
    public abstract class GpuSyncPrimitive : GObject
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
        public GpuSyncPrimitive(IEngine Engine) : base(Engine)
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
        public abstract void Begin();

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
