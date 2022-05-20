namespace ThinGin.Core.Common.Data.Interfaces
{
    /// <summary>
    /// Describes a (vertex) attributes memory layout
    /// </summary>
    public interface IAttributeDescriptor : IDataDescriptor, IDataAlignmentDescriptor
    {
        /// <summary>
        /// (If supported) The divisor to apply to index numbers in order to arrive at a final element index to reference.
        /// <note>This is meant to be used by the engine itself, for constructing complex but compact data buffers.</note>
        /// </summary>
        int IndexDivisor { get; set; }

        /// <summary>
        /// Indicates whether the values will be normalized and converted into floating point values on the GPU ([0-1] range for unsigned value types and [-1, 1] for signed types)
        /// </summary>
        bool Normalize { get; set; }
    }
}