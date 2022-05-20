namespace ThinGin.Core.Common.Engine.Interfaces
{
    public interface IEngineDelegate
    {
        /// <summary>
        /// Attempts to invoke the engine delegate
        /// </summary>
        bool TryInvoke();
    }
}
