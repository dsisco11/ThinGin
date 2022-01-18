using System;

namespace ThinGin.Core.World.Objects
{
    /// <summary>
    /// Represents a specific unique object within a world instance
    /// </summary>
    public class ObjectID
    {
        #region Values
        WeakReference<WObject> _objRef = null;
        WeakReference<WorldManager> _worldRef = null;
        #endregion

        #region Properties
        public readonly UInt64 Value;
        #endregion

        #region Accessors
        public WObject Object => _objRef.TryGetTarget(out var outRef) ? outRef : null;
        public WorldManager World => _worldRef.TryGetTarget(out var outRef) ? outRef : null;
        #endregion

        #region Constructors
        internal ObjectID(WorldManager world, WObject obj)
        {
            if (world is null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Value = world._objectid_seq++;
            _objRef = new WeakReference<WObject>(obj);
            _worldRef = new WeakReference<WorldManager>(world);
        }
        #endregion
    }
}
