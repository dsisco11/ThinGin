using ThinGin.Core.RenderHardware.Resources;
using ThinGin.Core.RenderHardware.Resources.Shaders;

namespace ThinGin.Core.RenderHardware.Pipelines
{
    public readonly struct BoundShaderStateInput
    {
        public readonly RHIVertexDecleration VertexLayout;
        public readonly RHIHullShader HullShader;
        public readonly RHIDomainShader DomainShader;
        public readonly RHIGeometryShader GeometryShader;
        public readonly RHIVertexShader VertexShader;
        public readonly RHIPixelShader PixelShader;
    }
}
