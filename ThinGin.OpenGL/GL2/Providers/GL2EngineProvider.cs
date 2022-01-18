
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;

namespace ThinGin.OpenGL.GL2.Providers
{
    public class GL2EngineProvider : IEngineProvider
    {
        #region Static Properties
        public static IEngineProvider Instance = new GL2EngineProvider();
        #endregion

        public IRenderEngine Create()
        {
            return new GL2Engine(System.Threading.Thread.CurrentThread);
        }
    }
}
