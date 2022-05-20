namespace ThinGin.Core.Common.Enums
{
    public enum ECameraViewMode
    {
        /// <summary>
        /// The cameras view is "first person" such that it is positioned at an origin point and rotates to look around.
        /// </summary>
        FirstPerson,

        /// <summary>
        /// The cameras view is orbiting around, and always facing toward, its origin point. But the orbital range of rotation for the camera is constrained to a half sphere.
        /// <note>Essentially this mode ensures that the cameras 'up' vector never points in the opposite direction of the 'world up' vector.</note>
        /// </summary>
        Orbital,

        /// <summary>
        /// The cameras view is orbiting around, and always facing toward, its origin point.
        /// </summary>
        Orbital_Free,
    }
}
