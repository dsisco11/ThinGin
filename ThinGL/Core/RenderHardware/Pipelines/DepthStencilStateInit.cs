using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.RenderHardware.Pipelines
{
    public readonly ref struct DepthStencilStateInit
    {
        public readonly bool EnableDepthWrite;
        public readonly ECompareMode DepthTest;

        public readonly byte StencilReadMask;
        public readonly byte StencilWriteMask;

        public readonly bool EnableBackFaceStencil;
        public readonly ECompareMode BackFaceStencilTest;
        public readonly EStencilOp BackFaceDepthFailStencilOp;
        public readonly EStencilOp BackFacePassStencilOp;
        public readonly EStencilOp BackFaceStencilFailStencilOp;

        public readonly bool EnableFrontFaceStencil;
        public readonly EStencilOp FrontFaceDepthFailStencilOp;
        public readonly EStencilOp FrontFacePassStencilOp;
        public readonly EStencilOp FrontFaceStencilFailStencilOp;
        public readonly ECompareMode FrontFaceStencilTest;
    }
}