using System.Drawing;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Providers;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.Rendering;
using ThinGin.OpenGL.Common.Framebuffers;

namespace ThinGin.OpenGL.Common.Providers
{
    public class GLFramebufferProvider : IFramebufferProvider
    {
        #region Static Properties
        public static IFramebufferProvider Instance = new GLFramebufferProvider();
        #endregion

        public GBuffer Create_FrameBuffer(EngineInstance Engine, Size Size)
        {
            return new GLFrameBuffer(Engine, Size);
        }
    }
}
