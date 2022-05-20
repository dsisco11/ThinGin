namespace ThinGin.Core.Common.Objects
{
    [System.Flags]
    public enum EObjectFlags
    {
        /// <summary> This object is the child of another object </summary>
        Parented = 0x1,
        /// <summary> This object implements update logic through the updater delegate system </summary>
        HasUpdater = 0x2,
    }
}
