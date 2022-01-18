using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;

namespace ThinGin.Core.Common.Providers
{
    public interface IRendererProvider
    {
        IMeshRenderer Create(IRenderEngine Engine, Mesh mesh);
    }
}
