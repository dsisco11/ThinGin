using System;
using System.Collections.Generic;
using System.Numerics;

using ThinGin.Core.Common.Types;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware.Context
{
    /// <summary>
    /// An RHI context that is capable of processing compute work.
    /// </summary>
    public interface IRHIComputeContext
    {
        IRHIComputeContext GetHighestLevelContext();
        IRHIComputeContext GetLowestLevelContext();

        void BeginTransitions(IEnumerable<RHIResourceTransition> Transitions);

        void BeginUAVOverlap(IEnumerable<RHIUnorderedAccessView> Views);
        void RHIBeginUAVOverlap();

        //
        void BuildAccelerationStructure(in RHIRayTracingGeometry Geometry);
        void BuildAccelerationStructure(in RHIRayTracingScene Scene);
        void BuildAccelerationStructures(params AccelerationStructureBuildParams[] Items);

        //
        void ClearUAVUInt(in RHIUnorderedAccessView View, ReadOnlySpan<uint> Values);
        void ClearUAVFloat(in RHIUnorderedAccessView View, ReadOnlySpan<float> Values);

        /// <summary>
        /// Performs a copy of the data in <paramref name="Source"/> to <paramref name="Destination"/>.
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        /// <param name="Offset"></param>
        /// <param name="NumBytes"></param>
        void CopyToStagingBuffer(in RHIVertexBuffer Source, in RHIStagingBuffer Destination, uint Offset, uint NumBytes);

        void DispatchComputeShader(uint ThreadGroupCountX, uint ThreadGroupCountY, uint ThreadGroupCountZ);
        void DispatchIndirectComputeShader(in RHIVertexBuffer ArgumentBuffer, uint ArgumentOffset);


        void EndTransitions(IEnumerable<RHIResourceTransition> Transitions);

        void EndUAVOverlap();
        void EndUAVOverlap(IEnumerable<RHIUnorderedAccessView> Views);

        object GetNativeCommandBuffer();

        /// <summary>
        /// Signals to the RHI that its cached state is no longer valid. This is used for RHI implementations which cache the render state internally (eg: OpenGL)
        /// </summary>
        void InvalidateCachedState();

        void PopEvent();
        void PostExternalCommandsReset();
        void PushEvent(string Name, Rgba Color);

        void SetAsyncComputeBudget(EAsyncComputeBudget Hint);
    }
}
