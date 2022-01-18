using System;
using System.Collections.Generic;

using ThinGin.Core.Console;

namespace ThinGin.Core.World.Objects
{
    public abstract class WObject : WObjectBase
    {
        #region Values
        private readonly WeakReference<WObject> _parentRef = new WeakReference<WObject>(null);

        private readonly List<WObject> _children = new List<WObject>();
        #endregion

        #region Scripts
        // XXX: TODO: add script objects that can take in a class as a handler and identify methods marked with a special attribute,
        // said methods will become named script events and will be executed in response to events of the same name which the object is subjected to!
        #endregion

        #region Flags
        private EObjectFlags flags = 0x0;
        public void Set_Flags(EObjectFlags Flags) => flags |= Flags;
        public void Clear_Flags(EObjectFlags Flags) => flags &= ~Flags;
        public bool Has_Flag(EObjectFlags Flag) => flags.HasFlag(Flag);

        /// <summary>
        /// Tests for a set of flags, returning those which are set.
        /// </summary>
        public EObjectFlags Get_Flags(EObjectFlags Flags) => (flags & Flags);
        #endregion

        #region Properties
        public readonly ObjectID ID;
        #endregion

        #region Accessors
        public WObject Parent => _parentRef.TryGetTarget(out var outRef) ? outRef : null;

        public IReadOnlyList<WObject> Children => this._children;
        #endregion

        #region Constructors
        public WObject(WorldManager world, WObject parent = null)
        {
            ID = world.Register(this);

            if (parent is object)
            {
                parent.Add_Child(this);
            }
        }
        #endregion

        #region Parenting
        public void Add_Child(WObject child)
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

        public bool Remove_Child(WObject child)
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
        internal void Unparented(WObject Caller)
        {
            _parentRef.SetTarget(null);
        }

        /// <summary>
        /// Called on a child object by its parent object upon adoption
        /// </summary>
        /// <param name="NewParent"></param>
        internal void Parented(WObject NewParent)
        {
            _parentRef.SetTarget(NewParent);
        }
        #endregion

        #region World
        internal void Registered(ObjectID id)
        {
        }

        internal void Unregistered(ObjectID id)
        {
        }
        #endregion

        #region Processing
        /// <summary>
        /// Attempts to process a 'command' on this object
        /// </summary>
        /// <returns></returns>
        public virtual bool TryProcessCommand(CCommand cmd) 
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Common Events
        public virtual void Tick(double time)
        {
        }
        #endregion

    }
}
