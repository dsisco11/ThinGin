namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIRayTracingShader : RHIShader
    {
        #region Values
        #endregion

        #region Constructors
        protected RHIRayTracingShader(in IRHI rhi, EShaderType shaderType) : base(rhi, shaderType)
        {
        }
        #endregion
    }
}
