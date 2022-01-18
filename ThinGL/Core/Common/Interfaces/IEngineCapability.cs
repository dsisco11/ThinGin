namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// Represents a GPU capability state that can be enabled or disabled. Examples include; Color Blending, Texturing, Alpha testing, Depth testing, etc.
    /// <para>The engine system is built this way because many modern GPU systems do not function via this fixed pipeline method, rather requiring the developer to perform these actions themselves within shaders or other custom code uploaded to the GPU.</para>
    /// </summary>
    public interface IEngineCapability : IStateMachineObject
    {
        /// <summary>
        /// Enables the capability
        /// </summary>
        void Enable();

        /// <summary>
        /// Disables the capability
        /// </summary>
        void Disable();
    }
}
