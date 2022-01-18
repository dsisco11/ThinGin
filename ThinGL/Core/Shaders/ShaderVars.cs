using System;

using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Shaders
{
    /// <summary>
    /// Facilitates the optimized storage and state management of a set of shader uniform variables.
    /// <remarks>This class provides persistent storage for all uniform values given to it, so that the user need only update their values when they change.</remarks>
    /// <para>This class will compile a dynamic map of shader uniform variable names to their relevant program id's, meaning that they do not need to be constantly refetched, saving GPU time.</para>
    /// </summary>
    public class ShaderVars : IEngineBindable
    {
        #region Values
        protected WeakReference<IShaderInstance> _shaderRef;
        protected IVariableBlock Block;
        #endregion

        #region Accessors
        public IShaderInstance Instance => _shaderRef.TryGetTarget(out var Shader) ? Shader : null;
        #endregion

        #region Constructors
        public ShaderVars(IShader shader)
        {
            _shaderRef = new WeakReference<IShaderInstance>(shader.Instance);
            Block = Instance.Engine.Provider.Shaders.Create_Variable_Block(Instance);
        }
        #endregion

        #region Indexer
        public ShaderVariable this[string Name] => Block[Name];
        #endregion

        #region IEngineBindable
        public virtual void Bind() => Block.Bind();
        public virtual void Unbind() => Block.Unbind();
        public virtual void Bound() => Block.Bound();
        public virtual void Unbound() => Block.Unbound();
        #endregion
    }
}
