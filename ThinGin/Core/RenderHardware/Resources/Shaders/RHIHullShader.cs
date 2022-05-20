namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIHullShader : RHIGraphicsShader
    {
        protected RHIHullShader(in IRHI rhi) : base(rhi, EShaderType.Hull)
        {
        }
    }
}