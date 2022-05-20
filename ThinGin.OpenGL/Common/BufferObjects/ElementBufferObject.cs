
using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Exceptions;
using ThinGin.Core.Common.Data.Interfaces;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.BufferObjects
{
    /// <summary>
    /// </summary>
    public class ElementBufferObject : GLBufferObject
    {
        #region Values
        private DrawElementsType _elementType;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        public override BufferTarget Target => BufferTarget.ElementArrayBuffer;

        public DrawElementsType ElementType { get => _elementType; set => _elementType = value; }
        #endregion

        #region Constructors
        public ElementBufferObject(EngineInstance Engine) : base(Engine)
        {
            if (!Engine.Renderer.IsSupported("arb_vertex_buffer_object"))
            {
                throw new Exceptions.OpenGLUnsupportedException(nameof(ElementBufferObject), "The element buffer object extension is not supported by the graphics driver!");
            }
        }
        #endregion

        #region Uploading
        public override void Upload(byte[] Data, IDataDescriptor Descriptor)
        {
            base.Upload(Data, Descriptor);

            ElementType = Get_Element_Type(Descriptor.ValueType);
        }
        #endregion

        protected DrawElementsType Get_Element_Type(EValueType DataType)
        {
            return DataType switch
            {
                EValueType.UInt8 => DrawElementsType.UnsignedByte,
                EValueType.UInt16 => DrawElementsType.UnsignedShort,
                EValueType.UInt32 => DrawElementsType.UnsignedInt,
                _ => throw new ThinGinException($"Invalid data type for element buffer object! (Type: {DataType})"),
            };
        }
    }
}
