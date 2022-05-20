using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Enums;
using ThinGin.Core.RenderHardware.Resources.Rasterizer;

namespace ThinGin.Core.RenderHardware.Pipelines
{
    public readonly ref struct GraphicsPipelineStateInit
    {
        public readonly uint Flags;

        public readonly bool DepthBounds;
        public readonly bool HasFragmentDensityAttachment;
        public readonly bool FromPipelineStateFileCache;
        public readonly RHIBlendState BlendState;

        public readonly BoundShaderStateInput BoundShaderState;
        public readonly ExclusiveDepthStencilAccess DepthStencilAccess;

        public readonly RHIDepthStencilState DepthStencilState;

        public readonly uint DepthStencilTargetFlag;
        public readonly EPixelFormat DepthStencilTargetFormat;

        public readonly ERenderTargetLoadAction DepthTargetLoadAction;
        public readonly ERenderTargetStoreAction DepthTargetStoreAction;

        public readonly ImmutableSamplerState ImmutableSamplerState;

        public readonly byte MultiViewCount;
        public readonly ushort NumSamples;


        public readonly ETopology PrimitiveType;
        public readonly RHIRasterizerState RasterizerState;

        public readonly uint RenderTargetFlags;
        public readonly EPixelFormat[] RenderTargetFormats;

        public readonly uint RenderTargetsEnabled;
        // XXX: VRShadingRate value if we get VR support

        public readonly ERenderTargetLoadAction StencilTargetLoadAction;
        public readonly ERenderTargetStoreAction StencilTargetStoreAction;

        public readonly ESubpassHint SubpassHint;
        public readonly byte SubpassIndex;

    }
}
