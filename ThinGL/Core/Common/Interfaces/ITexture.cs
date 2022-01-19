using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Textures;

namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// An OpenGL texture object with a set Size and ID
    /// </summary>
    public interface ITexture : IGraphicsObject
    {
        /// <summary> Our graphics system texture handle (in OpenGL it is the texture ID). </summary>
        int Handle { get; }

        /// <summary> Holds information about the format of the underlying texture data. </summary>
        TextureMetadata Metadata { get; }
    }
}
