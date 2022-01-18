using System;
using System.Collections.Generic;

namespace ThinGin.Core.Shaders
{
    public class VariableBlock : IVariableBlock
    {
        #region Values
        protected WeakReference<IShaderInstance> _shaderRef;
        protected Dictionary<string, ShaderVariable> Items { get; private set; } = new Dictionary<string, ShaderVariable>();
        #endregion

        #region Accessors
        public IShaderInstance Shader => _shaderRef.TryGetTarget(out var Shader) ? Shader : null;

        public IEnumerable<ShaderVariable> Values
        {
            get
            {
                var enumerator = Items.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }

                yield break;
            }
        }
        #endregion

        #region Constructors
        public VariableBlock(Shader shader) : this(shader.Instance)
        {
        }
        public VariableBlock(IShaderInstance shader)
        {
            _shaderRef = new WeakReference<IShaderInstance>(shader);
            shader.OnChanged += (o, e) => Repopulate();

            if (shader.IsValid)
            {
                Repopulate();
            }
        }
        #endregion

        #region Indexer
        public ShaderVariable this[string Name]
        {
            get 
            {
                if (Items.TryGetValue(Name, out ShaderVariable outUniform))
                    return outUniform;

                // We create a kind of temporary undefined variable object jsut to store data in, the shader might update it later with a definition, at which point it will be fully valid.
                // We do this because the shader isnt guaranteed to be compiled and initialized at the moment the user starts assigning values to the named variables!
                var shaderVar = Shader.Engine.Provider.Shaders.Create_Variable(Shader, null);
                if (Items.TryAdd(Name, shaderVar))
                    return shaderVar;

                return null;
            }
        }
        #endregion

        #region Population
        /// <summary>
        /// Repopulates the list of uniforms from the owning shader
        /// </summary>
        private void Repopulate()
        {
            var newBlock = new Dictionary<string, ShaderVariable>(Shader.Variables.Length);
            var oldBlock = Items;

            foreach (var def in Shader.Variables)
            {
                // If this uniform existed in the old block then preserve its value.
                if (oldBlock.TryGetValue(def.Name, out ShaderVariable cUniform))
                {
                    ShaderVariable newVar = Shader.Engine.Provider.Shaders.Create_Variable(cUniform, def);
                    newBlock.Add(def.Name, newVar);
                }
                else// Otherwise just create a new instance
                {
                    ShaderVariable newVar = Shader.Engine.Provider.Shaders.Create_Variable(Shader, def);
                    newBlock.Add(def.Name, newVar);
                }
            }

            Items = newBlock;
        }
        #endregion

        #region IEngineBindable
        public void Bind()
        {
            if (!_shaderRef.TryGetTarget(out _))
            {
                throw new Exception("Attempting to upload uniform value block for non-existant shader program!");
            }

            foreach (var uniform in Values)
            {
                uniform.Submit();
            }
        }

        public void Unbind()
        {
        }

        public void Bound()
        {
        }

        public void Unbound()
        {
        }
        #endregion
    }
}
