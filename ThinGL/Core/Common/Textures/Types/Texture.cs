
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Textures.Types
{
    /// <summary>
    ///  Stores the actual OpenGL texture instance and manages proper disposal once all users have released it.
    /// </summary>
    public abstract class Texture : GObject, ITexture
    {
        #region Values
        protected int _id = 0;
        #endregion

        #region Properties
        public readonly PixelDescriptor GpuLayout;

        /// <summary> <inheritdoc cref="ITexture.handle"/> </summary>
        public virtual int Handle { get => _id; }

        /// <summary>
        /// A textures priority is a number indicating its importance, eg; a number which indicates its unload order when clearing memory for resource management. Resources are unloaded in order according to their priority, with lowest priority numbers being first.
        /// <para>Higher values indicate lower chance of being forced to unload in resource constrained scenarios.</para>
        /// </summary>
        /// <returns>Integer representing position in the unloading order</returns>
        public int Priority { get; set; } = 0;

        public TextureMetadata Metadata { get; protected set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new GPU texture
        /// </summary>
        protected Texture(IEngine Engine, PixelDescriptor GpuLayout) : base(Engine)
        {
            this.GpuLayout = GpuLayout;
        }
        #endregion

        public abstract bool TryLoad(TextureMetadata Metadata, byte[] RawPixels);

    }
}
