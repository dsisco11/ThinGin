
using System;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Shaders;

namespace ThinGin.OpenGL.Common.Shaders
{
    /// <summary>
    /// <inheritdoc cref="ShaderVariable"/>
    /// </summary>
    public abstract class GLShaderVariable : ShaderVariable
    {
        #region Properties
        public EVariableType UniformType => (EVariableType)Descriptor.Context;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="ShaderVariable(ShaderVariable)"/>
        /// </summary>
        public GLShaderVariable(ShaderVariable other) : base(other)
        {
        }

        /// <summary>
        /// <inheritdoc cref="ShaderVariable(ShaderVariable, ReferencedDataDescriptor)"/>
        /// </summary>
        public GLShaderVariable(ShaderVariable other, ReferencedDataDescriptor Descriptor) : base(other, Descriptor)
        {
        }

        /// <summary>
        /// <inheritdoc cref="ShaderVariable(IShaderInstance, ReferencedDataDescriptor)"/>
        /// </summary>
        public GLShaderVariable(IShaderInstance Shader, ReferencedDataDescriptor Descriptor) : base(Shader, Descriptor)
        {
        }
        #endregion
    }
}
