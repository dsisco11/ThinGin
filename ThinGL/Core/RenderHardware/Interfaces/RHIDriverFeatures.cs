namespace ThinGin.Core.RenderHardware
{
    /// <summary>
    /// Stores the values for all of an RHI drivers features and limitations, such as maximum texture size and such.
    /// </summary>
    public readonly struct RHIDriverFeatures
    {
        /// <summary> Drivers which do not support tiled GPU MSAA should specify 0 </summary>
        public readonly uint MaxMSAASamplesTileMem;

        public readonly uint MaxTextureSize;
        public readonly uint MaxArrayTextureLayers;

        public readonly uint Max3DTextureSize;
        public readonly uint MaxCubeMapTextureSize;

        public readonly uint MaxTextureUnits;
        public readonly uint MaxTextureImageUnits;

        public readonly uint MaxRenderbufferSize;
        public readonly uint MaxTextureBufferSize;

        public readonly uint MaxElementsIndices;
        public readonly uint MaxElementsVertices;

        public readonly uint MaxVertexAttribs;
        public readonly uint MaxUniformBufferBindings;

        // GMaxOpenGLColorSamples
        // GMaxOpenGLDepthSamples

        // Bools
        /// <summary>
        /// Indicates whether the current engine can support uniform buffer objects
        /// </summary>
        public readonly bool Supports_UniformBufferObjects;
        /// <summary>
        /// Indicates whether the current engine can support attribute divisors
        /// </summary>
        public readonly bool Supports_AttributeDivisors;
    }
}
