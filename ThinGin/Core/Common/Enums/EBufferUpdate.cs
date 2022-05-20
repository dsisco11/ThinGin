namespace ThinGin.Core.Common.Enums
{
    /// <summary>
    /// Describes how often a buffers data will be changed
    /// </summary>
    public enum EBufferUpdate
    {
        /// <summary>
        /// Buffer should be uploaded to about once
        /// </summary>
        Static,
        /// <summary>
        /// Buffer can be uploaded to a bunch of times over its lifespan
        /// </summary>
        Dynamic,
        /// <summary>
        /// Buffer uploaded to constantly
        /// </summary>
        Streaming
    }
}
