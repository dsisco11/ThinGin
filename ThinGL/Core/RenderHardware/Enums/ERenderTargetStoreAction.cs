namespace ThinGin.Core.RenderHardware.Enums
{
    /// <summary>
    /// Action to take at the end of a pass OR when a render target is unset.
    /// </summary>
    public enum ERenderTargetStoreAction
    {
        /// <summary> Contents of the render target emitted during the pass are NOT stored back into memory </summary>
        NoAction,
        /// <summary> Contents of the render target emitted during the pass are stored back into memory. </summary>
        Store,
        /// <summary> Contents of the render target emitted during the pass are resolved and stored back into memory. </summary>
        ResolveMultisample,
    }
}