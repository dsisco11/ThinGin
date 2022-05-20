using ThinGin.Core.RenderHardware.Resources.Shaders;

namespace ThinGin.Core.RenderHardware.Compute
{
    public class RHIComputePipelineStateFallback
    {
        #region Values
        public readonly RHIComputeShader ComputeShader;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIComputePipelineStateFallback(in RHIComputeShader ComputeShader)
        {
            this.ComputeShader = ComputeShader;
        }
        #endregion

    }
}
