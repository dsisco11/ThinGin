namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIPipelineBinaryLibrary : RHIUntrackedResource
    {
        #region Values
        public readonly EShaderPlatform ShaderPlatform;
        public readonly string FilePath;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        protected RHIPipelineBinaryLibrary(in IRHI rhi, in EShaderPlatform shaderPlatform, in string filePath) : base(rhi)
        {
            ShaderPlatform = shaderPlatform;
            FilePath = filePath;
        }
        #endregion
    }
}
