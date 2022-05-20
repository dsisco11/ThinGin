using ThinGin.Core.RenderHardware.Enums;

namespace ThinGin.Core.RenderHardware.Resources.Rasterizer
{
    public readonly ref struct RasterizerStateConfig
    {
        #region Values
        public readonly bool AllowMSAA;
        public readonly bool EnableLineAA;
        public readonly float DepthBias;
        public readonly float SlopeScaleDepthBias;
        public readonly ERasterCullMode CullMode;
        public readonly ERasterFillMode FillMode;
        #endregion

    }
}
