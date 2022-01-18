using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Shaders
{
    public interface IVariableBlock : IEngineBindable
    {
        ShaderVariable this[string Name] { get; }
    }
}
