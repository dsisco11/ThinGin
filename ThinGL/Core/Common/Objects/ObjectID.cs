using System;
using System.Runtime.CompilerServices;

using ThinGin.Core.Engine.Common.Core;
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
        WeakReference<EngineInstance> _engineRef = null;
        #endregion

        #region Properties
        public readonly int Value;
        #endregion

        #region Accessors
        public EngineObjectBase Object => _objRef.TryGetTarget(out var outRef) ? outRef : null;
        public EngineInstance Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        public WorldManager World => Engine.World;
        #endregion

        #region Constructors
        internal ObjectID(EngineInstance engine, EngineObjectBase obj)
        {
            if (engine is null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            _objRef = new WeakReference<EngineObjectBase>(obj);
            _engineRef = new WeakReference<EngineInstance>(engine);
        }
        #endregion

        #region GetHashCode
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Value;
        #endregion
    }
}
