
using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.Core.Shaders;
using ThinGin.OpenGL.Common.Shaders;

namespace ThinGin.OpenGL.GL2.Providers
{
    public class GL2ShaderProvider : IShaderProvider
    {
        #region Static Properties
        public static IShaderProvider Instance = new GL2ShaderProvider();
        #endregion

        public IShaderInstance Create(IRenderEngine Engine)
        {
            return new GLShaderInstance(Engine);
        }

        public ShaderVariable Create_Variable(ShaderVariable Variable)
        {
            return new GL2ShaderVariable(Variable);
        }

        public ShaderVariable Create_Variable(ShaderVariable Variable, ReferencedDataDescriptor Descriptor)
        {
            return new GL2ShaderVariable(Variable, Descriptor);
        }

        public ShaderVariable Create_Variable(IShaderInstance Shader, ReferencedDataDescriptor Descriptor)
        {
            return new GL2ShaderVariable(Shader, Descriptor);
        }

        public IVariableBlock Create_Variable_Block(IShaderInstance Shader)
        {
            return new VariableBlock(Shader);
        }
    }
}
