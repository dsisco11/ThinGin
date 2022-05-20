
using ThinGin.Core.RenderHardware;
using ThinGin.Core.RenderHardware.Synchronization;

namespace ThinGin.OpenGL.OpenGL.RenderHardware.Synchronization
{
    public class OpenGLFenceObject : RHIFenceObject
    {
        #region Values
        #endregion

        #region Constructors
        public OpenGLFenceObject(RenderHardwareInterface rhi) : base(rhi)
        {
        }


        public override RHIDelegate Get_Initializer()
        {
            throw new System.NotImplementedException();
        }

        public override RHIDelegate Get_Releaser()
        {
            throw new System.NotImplementedException();
        }

        protected override bool Poll()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
