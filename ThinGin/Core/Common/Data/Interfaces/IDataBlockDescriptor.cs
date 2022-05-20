namespace ThinGin.Core.Common.Data.Interfaces
{
    /// <summary>
    /// Describes a block of data that has a specific size
    /// </summary>
    public interface IDataBlockDescriptor
    {
        /// <summary>
        /// Total size (in bytes) of the data structure
        /// </summary>
        int Size { get; }
    }
}