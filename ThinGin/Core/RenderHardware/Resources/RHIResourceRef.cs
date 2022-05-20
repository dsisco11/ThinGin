using System;

namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// Wraps a weak reference to an <see cref="RHIResource"/>
    /// </summary>
    public sealed class RHIResourceRef
    {
        #region Properties
        public readonly WeakReference<RHIResource> _resRef = null;
        #endregion

        #region Constructors
        public RHIResourceRef(RHIResource target)
        {
            _resRef = new WeakReference<RHIResource>(target);
        }
        #endregion

        public void Set(RHIResource resource)
        {
            _resRef.SetTarget(resource);
        }

        public RHIResource Get()
        {
            return _resRef.TryGetTarget(out var outRef) ? outRef : null;
        }
    }
}
