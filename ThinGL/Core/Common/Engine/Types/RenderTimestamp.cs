namespace ThinGin.Core.Common.Engine
{
    /// <summary>
    /// Used to track timing deltas within the engine in an extensible way
    /// </summary>
    public class RenderTimestamp
    {
        #region Values
        private double value;
        #endregion

        #region Constructors
        public RenderTimestamp()
        {
        }
        #endregion

        public double Get()
        {
            return value;
        }

        public void Set(double value)
        {
            this.value = value;
        }
    }
}
