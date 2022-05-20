using System;
using System.Collections.Generic;

using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Engine;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Objects
{
    /// <summary>
    /// low level engine related stuff for objects, contains the stuff that should not be messed with by end users
    /// </summary>
    public abstract class EngineObjectBase : IDisposable
    {
        #region Values
        private int _disposedValue = 0;
        private int _initializedValue = 0;
        private int _releasedValue = 0;
        private int _updatedValue = 0;
        private readonly WeakReference<EngineInstance> _engineRef;
        private readonly WeakReference<EngineObjectBase> _parentRef = new WeakReference<EngineObjectBase>(null);
        private readonly List<EngineObjectBase> _children = new List<EngineObjectBase>();
        #endregion

        #region Properties
        #endregion

        #region Accessors
        public IReadOnlyList<EngineObjectBase> Children => _children;
        public EngineInstance Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        public EngineObjectBase Parent => _parentRef.TryGetTarget(out var outRef) ? outRef : null;

        public EngineObjectManager Manager => Engine.World.Objects;

        public bool IsInitialized => (_initializedValue != 0);
        #endregion

        #region Flags
        private EObjectFlags flags = 0x0;
        public void Set_Flags(EObjectFlags Flags) => flags |= Flags;
        public void Clear_Flags(EObjectFlags Flags) => flags &= ~Flags;
        public bool Has_Flag(EObjectFlags Flag) => flags.HasFlag(Flag);

        /// <summary>
        /// Tests for a set of flags, returning those which are set.
        /// </summary>
        public EObjectFlags Get_Flags(EObjectFlags Flags) => flags & Flags;
        #endregion

        #region Constructors
        public EngineObjectBase(EngineInstance engine)
        {
            _engineRef = new WeakReference<EngineInstance>(engine);
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
                if (Get_Initializer().TryInvoke())
                {
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
                var res = Get_Releaser()?.TryInvoke() ?? false;
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
                Manager.DeferUpdate(this as EngineObject);
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
                Manager.Unregister(this as EngineObject);
                System.Threading.Interlocked.Increment(ref _updatedValue);
            }
        }

        ~EngineObjectBase()
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

        #region Parenting
        public void Add_Child(EngineObjectBase child)
        {
            if (child is null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            if (child.Has_Flag(EObjectFlags.Parented))
            {
                child.Parent.Remove_Child(child);
            }

            _children.Add(child);
            child.Parented(this);
        }

        public bool Remove_Child(EngineObjectBase child)
        {
            if (child is null)
            {
                throw new ArgumentNullException(nameof(child));
            }

            if (_children.Remove(child))
            {
                child.Unparented(this);
                return true;
            }

            return false;
        }
        #endregion

        #region Childing
        /// <summary>
        /// Called on a child object by its parent object upon being orphaned
        /// </summary>
        /// <param name="Caller"></param>
        internal void Unparented(EngineObjectBase Caller)
        {
            _parentRef.SetTarget(null);
        }

        /// <summary>
        /// Called on a child object by its parent object upon adoption
        /// </summary>
        /// <param name="NewParent"></param>
        internal void Parented(EngineObjectBase NewParent)
        {
            _parentRef.SetTarget(NewParent);
        }
        #endregion


    }
}
