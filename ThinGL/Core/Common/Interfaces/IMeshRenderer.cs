using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// Manages all objects related to rendering a mesh as well as the method of rendering.
    /// </summary>
    public interface IMeshRenderer : IEngineObject
    {
        void Render(ETopology primitiveType);
    }
}
