namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIGeometryShader : RHIGraphicsShader
    {
        protected RHIGeometryShader(in IRHI rhi) : base(rhi, EShaderType.Geometry)
        {
        }
    }
}