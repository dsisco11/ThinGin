namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIPixelShader : RHIGraphicsShader
    {
        protected RHIPixelShader(in IRHI rhi) : base(rhi, EShaderType.Pixel)
        {
        }
    }
}