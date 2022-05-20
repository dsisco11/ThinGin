using System;

namespace ThinGin.Core.RenderHardware.Resources
{
    /// <summary>
    /// Represents anything which (in an abstract sense) is related to rendering and must be properly released upon disposal such that its lifetime should be managed by the engine implementation it belongs to.
    /// Such objects always belong to an engine instance at the time of their instantiation, and also provide delegates to both initialize and release their unmanaged handles for the specific graphics api to which they belong.
    /// <para>
    /// Engine managed objects may supply a null value for its initialization delegate under special conditions where it requires more complex initialization procedures.
    /// However; because the engine does both lazy initialization and release of these objects, it is HIGHLY encouraged to work within a delegate that is provided to the engine via the normal overloaded provider functions.
    /// </para>
    /// </summary>
    public abstract class RHIResource : IDisposable
    {
        #region Values
        private int _initializedValue = 0;
        private int _releasedValue = 0;
        private int _disposedValue = 0;
        private int _updatedValue = 0;

        /// <summary>
        /// Keep track of which engine we belong to
        /// </summary>
        private readonly WeakReference<IRHI> _ref = new WeakReference<IRHI>(null);

        private RHIDelegate _releaser;
        private RHIDelegate _initializer;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        /// <summary>
        /// The <see cref="IRHI"/> which has ownership of this resource.
        /// </summary>
        public IRHI RHI => _ref.TryGetTarget(out var outRef) ? outRef : null;
        public bool IsInitialized => _initializedValue != 0;
        #endregion

        #region Constructors
        public RHIResource(IRHI rhi)
        {
            _ref.SetTarget(rhi);

            _releaser = Get_Releaser();
            _initializer = Get_Initializer();

            if (_initializer is null)
            {
                _initializedValue = 1;// Provided no initializer, so we assume the resource self managed initialization via other means...
            }
            else
            {
                rhi.Resources.Register(this);
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
                if (_initializer is object)
                {
                    _initializer.Invoke();
#if RHI_ERROR_CHECKING
                    RHI.ErrorCheck(out _);
#endif
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
                if (_releaser is object)
                {
                    _releaser.Invoke();
#if RHI_ERROR_CHECKING
                    RHI.ErrorCheck(out _);
#endif
                    return true;
                }

                return false;
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
                RHI.Resources.LazyUpdate(this);
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
                var _updater = Get_Updater();
                if (_updater is object)
                {
                    _updater.Invoke();
#if RHI_ERROR_CHECKING
                    RHI.ErrorCheck(out _);
#endif
                    return true;
                }

                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Delegates
        public abstract RHIDelegate Get_Initializer();
        public abstract RHIDelegate Get_Releaser();
        public virtual RHIDelegate Get_Updater() => null;
        #endregion

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (System.Threading.Interlocked.Exchange(ref _disposedValue, 1) == 0)
            {
                RHI?.Resources.Unregister(this);
                System.Threading.Interlocked.Increment(ref _updatedValue);
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RHIResource()
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
