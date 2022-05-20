namespace ThinGin.Core.Console.Commands
{
    /// <summary>
    /// Marks a method as a handler for a virtual console command
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CommandHandler : System.Attribute
    {
        public CommandHandler()
        {
        }
    }
}
