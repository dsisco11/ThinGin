namespace ThinGin.Core.Common.Enums
{
    /// <summary>
    /// Specifies various properties of the rendering engine which are tracked via state-machine
    /// </summary>
    public enum EStateMachineProperty : uint
    {
        NULL = 0,
        Color,
        TexturingState,
        /// <summary> EG: <see cref="OpenTK.Graphics.OpenGL.TextureTarget.Texture2D"/> </summary>
        TexturingTarget,
        /// <summary> EG: <see cref="OpenTK.Graphics.OpenGL.TextureUnit.Texture0"/> </summary>
        TextureSlot,
        MatrixMode,
        /// <summary>
        /// Is color blending enabled
        /// </summary>
        BlendingState,
        /// <summary>
        /// is alpha blending enabled
        /// </summary>
        AlphaTestState,
    }
}