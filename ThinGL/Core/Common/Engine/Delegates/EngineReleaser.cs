
using System;

using ThinGin.Core.Common.Engine.Interfaces;

namespace ThinGin.Core.Common.Engine.Delegates
{
    /// <summary>
    /// Provides a generic releaser object that facilitates lazy disposal of a GPU resource by calling a specified graphics API delegate and passing it the given handle object to dispose.
    /// </summary>
    public sealed class EngineReleaser<Ty> : IEngineDelegate
    {
        #region Values
        private readonly Ty _handle;
        private readonly Action<Ty> _delegate;
        private int _triggered = 0;
        #endregion

        #region Constructors
        public EngineReleaser(Ty Handle, Action<Ty> ReleaseDelegate)
        {
            _handle = Handle;
            _delegate = ReleaseDelegate ?? throw new ArgumentNullException(nameof(ReleaseDelegate));
        }
        #endregion

        /// <summary>
        /// <inheritdoc cref="IEngineDelegate.TryInvoke"/>
        /// </summary>
        public bool TryInvoke()
        {
            if (System.Threading.Interlocked.Exchange(ref _triggered, 1) == 0)
            {
                if (_delegate is object)
                {
                    var list = _delegate.GetInvocationList();
                    if (list is object && list.Length > 0)
                    {
                        _delegate.Invoke(_handle);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
