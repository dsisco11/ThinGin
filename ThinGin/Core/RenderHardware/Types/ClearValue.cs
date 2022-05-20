using System.Numerics;

namespace ThinGin.Core.RenderHardware.Types
{
    /// <summary>
    /// There really isnt a better name for this, it tracks how and what kind of GBuffer data to clear
    /// </summary>
    public readonly struct ClearValue
    {
        #region Constants
        public static ClearValue None = new ClearValue();
        public static ClearValue Transparent = new ClearValue(new Vector4(0f, 0f, 0f, 0f));
        public static ClearValue Black = new ClearValue(new Vector4(0f, 0f, 0f, 1f));
        public static ClearValue White = new ClearValue(new Vector4(1f, 1f, 1f, 1f));

        public static ClearValue DepthOne = new ClearValue(1f, 0);
        public static ClearValue DepthZero = new ClearValue(0f, 0);

        public static ClearValue DepthFar = DepthZero;
        public static ClearValue DepthNear = DepthOne;
        #endregion

        #region Values
        public readonly EClearType Binding;
        public readonly float[] Color;
        public readonly float Depth;
        public readonly uint Stencil;
        #endregion

        #region Constructors
        public ClearValue(EClearType binding = EClearType.None)
        {
            Binding = binding;
            Color = null;
            Depth = 0f;
            Stencil = 0;
        }

        public ClearValue(Vector4 clearColor)
        {
            Binding = EClearType.Color;
            Color = new float[4] { clearColor.X, clearColor.Y, clearColor.Z, clearColor.W };
            Depth = 0f;
            Stencil = 0;
        }

        public ClearValue(float depthClearValue, uint stencilClearValue)
        {
            Binding = EClearType.DepthStencil;
            Color = null;
            Depth = depthClearValue;
            Stencil = stencilClearValue;
        }
        #endregion
    }
}
