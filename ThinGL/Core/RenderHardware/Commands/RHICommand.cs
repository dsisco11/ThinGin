namespace ThinGin.Core.RenderHardware.Commands
{
    /// <summary>
    /// Base class for all executable RHI commands
    /// </summary>
    public abstract class RHICommand
    {
        #region Properties
        #endregion

        #region Constructors
        public RHICommand()
        {
        }
        #endregion

        public abstract void Execute();
    }
}
