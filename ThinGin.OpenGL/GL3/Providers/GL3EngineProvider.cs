
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;

namespace ThinGin.OpenGL.GL3.Providers
{
    public class GL3EngineProvider : IEngineProvider
    {
        #region Static Properties
        public static IEngineProvider Instance = new GL3EngineProvider();
        #endregion

        public IRenderEngine Create()
        {
            return new GL3Engine(System.Threading.Thread.CurrentThread);
        }
    }
}
