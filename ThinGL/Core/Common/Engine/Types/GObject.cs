using System;

using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Engine.Types
{
    /// <summary>
    /// Represents anything which (in an abstract sense) is related to rendering and must be properly released upon disposal such that its lifetime should be managed by the engine implementation it belongs to.
    /// Such objects always belong to an engine instance at the time of their instantiation, and also provide delegates to both initialize and release their unmanaged handles for the specific graphics api to which they belong.
    /// <para>
    /// Engine managed objects may supply a null value for its initialization delegate under special conditions where it requires more complex initialization procedures.
    /// However; because the engine does both lazy initialization and release of these objects, it is HIGHLY encouraged to work within a delegate that is provided to the engine via the normal overloaded provider functions.
    /// </para>
    /// </summary>
    public abstract class GObject : IGraphicsObject, IDisposable
    {
        #region Properties
        private int _initializedValue = 0;
        private int _releasedValue = 0;
        private int _disposedValue = 0;
        private int _updatedValue = 0;
        /// <summary>
        /// Keep track of which engine we belong to
        /// </summary>
        private WeakReference<EngineInstance> _engineRef = new WeakReference<EngineInstance>(null);

        private IEngineDelegate _releaser;
        private IEngineDelegate _initializer;
        #endregion

        #region Accessors
        /// <summary>
        /// The <see cref="EngineInstance"/> which has ownership of this resource.
        /// </summary>
        public EngineInstance Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        //public GraphicsObjectManager Manager => 

        public bool IsInitialized => (_initializedValue != 0);
        #endregion

        #region Constructors
        public GObject(EngineInstance engine)
        {
            _engineRef.SetTarget(engine);
            _releaser = Get_Releaser();
            _initializer = Get_Initializer();

            if (_initializer is null)
            {
                _initializedValue = 1;// Provided no initializer, so we assume the resource self managed initialization via other means...
            }
            else
            {
                engine.Rendering.Objects.Register(this);
            }
        }
        #endregion

        #region Lifetime Management
        /// <summary>
        /// Ensures that the object has been initialized and updated such that it is ready for use.
        /// </summary>
        /// <returns></returns>
        public bool EnsureReady()
        {
            if (!TryInitialize()) return false;
            if (!TryUpdate()) return false;

            return true;
        }
        /// <summary>
        /// Performs early initialization of the resource if required.
        /// Use this in circumstances where the resource is created and immediately used within the same engine cycle (before the engine can perform lazy initialization).
        /// </summary>
        public bool EnsureInitialized()
        {
            return TryInitialize();
        }

        internal bool TryInitialize()
        {
            if (System.Threading.Interlocked.Exchange(ref _initializedValue, 1) == 0)
            {
                if (_initializer.TryInvoke())
                {
                    Engine.ErrorCheck(out _);
                    return true;
                }
                else// if the initializer fails then we auto release to avoid corruption by default
                {
                    TryRelease();
                    _disposedValue = 1;
                    return false;// failed to initialize
                }
            }
            else// Return true because initialization has already been done...
            {
                return true;
            }
        }
        internal bool TryRelease()
        {
            if (System.Threading.Interlocked.Exchange(ref _releasedValue, 1) == 0)
            {
                System.Threading.Interlocked.Increment(ref _updatedValue);// We increment this as a sort of sanity-check to prevent late-bound updates for previously released objects...
                var res = _releaser?.TryInvoke() ?? false;
                Engine.ErrorCheck(out _);
                return res;
            }
            else// Return true because releasal has already been done...
            {
                return true;
            }
        }
        #endregion

        #region Update Management
        public void Invalidate()
        {
            if (_disposedValue != 0)
                return;

            if (System.Threading.Interlocked.Exchange(ref _updatedValue, 0) == 1)
            {
                Engine.Rendering.Objects.LazyUpdate(this);
            }
        }

        public bool EnsureUpdated()
        {
            if (_disposedValue != 0)
                return false;

            return TryUpdate();
        }

        internal bool TryUpdate()
        {
            if (System.Threading.Interlocked.Exchange(ref _updatedValue, 1) == 0)
            {
                var res = Get_Updater()?.TryInvoke() ?? true;
                Engine.ErrorCheck(out _);
                return res;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Delegates
        public abstract IEngineDelegate Get_Initializer();
        public abstract IEngineDelegate Get_Releaser();
        public virtual IEngineDelegate Get_Updater() => null;
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (System.Threading.Interlocked.Exchange(ref _disposedValue, 1) == 0)
            {
                Engine?.Rendering.Objects.Unregister(this);
                System.Threading.Interlocked.Increment(ref _updatedValue);
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~GObject()
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
