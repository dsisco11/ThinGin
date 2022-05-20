using System;
using System.Runtime.CompilerServices;

using ThinGin.Core.World;

namespace ThinGin.Core.Common.Objects
{
    /// <summary>
    /// Represents a specific unique object within a world instance
    /// </summary>
    public sealed class ObjectID
    {
        #region Values
        WeakReference<EngineObjectBase> _objRef = null;
        #endregion

        #region Properties
        public readonly int Value;
        #endregion

        #region Accessors
        public EngineObjectBase Object => _objRef.TryGetTarget(out var outRef) ? outRef : null;
        public WorldManager World => Object.Engine.World;
        #endregion

        #region Constructors
        internal ObjectID(int value, EngineObjectBase obj)
        {
            Value = value;

            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _objRef = new WeakReference<EngineObjectBase>(obj);
        }
        #endregion

        #region GetHashCode
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Value;
        #endregion
    }
}
