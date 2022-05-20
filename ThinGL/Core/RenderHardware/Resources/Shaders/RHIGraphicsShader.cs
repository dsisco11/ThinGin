namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIGraphicsShader : RHIShader
    {
        #region Values
        #endregion

        #region Constructors
        protected RHIGraphicsShader(in IRHI rhi, EShaderType shaderType) : base(rhi, shaderType)
        {
        }
        #endregion
    }
}
