namespace ThinGin.OpenGL.Common.Framebuffers
{
    public struct RenderBufferOptions
    {
        #region Properties
        /// <summary>
        /// Enable or disable Multi-Sample AntiAliasing
        /// </summary>
        public bool MSAA;
        /// <summary>
        /// Number of samples to use for MSAA
        /// </summary>
        public int Samples;
        #endregion

        #region Constructors
        public RenderBufferOptions(bool EnableMSAA, int MSAASamples = 4)
        {
            MSAA = EnableMSAA;
            Samples = MSAASamples;
        }
        #endregion
    }
}
