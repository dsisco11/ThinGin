using ThinGin.Core.RenderHardware.Pipelines;

namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public abstract class RHIComputeShader : RHIShader
    {
        #region Values
        public readonly PipelineStateStats Stats;
        #endregion

        #region Constructors
        protected RHIComputeShader(in IRHI rhi) : base(rhi, EShaderType.Compute)
        {
            Stats = new PipelineStateStats();
        }
        #endregion
    }
}
