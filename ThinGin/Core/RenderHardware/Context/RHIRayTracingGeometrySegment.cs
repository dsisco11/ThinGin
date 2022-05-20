using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware.Context
{
    public readonly struct RHIRayTracingGeometrySegment
    {
        /// <summary> Indicates whether this section is enabled and should be taken into account during acceleration structure creation </summary>
        public readonly bool Enabled;
        /// <summary> Indicates whether the any-hit shader could be invoked when hitting this geometry </summary>
        public readonly bool ForceOpaque;
        /// <summary> The any-hit shader may be invoked multipler times for the same primitive during ray traversal </summary>
        public readonly bool AllowDuplicateAnyHitShaderInvocation;

        public readonly uint FirstPrimitive;
        public readonly uint NumPrimitives;

        /// <summary> Offset (in bytes) from the base address of the vertex buffer </summary>
        public readonly uint VertexBufferOffset;
        /// <summary> Offset (in bytes) between elements of the vertex buffer </summary>
        public readonly uint VertexBufferStride;

        public readonly RHIVertexBuffer VertexBuffer;
        public readonly uint VertexElementCount;
        public readonly EValueType VertexElementType;
    }
}