using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Data
{
    /// <summary>
    /// Describes some generic piece of data with a name and value type.
    /// </summary>
    public class NamedDataDescriptor : AttributeDescriptor
    {
        #region Values
        public string Name;
        #endregion

        #region Constructors
        /// <summary> <inheritdoc cref="AttributeDescriptor()"/> </summary>
        public NamedDataDescriptor(string Name)
        {
            this.Name = Name;
        }

        /// <summary> <inheritdoc cref="AttributeDescriptor(EValueType)"/> </summary>
        public NamedDataDescriptor(string Name, EValueType ValueType) : base(ValueType)
        {
            this.Name = Name;
        }

        /// <summary> <inheritdoc cref="AttributeDescriptor(int, EValueType)"/> </summary>
        public NamedDataDescriptor(string Name, int Count, EValueType ValueType) : base(Count, ValueType)
        {
            this.Name = Name;
        }

        /// <summary> <inheritdoc cref="SimpleDataDescriptor(int, EValueType, int)"/> </summary>
        public NamedDataDescriptor(string Name, int Count, EValueType ValueType, int ByteOffset = 0, int Stride = 0) : base(Count, ValueType, ByteOffset, Stride)
        {
            this.Name = Name;
        }
        #endregion
    }
}
