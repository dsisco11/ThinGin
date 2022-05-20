
using ThinGin.Core.RenderHardware.Enums;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware.Context
{
    public readonly struct AccelerationStructureBuildParams
    {
        public readonly EAccelerationStructureBuildMode BuildMode;
        public readonly RHIRayTracingGeometry Geometry;
        /// <summary>
        /// Optional set of geometry segments which can be used to change per-segment vertex buffers. Only the fields related to vertex buffers are used.
        /// </summary>
        public readonly RHIRayTracingGeometrySegment[] Segments;
    }
}