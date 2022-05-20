using System;
using System.Runtime.CompilerServices;

using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Console;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Objects
{
    /// <summary>
    /// base class for all things that can be and have other things attached to them as children, this forms the basis of all things in the engine.
    /// </summary>
    public abstract class EngineObject : EngineObjectBase
    {
        #region Values
        public readonly ObjectID Id;
        #endregion

        #region Scripts
        // XXX: TODO: add script objects that can take in a class as a handler and identify methods marked with a special attribute,
        // said methods will become named script events and will be executed in response to events of the same name which the object is subjected to!
        #endregion

        #region Constructors
        public EngineObject(EngineInstance engine) : base(engine)
        {
            Id = engine.World.Objects.Register(this);
        }
        #endregion

        #region Internal Lifecycle
        public override IEngineDelegate Get_Initializer() => null;
        public override IEngineDelegate Get_Releaser() => null;
        public override IEngineDelegate Get_Updater() => null;
        #endregion


        #region GetHashCode
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Id.Value;
        #endregion

        #region World
        internal void OnRegistered(ObjectID id)
        {
        }

        internal void OnUnregistered(ObjectID id)
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
        public virtual void OnTick(double time)
        {
        }
        #endregion
    }
}
