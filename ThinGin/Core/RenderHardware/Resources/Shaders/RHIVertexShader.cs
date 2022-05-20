namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIVertexShader : RHIGraphicsShader
    {
        protected RHIVertexShader(in IRHI rhi) : base(rhi, EShaderType.Vertex)
        {
        }
    }
}