
namespace ThinGin.Core.Common.Data.Interfaces
{
    /// <summary>
    /// Describes a data blocks aligment relative to others
    /// </summary>
    public interface IDataAlignmentDescriptor : IDataBlockDescriptor
    {
        /// <summary>
        /// Number of bytes this data structure is offset within its parent data structure.
        /// <note>Auto computed by the engine if using data builders like the <see cref="Meshes.MeshBuilder"/></note>
        /// </summary>
        int Offset { get; set; }

        /// <summary>
        /// Number of bytes between consecutive instances of data represented by this descriptor.
        /// <note>Auto computed by the engine if using data builders like the <see cref="Meshes.MeshBuilder"/></note>
        /// </summary>
        int Stride { get; set; }

        /// <summary>
        /// Indicates whether the data will be interlaced amongst other data in a compound structure.
        /// <note>This is really meant to be used by the engine systems, users shouldn't have to care about final buffer structure, 
        /// but if you do then setting this to false prevents the engine from interlacing the data subject to this particular descriptor.</note>
        /// </summary>
        bool Interleaved { get; set; }
    }
}