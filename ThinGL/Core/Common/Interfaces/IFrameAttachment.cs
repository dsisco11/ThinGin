
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Rendering;

namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// Implements a generic attachment object for an OpenGL framebuffer
    /// <para>This enables seamless support for renderbuffers vs textures</para>
    /// </summary>
    public interface IFrameAttachment : IEngineObject
    {
        FrameBuffer Owner { get; }
        bool IsAttached { get; }
        int TypeId { get; }

        void Attach(FrameBuffer frameBuffer, int SlotNo);
        void Detach(FrameBuffer frameBuffer);
    }
}
