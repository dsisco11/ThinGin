using ThinGin.Core.Engine;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.World
{
    public class WorldManager
    {
        #region Values
        public readonly EngineObjectManager Objects;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public WorldManager(EngineInstance engine)
        {
            Objects = new EngineObjectManager(engine);
        }
        #endregion


        #region Ticking
        public void Update(double time)
        {
            foreach (var obj in Objects)
            {
                obj.OnTick(time);
            }
        }
        #endregion
    }
}
