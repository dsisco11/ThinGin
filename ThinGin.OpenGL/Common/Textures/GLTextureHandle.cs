using ThinGin.Core.Common.Textures.Types;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.Textures
{
    public class GLTextureHandle : TextureHandle
    {
        #region Values
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public GLTextureHandle(EngineInstance engine) : base(engine)
        {
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => { OpenTK.Graphics.OpenGL.GL.GenTextures(1, out _handle); });
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(_handle, (int h) => { OpenTK.Graphics.OpenGL.GL.DeleteTexture(h); h = 0; });
        #endregion
    }
}
