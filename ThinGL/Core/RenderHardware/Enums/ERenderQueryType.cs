namespace ThinGin.Core.RenderHardware.Resources.Queries
{
    public enum ERenderQueryType
    {
        /// <summary>
        /// Generic, stuff like waiting for frame sync
        /// </summary>
        Undefined,
        /// <summary>
        /// Result is number of samples rendered, eg not culled.
        /// </summary>
        Occlusion,
        /// <summary>
        /// Result is current time in micro seconds = 1/1000 ms = 1/1000000 sec (not a duration).
        /// </summary>
        AbsoluteTime,
    }
}
