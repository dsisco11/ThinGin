#define USING_INVERTED_Z_BUFFER
namespace ThinGin.Core.RenderHardware.Pipelines
{
    public enum ECompareMode
    {
        Less,
        LessEqual,
        Greater,
        GreaterEqual,
        Equal,
        NotEqual,
        Never,
        Always,

#if USING_INVERTED_DEPTH_BUFFER
        DepthNearOrEqual = GreaterEqual,
        DepthNear = Greater,
        DepthFartherOrEqual = LessEqual,
        DepthFarther = Less,
#else
        DepthNearOrEqual = LessEqual,
        DepthNear = Less,
        DepthFartherOrEqual = GreaterEqual,
        DepthFarther = Greater,
#endif
    }
}