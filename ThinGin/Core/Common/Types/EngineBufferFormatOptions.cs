namespace ThinGin.Core.Common.Types
{
    /// <summary>
    /// Provides information about any limitations for specifying memory layout which the engine is currently subject to...
    /// </summary>
    public struct EngineBufferFormatOptions
    {
        #region Values
        /// <summary>
        /// Support for applying a divisor to supplied indices in order to obtain a final index for an element or attribute.
        /// </summary>
        public readonly bool IndexDivisor;
        #endregion

        #region Constructors
        public EngineBufferFormatOptions(bool indexDivisor)
        {
            IndexDivisor = indexDivisor;
        }
        #endregion
    }
}
