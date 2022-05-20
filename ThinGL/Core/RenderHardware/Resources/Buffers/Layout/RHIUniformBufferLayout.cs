namespace ThinGin.Core.RenderHardware.Resources
{
    public readonly struct RHIUniformBufferLayout
    {

        #region Values
        /// <summary>
        /// Static indice of this buffers binding slot in the shader
        /// </summary>
        public readonly byte StaticSlot;
        /// <summary>
        /// The size in bytes of the constant buffer.
        /// </summary>
        public readonly uint ConstantSize;
        public readonly uint RenderTargetsOffset;

        // XXX: Placeholders
        // List<> GraphBuffers; // list of buffer refs
        // List<> Textures; // list of inlined texture refs
        // List<> UniformBuffers; // Inlined shader uniform buffer refs
        // List<> Resources; // Inlined shader resources
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        #endregion

    }
}
