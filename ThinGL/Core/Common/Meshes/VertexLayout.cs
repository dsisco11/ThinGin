
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using ThinGin.Core.Common.Data.Interfaces;
using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Meshes
{
    /// <summary>
    ///  Describes the form and arrangement of elements which compose a type of vertex.
    ///  <note>This structure essentially takes in the set of attributes a vertex will have and then compiles a memory mapping description for where and how to locate said data.</note>
    /// </summary>
    public class VertexLayout
    {
        #region Constants
        private static readonly int ATTRIBUTE_TYPE_ENUM_MAX = System.Enum.GetValues(typeof(EVertexAttribute)).Cast<int>().Max() + 1;
        #endregion

        #region Values
        /// <summary>
        /// Total byte size of the vertex
        /// </summary>
        private int _size = 0;
        private IAttributeDescriptor[] Attributes;

        /// <summary>
        /// Tracks the <see cref="EVertexAttribute"/> of each element
        /// </summary>
        private EVertexAttribute[] _attrType;

        /// <summary>
        /// Lists whether a given <see cref="EVertexAttribute"/> type is present within this vertex format
        /// </summary>
        private bool[] _hasAttributeType;

        /// <summary>
        /// Table which maps attribute types to their element index
        /// </summary>
        private int[] _attrTypeIndexTable = null;

        #endregion

        #region Accessors
        public int Size
        {
            get
            {
                int size = 0;
                for (int i = 0; i < Attributes.Length; i++)
                {
                    var current = Attributes[i];
                    if (current is object)
                    {
                        size += current.Size;
                    }
                }

                return size;
            }
        }
        /// <summary>
        /// Number of attributes which comprise the layout
        /// </summary>
        public int Count => Attributes.Length;
        #endregion

        #region Indexer
        public IAttributeDescriptor this[int index]
        {
            get => Attributes[index];
            set => Attributes[index] = value;
        }
        #endregion

        #region Constructors
        public VertexLayout(params (EVertexAttribute AttributeType, IAttributeDescriptor Descriptor)[] Attributes)
        {
            // First, compile the list of attribute types
            var descriptorList = new List<IAttributeDescriptor>(Attributes.Length);
            var _attrTypeList = new List<EVertexAttribute>(ATTRIBUTE_TYPE_ENUM_MAX + (Attributes?.Length ?? 0));
            for (int i = 0; i < Attributes.Length; i++)
            {
                var Attribute = Attributes[i];
                _attrTypeList.Add(Attribute.AttributeType);
                descriptorList.Add(Attribute.Descriptor);
            }

            _attrType = _attrTypeList.ToArray();
            _map_attribute_indices();
            this.Attributes = descriptorList.ToArray();
        }

        public VertexLayout(IAttributeDescriptor Position = null, IAttributeDescriptor Normal = null, IAttributeDescriptor Color = null, IAttributeDescriptor UV = null, params IAttributeDescriptor[] Attributes)
        {
            // First, compile the list of attribute types
            var _attrTypeList = new List<EVertexAttribute>(ATTRIBUTE_TYPE_ENUM_MAX + (Attributes?.Length ?? 0));

            // Fill in the type value for the common attributes
            if (Position is object) _attrTypeList.Add(EVertexAttribute.Position);
            if (Normal is object) _attrTypeList.Add(EVertexAttribute.Normal);
            if (Color is object) _attrTypeList.Add(EVertexAttribute.Color);
            if (UV is object) _attrTypeList.Add(EVertexAttribute.UVMap);
            // For all other attributes we list their type as 'custom'
            for (int i = 0; i < Attributes.Length; i++) { _attrTypeList.Add(EVertexAttribute.Custom); }

            _attrType = _attrTypeList.ToArray();
            _map_attribute_indices();

            // Populate the elements array
            var list = new List<IAttributeDescriptor>();
            if (Position is object) list.Add(Position);
            if (Normal is object) list.Add(Normal);
            if (Color is object) list.Add(Color);
            if (UV is object) list.Add(UV);
            for (int i = 0; i < Attributes.Length; i++) { list.Add(Attributes[i]); }
            // 
            this.Attributes = list.ToArray();
        }

        private void _map_attribute_indices()
        {
            // Now, map the index order for our vertex elements (what order the ones which are valid occur in)
            // In other words; we are creating a reverse lookup table from attribute type value to element index.
            // This is so we can perform instant lookups of common named attribute types like positions, normals, color, etc.
            _attrTypeIndexTable = new int[ATTRIBUTE_TYPE_ENUM_MAX];
            System.Array.Fill(_attrTypeIndexTable, -1);// Since only the attributes which actually exist are 

            for (int i = 0; i < _attrType.Length; i++)
            {
                int idx = (int)_attrType[i];
                if (_attrTypeIndexTable[idx] > -1)
                    break;

                _attrTypeIndexTable[idx] = i;
            }
        }
        #endregion

        /// <summary>
        /// Consults a lookup table for the element index of an attribute type (eg: the attributes position amongst all other attributes)
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(EVertexAttribute Attribute) => _attrTypeIndexTable[(int)Attribute];


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IAttributeDescriptor Get(EVertexAttribute Attribute, int offset = 0)
        {
            if (_hasAttributeType[(int)Attribute])
            {
                int index = _attrTypeIndexTable[(int)Attribute] + offset;
                if (index < Attributes.Length)
                {
                    return Attributes[index];
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(EVertexAttribute Attribute) => _hasAttributeType[(int)Attribute];


    }
}
