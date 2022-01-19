using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;

namespace ThinGin.OpenGL.Common.Textures
{
    public class GLTextureHandle : TextureHandle
    {
        #region Values
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public GLTextureHandle(IEngine Engine) : base(Engine)
        {
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => { OpenTK.Graphics.OpenGL.GL.GenTextures(1, out _handle); });
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(_handle, (int h) => { OpenTK.Graphics.OpenGL.GL.DeleteTexture(h); h = 0; });
        #endregion
    }
}
