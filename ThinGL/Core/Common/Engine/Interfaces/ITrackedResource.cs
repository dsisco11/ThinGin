using System;

using ThinGin.Core.RenderHardware;

namespace ThinGin.Core.Common.Engine.Interfaces
{
    public interface ITrackedResource : IDisposable
    {
        #region Accessors
        IRHI RHI { get; }
        bool IsInitialized { get; }
        #endregion

        #region State Enforcement
        bool EnsureInitialized();
        bool EnsureReady();
        bool EnsureUpdated();
        #endregion

        #region Engine Delegate Retreival
        RHIDelegate Get_Initializer();
        RHIDelegate Get_Releaser();
        RHIDelegate Get_Updater();
        #endregion

        #region Invalidation
        /// <summary>
        /// Triggers a deferred update of the resource
        /// </summary>
        void Invalidate();
        #endregion
    }
}