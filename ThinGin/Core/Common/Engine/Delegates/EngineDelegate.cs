
using System;

using ThinGin.Core.Common.Engine.Interfaces;

namespace ThinGin.Core.Common.Engine.Delegates
{
    /// <summary>
    /// Provides a generic initializer delegate that facilitates lazy initialization of an engine resource.
    /// </summary>
    public sealed class EngineDelegate : IEngineDelegate
    {
        #region Values
        private readonly Action _delegate;
        private int _triggered = 0;
        #endregion

        #region Constructors
        public EngineDelegate(Action InitializerDelegate)
        {
            _delegate = InitializerDelegate ?? throw new ArgumentNullException(nameof(InitializerDelegate));
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
                        _delegate.Invoke();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
