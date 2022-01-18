using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Shaders;

namespace ThinGin.Core.Common.Providers
{
    public interface IShaderProvider
    {
        IShaderInstance Create(IRenderEngine Engine);

        ShaderVariable Create_Variable(ShaderVariable Variable);
        ShaderVariable Create_Variable(ShaderVariable Variable, ReferencedDataDescriptor Descriptor);
        ShaderVariable Create_Variable(IShaderInstance Shader, ReferencedDataDescriptor Descriptor);

        IVariableBlock Create_Variable_Block(IShaderInstance Shader);
    }
}
