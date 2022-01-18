using System.Drawing;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.Core.Rendering;
using ThinGin.OpenGL.Common.Framebuffers;

namespace ThinGin.OpenGL.Common.Providers
{
    public class GLFramebufferProvider : IFramebufferProvider
    {
        #region Static Properties
        public static IFramebufferProvider Instance = new GLFramebufferProvider();
        #endregion

        public FrameBuffer Create_FrameBuffer(IRenderEngine Engine, Size Size)
        {
            return new GLFrameBuffer(Engine, Size);
        }
    }
}
