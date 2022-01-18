using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Providers
{
    public interface IEngineProvider
    {
        IRenderEngine Create();
    }
}
