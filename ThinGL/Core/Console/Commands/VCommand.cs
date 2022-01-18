namespace ThinGin.Core.Console.Commands
{
    /// <summary>
    /// To create a new console command, first, inherit from this base class.
    /// Then define your commands logic in a series of overloaded <see langword="static"/> functions marked with the <see cref="CommandHandler"/> attribute.
    /// </summary>
    public abstract class VCommand
    {
        #region Constructors
        public VCommand()
        {
        }
        #endregion
    }
}
