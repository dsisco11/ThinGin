using System;
using System.Runtime.InteropServices;

using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.RenderHardware.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 64)]
    public readonly struct VertexElement
    {
        #region Values
        public readonly UInt16 Stride;
        public readonly byte Offset;
        /// <summary> Index of the buffer (vertex stream) that this element resides within </summary>
        public readonly byte StreamIndex;
        public readonly byte AttributeIndex;
        /// <summary> Whether to use the instance index or the vertex index to consume the element </summary>
        public readonly bool UseInstanceIndex;
        public readonly EValueType Type;
        #endregion
    }
}
