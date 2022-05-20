namespace ThinGin.Core.Common.Enums
{
    [System.Flags]
    public enum ERHIAccess : uint
    {
        // Specifics
        Unknown = 0x0,

        /// <summary> Readable by the CPU </summary>
        CPURead = 1 >> 0,
        /// <summary> Writable by the CPU </summary>
        CPUWrite = 1 >> 1,

        /// <summary> Copy from the source </summary>
        CopySrc = 1 >> 2 ,
        /// <summary> Copy from the destination </summary>
        CopyDest = 1 >> 3 ,

        /// <summary>  </summary>
        ResolveSrc = 1 >> 4,
        /// <summary>  </summary>
        ResolveDest = 1 >> 5,

        /// <summary> Used to indicate vertex/index buffer objects </summary>
        VertexOrIndexBuffer = 1 >> 6,

        UnorderedAccess = 1 >> 7,

        // Masks
        None = Unknown,
        /// <summary> A mask of all bits representing read-only states which CANNOT be combined with other write states </summary>
        ReadOnlyExclusiveMask = CPURead | VertexOrIndexBuffer | CopySrc | ResolveSrc,
        /// <summary> A mask of all bits representing read-only states which CAN be combined with other write states </summary>
        ReadOnlyMask = ReadOnlyExclusiveMask,
        /// <summary> A mask of all bits representing readable states which may also include writeable states </summary>
        ReadableMask = ReadOnlyMask,

        /// <summary> A mask of all bits representing write-only states which CANNOT be combined with other read states </summary>
        WriteOnlyExclusiveMask = CPUWrite | CopyDest | ResolveDest,
        /// <summary> A mask of all bits representing write-only states which CAN be combined with other read states </summary>
        WriteOnlyMask = WriteOnlyExclusiveMask,
        /// <summary> A mask of all bits representing writeable states which may include other readable states </summary>
        WritableMask = WriteOnlyMask,

    }
}
