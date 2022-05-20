
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.Common.Textures.Types
{
    /// <summary>
    ///  Stores the actual GPU texture instance and manages proper disposal once all users have released it.
    /// </summary>
    public abstract class Texture : RHITexture, ITexture
    {
        #region Values
        #endregion

        #region Properties
        #endregion

        #region Constructors
        // TODO: Need to convert PixelDescriptors into EPixelFormat enum values
        protected Texture(EngineInstance engine, PixelDescriptor GpuLayout) : base(engine.Renderer.RHI, GpuLayout)
        {
        }
        #endregion

        public abstract bool TryLoad(TextureDescriptor Metadata, byte[] RawPixels);

    }
}
