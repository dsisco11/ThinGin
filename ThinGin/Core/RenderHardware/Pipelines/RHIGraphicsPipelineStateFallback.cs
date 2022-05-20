namespace ThinGin.Core.RenderHardware.Pipelines
{
    /// <summary>
    /// This pipeline state object is a fallback for RHIs that do not support pipelines, it will set the graphics state using state machine type calls.
    /// </summary>
    public abstract class RHIGraphicsPipelineStateFallback : RHIGraphicsPipelineState
    {
        #region Values
        #endregion

        #region Constructors
        public RHIGraphicsPipelineStateFallback(in GraphicsPipelineStateInit State)
        {
        }
        #endregion
    }
}
