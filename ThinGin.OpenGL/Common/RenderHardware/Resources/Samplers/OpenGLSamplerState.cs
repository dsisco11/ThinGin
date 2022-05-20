
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.OpenGL.Common.RenderHardware.Resources.Samplers
{
    public class OpenGLSamplerState : RHISamplerState
    {
        #region Values
        public readonly OpenGLSamplerStateData Data;
        #endregion

        #region Properties
        public readonly RHIHandle Handle;
        #endregion

        #region Accessors
        public override bool IsImmutable => true;
        #endregion

        #region Constructors
        #endregion
        public OpenGLSamplerState(RenderHardwareInterface rhi, RHIHandle sampler, OpenGLSamplerStateData data) : base(rhi)
        {
            Handle = sampler;
            Data = data;
        }
    }
}
