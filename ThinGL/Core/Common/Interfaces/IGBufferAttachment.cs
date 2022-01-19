
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Rendering;

namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// Implements a generic attachment object for an OpenGL framebuffer
    /// <para>This enables seamless support for renderbuffers vs textures</para>
    /// </summary>
    public interface IGBufferAttachment : IGraphicsObject
    {
        GBuffer Owner { get; }
        bool IsAttached { get; }
        int TypeId { get; }

        void Attach(GBuffer frameBuffer, int SlotNo);
        void Detach(GBuffer frameBuffer);
    }
}
