using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Data.Interfaces
{
    public interface IDataDescriptor : IDataBlockDescriptor
    {
        /// <summary>
        /// Number of values the data structure contains
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Size (in bytes) of the value type
        /// </summary>
        int ValueSize { get; }

        /// <summary>
        /// Type of value stored by the data structure
        /// </summary>
        EValueType ValueType { get; }
    }
}