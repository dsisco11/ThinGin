
using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.Core.Shaders;
using ThinGin.OpenGL.Common.Shaders;

namespace ThinGin.OpenGL.GL3.Providers
{
    public class GL3ShaderProvider : IShaderProvider
    {
        #region Static Properties
        public static IShaderProvider Instance = new GL3ShaderProvider();
        #endregion

        public IShaderInstance Create(IRenderEngine Engine)
        {
            return new GLShaderInstance(Engine);
        }

        public ShaderVariable Create_Variable(ShaderVariable Variable)
        {
            return new GL3ShaderVariable(Variable);
        }

        public ShaderVariable Create_Variable(ShaderVariable Variable, ReferencedDataDescriptor Descriptor)
        {
            return new GL3ShaderVariable(Variable, Descriptor);
        }

        public ShaderVariable Create_Variable(IShaderInstance Shader, ReferencedDataDescriptor Descriptor)
        {
            return new GL3ShaderVariable(Shader, Descriptor);
        }

        public IVariableBlock Create_Variable_Block(IShaderInstance Shader)
        {
            return new VariableBlock(Shader);
        }
    }
}
