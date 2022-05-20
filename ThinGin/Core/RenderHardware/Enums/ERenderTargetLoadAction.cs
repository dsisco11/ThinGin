namespace ThinGin.Core.RenderHardware.Enums
{
    /// <summary>
    /// Action to take when a render target is set.
    /// </summary>
    public enum ERenderTargetLoadAction
    {
        /// <summary> Existing content is not preserved. Untouched contents of the render buffer are undefined. </summary>
        NoAction,
        /// <summary> The render buffer is cleared using the value specified </summary>
        Clear,
        /// <summary> Existing contents of the render buffer are preserved </summary>
        Load,
    }
}