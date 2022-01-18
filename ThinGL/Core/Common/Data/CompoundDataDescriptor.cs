using System.Collections.Generic;

using ThinGin.Core.Common.Data.Interfaces;

namespace ThinGin.Core.Common.Data
{
    /// <summary>
    /// A compound data structure constructed from multiple <see cref="AttributeDescriptor"/> instances.
    /// </summary>
    public class CompoundDataDescriptor : IDataAlignmentDescriptor
    {
        #region Values
        private int _size;
        private int _count;
        private int _offset;
        private int _stride;
        #endregion

        #region Properties
        public readonly IAttributeDescriptor[] Elements = null;
        #endregion

        #region Accessors
        /// <summary>
        /// <inheritdoc cref="IDataAlignmentDescriptor.Size"/>
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// <inheritdoc cref="IDataAlignmentDescriptor.Count"/>
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// <inheritdoc cref="IDataAlignmentDescriptor.Offset"/>
        /// </summary>
        public int Offset { get => _offset; set => _offset = value; }

        /// <summary>
        /// <inheritdoc cref="IDataAlignmentDescriptor.Stride"/>
        /// </summary>
        public int Stride { get => _stride; set => _stride = value; }

        /// <summary>
        /// <inheritdoc cref="IDataAlignmentDescriptor.Interleaved"/>
        /// </summary>
        public bool Interleaved { get; set; } = true;
        #endregion

        #region Indexer
        public IAttributeDescriptor this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }
        #endregion

        #region Constructors
        public CompoundDataDescriptor(params IAttributeDescriptor[] Elements)
        {
            _size = 0;
            _count = 0;
            _offset = 0;

            var list = new List<IAttributeDescriptor>();
            for (int i = 0; i < Elements.Length; i++)
            {
                var E = Elements[i];
                if (E is object)
                {
                    list.Add(E);
                    _size += E.Size;
                    _count += E.Count;
                }
            }

            this.Elements = list.ToArray();
        }
        #endregion
    }
}
