namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIDomainShader : RHIGraphicsShader
    {
        protected RHIDomainShader(in IRHI rhi) : base(rhi, EShaderType.Domain)
        {
        }
    }
}