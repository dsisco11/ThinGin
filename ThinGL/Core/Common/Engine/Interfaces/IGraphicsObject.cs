using System;

using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Engine.Interfaces
{
    public interface IGraphicsObject : IDisposable
    {
        #region Accessors
        EngineInstance Engine { get; }
        bool IsInitialized { get; }
        #endregion

        #region State Enforcement
        bool EnsureInitialized();
        bool EnsureReady();
        bool EnsureUpdated();
        #endregion

        #region Engine Delegate Retreival
        IEngineDelegate Get_Initializer();
        IEngineDelegate Get_Releaser();
        IEngineDelegate Get_Updater();
        #endregion

        #region Invalidation
        void Invalidate();
        #endregion
    }
}