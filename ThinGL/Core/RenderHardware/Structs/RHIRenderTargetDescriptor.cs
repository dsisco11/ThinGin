using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.RenderHardware
{
    public readonly struct RHIRenderTargetDescriptor
    {
        public readonly EBlendOp BlendOp;
        public readonly EBlendMode AlphaDestBlend;
        public readonly EBlendMode AlphaSrcBlend;

        public readonly EBlendOp ColorBlendOp;
        public readonly EBlendMode ColorDestBlend;
        public readonly EBlendMode ColorSrcBlend;

        public readonly EColorWriteMask ColorWriteMask;
    }
}
