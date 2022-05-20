using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Providers
{
    public interface IRendererProvider
    {
        IMeshRenderer Create(EngineInstance Engine, Mesh mesh);
    }
}
