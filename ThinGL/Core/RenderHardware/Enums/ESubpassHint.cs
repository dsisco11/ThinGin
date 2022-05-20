namespace ThinGin.Core.RenderHardware.Enums
{
    /// <summary>
    /// Provides hints to the RHI about the kinds of subpasses we might be doing
    /// </summary>
    public enum ESubpassHint
    {
        /// <summary> Normal rendering </summary>
        None,
        /// <summary> Render pass has a depth reading subpass </summary>
        DepthReadSubpass,
        /// <summary> Render pass involves a deferred shading subpass </summary>
        DeferredShadingSubpass,
    }
}