
using ThinGin.Core.RenderHardware.Types;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHIVertexDecleration
    {
        #region Values
        public readonly ushort[] Strides;
        public readonly VertexElement[] Elements;
        #endregion

        #region Constructors
        public RHIVertexDecleration(in VertexElement[] elements, in ushort[] strides)
        {
            Elements = elements;
            Strides = strides;
        }
        #endregion
    }
}
