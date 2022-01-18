namespace ThinGin.Core.Common.Enums
{
    public enum ECullingMode
    {
        /// <summary> Both the front and back faces of primitives will be unculled </summary>
        None = 0,

        /// <summary> Front face of primitives will be culled </summary>
        Front = 1,

        /// <summary> Back face of primitives will be culled </summary>
        Back = 2,
    }
}
