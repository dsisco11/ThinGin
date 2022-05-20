namespace ThinGin.Core.Common.Interfaces
{
    /// <summary>
    /// Represents some graphics library state object that can be bound and unbound, or additionally notified of a change in its bind state (eg; something else was bound and thus this object was unbound).
    /// <para>Being 'bound' is in reference to the gpu state machine. Binding changes which object some set of gpu calls references, such as a mesh, texture, shader, or other object.</para>
    /// </summary>
    public interface IEngineBindable
    {

        /// <summary>
        /// Binds the object into use
        /// </summary>
        void Bind();

        /// <summary>
        /// Unbinds the object from use
        /// </summary>
        void Unbind();

        /// <summary>
        /// Notifies the bindable object that it has been bound
        /// </summary>
        void Bound();

        /// <summary>
        /// Notifies the bindable object that it has been unbound
        /// </summary>
        void Unbound();
    }
}
