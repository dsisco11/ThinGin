namespace ThinGin.Core.Console
{
    public struct VConsoleChangeArgs
    {
        #region Properties
        public readonly VConsoleLine Line;

        /// <summary>
        /// Offset(inclusive) into the text where the changes begin
        /// </summary>
        public readonly int Start;

        /// <summary>
        /// Number of characters which changed
        /// </summary>
        public readonly int Length;
        #endregion

        #region Constructors
        public VConsoleChangeArgs(VConsoleLine line, int start, int length)
        {
            Line = line;
            Start = start;
            Length = length;
        }
        #endregion
    }
}
