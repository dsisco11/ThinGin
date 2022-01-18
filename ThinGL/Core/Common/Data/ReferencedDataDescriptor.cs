using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Data
{
    /// <summary>
    /// Describes some piece of data with an identity, referenced with a handle and assigned a name.
    /// </summary>
    public class ReferencedDataDescriptor : NamedDataDescriptor
    {
        #region Values
        public object Handle;
        /// <summary>
        /// Additional contextual information to aid in the native implementation of the data structure
        /// </summary>
        public object Context;
        #endregion

        #region Constructors
        /// <summary> <inheritdoc cref="NamedDataDescriptor(string, int, EValueType, int, int)"/> </summary>
        public ReferencedDataDescriptor(object Handle, object Context = null, string Name = null, int Count = 1, EValueType ValueType = EValueType.NULL, int ByteOffset = 0, int Stride = 0) : base(Name, Count, ValueType, ByteOffset, Stride)
        {
            this.Handle = Handle;
            this.Context = Context;
        }
        #endregion
    }
}
