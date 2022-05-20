using ThinGin.Core.Common.Engine;
using ThinGin.Core.Engine;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.World
{
    public class WorldManager
    {
        #region Values
        public readonly EngineObjectManager Objects;
        private double _time = 0d;
        #endregion

        #region Properties
        /// <summary>
        /// This is a seed number that is different each time the application is run.
        /// Use it for generic randomness seeding where being deterministic is not a requirement.
        /// </summary>
        public uint SeedUnstable = 0;
        #endregion

        #region Accessors
        /// <summary> Total cumulative seconds since the world started ticking </summary>
        public double Time => _time;
        #endregion

        #region Constructors
        public WorldManager(EngineInstance engine)
        {
            Objects = new EngineObjectManager(engine);
            SeedUnstable = unchecked((uint)System.Guid.NewGuid().GetHashCode());
        }
        #endregion

        public RenderTimestamp Get_Timestamp()
        {
            var ts = new RenderTimestamp();
            ts.Set(_time);
            return ts;
        }


        #region Ticking
        public void Tick(double time)
        {
            _time += time;
            foreach (var obj in Objects)
            {
                obj.OnTick(time);
            }
        }
        #endregion
    }
}
