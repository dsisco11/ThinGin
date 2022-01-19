namespace ThinGin.Core.Engine.Common
{
    public abstract class EngineCompatabilityList
    {
        #region Properties
        public int MaxTextureSize { get; protected set; }
        public int MaxArrayTextureLayers { get; protected set;}

        public int Max3DTextureSize { get; protected set;}
        public int MaxCubeMapTextureSize { get; protected set;}

        public int MaxTextureUnits { get; protected set;}
        public int MaxTextureImageUnits { get; protected set;}

        public int MaxRenderbufferSize { get; protected set;}
        public int MaxTextureBufferSize { get; protected set;}

        public int MaxElementsIndices { get; protected set;}
        public int MaxElementsVertices { get; protected set;}

        public int MaxVertexAttribs { get; protected set;}
        public int MaxUniformBufferBindings { get; protected set;}
        #endregion

        #region Supports
        /// <summary>
        /// Indicates whether the current engine can support uniform buffer objects
        /// </summary>
        public bool UniformBufferObjects { get; protected set; } = false;
        /// <summary>
        /// Indicates whether the current engine can support attribute divisors
        /// </summary>
        public bool AttributeDivisors { get; protected set; } = false;
        #endregion
    }
}
