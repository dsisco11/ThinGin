namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IStateMachineObject
    {
        object Identifier { get; }

        /// <summary>
        /// </summary>
        void Push();

        /// <summary>
        /// </summary>
        void Pop();
    }
}
