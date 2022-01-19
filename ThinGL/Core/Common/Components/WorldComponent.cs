using ThinGin.Core.Common.Objects;
using ThinGin.Core.Common.Units.Types;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Components
{
    public class WorldComponent : EngineObject
    {
        #region Values
        #endregion

        #region Properties
        public readonly Transform Transform;
        #endregion

        #region Constructors
        public WorldComponent(EngineInstance engine) : base(engine)
        {
            Transform = new Transform(engine);
        }
        #endregion
    }
}
