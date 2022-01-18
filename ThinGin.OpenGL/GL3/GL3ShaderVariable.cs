#define USE_SPANS

using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Shaders;
using ThinGin.OpenGL.Common.Exceptions;
using ThinGin.OpenGL.Common.Shaders;

namespace ThinGin.OpenGL.GL3
{
    /// <summary>
    /// <inheritdoc cref="GLShaderVariable"/>
    /// </summary>
    public class GL3ShaderVariable : GLShaderVariable
    {
        #region Constructors
        /// <summary>
        /// <inheritdoc cref="GLShaderVariable(ShaderVariable)"/>
        /// </summary>
        public GL3ShaderVariable(ShaderVariable other) : base(other)
        {
        }

        /// <summary>
        /// <inheritdoc cref="GLShaderVariable(ShaderVariable, ReferencedDataDescriptor)"/>
        /// </summary>
        public GL3ShaderVariable(ShaderVariable other, ReferencedDataDescriptor Descriptor) : base(other, Descriptor)
        {
        }

        /// <summary>
        /// <inheritdoc cref="GLShaderVariable(IShaderInstance, ReferencedDataDescriptor)"/>
        /// </summary>
        public GL3ShaderVariable(IShaderInstance Shader, ReferencedDataDescriptor Descriptor) : base(Shader, Descriptor)
        {
        }
        #endregion

        protected override DataChunk PreProcess(DataChunk dataChunk)
        {
            // OpenGL only supports certain value types so we convert all other types into one of those
            switch (dataChunk.Type)
            {
                case EValueType.SBYTE:
                case EValueType.SHORT:
                    return new DataChunk(dataChunk.ToInt());
                case EValueType.BYTE:
                case EValueType.USHORT:
                    return new DataChunk(dataChunk.ToUInt());
                case EValueType.FLOAT_HALF:// We dont support halfs yet because .net
                case EValueType.DOUBLE:
                    return new DataChunk(dataChunk.ToFloat());
            }

            return dataChunk;
        }

        protected unsafe override DataSubmissionDelegate getSubmissionDelegate()
        {
            const bool Transpose = false;
            switch (Descriptor.ValueType)
            {// The ComponentCount of our descriptor tells us how many components are in the variable data type (eg: 2 for vec2)
                case EValueType.INT:
                    {
#if USE_SPANS
                        return Descriptor.Count switch
                        {
                            1 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { int* ptr = (int*)sptr; GL.Uniform1(ID, Descriptor.Count, ptr); } },
                            2 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { int* ptr = (int*)sptr; GL.Uniform2(ID, Descriptor.Count, ptr); } },
                            3 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { int* ptr = (int*)sptr; GL.Uniform3(ID, Descriptor.Count, ptr); } },
                            4 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { int* ptr = (int*)sptr; GL.Uniform4(ID, Descriptor.Count, ptr); } },
                            _ => throw new OpenGLUnsupportedException(UniformType.ToString())
                        };
#else
                        return Descriptor.Count switch
                        {
                            1 => (DataChunk Data) => GL.Uniform1(ID, Descriptor.Count, Data.AsInt()),
                            2 => (DataChunk Data) => GL.Uniform2(ID, Descriptor.Count, Data.AsInt()),
                            3 => (DataChunk Data) => GL.Uniform3(ID, Descriptor.Count, Data.AsInt()),
                            4 => (DataChunk Data) => GL.Uniform4(ID, Descriptor.Count, Data.AsInt()),
                            _ => throw new OpenGLUnsupportedException(UniformType.ToString())
                        };
#endif
                    }
                case EValueType.UINT:
                    {
#if USE_SPANS
                        return Descriptor.Count switch
                        {
                            1 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { uint* ptr = (uint*)sptr; GL.Uniform1(ID, Descriptor.Count, ptr); } },
                            2 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { uint* ptr = (uint*)sptr; GL.Uniform2(ID, Descriptor.Count, ptr); } },
                            3 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { uint* ptr = (uint*)sptr; GL.Uniform3(ID, Descriptor.Count, ptr); } },
                            4 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { uint* ptr = (uint*)sptr; GL.Uniform4(ID, Descriptor.Count, ptr); } },
                            _ => throw new OpenGLUnsupportedException(UniformType.ToString())
                        };
#else
                        return Descriptor.Count switch
                        {
                            1 => (DataChunk Data) => GL.Uniform1(ID, Descriptor.Count, Data.AsUInt()),
                            2 => (DataChunk Data) => GL.Uniform2(ID, Descriptor.Count, Data.AsUInt()),
                            3 => (DataChunk Data) => GL.Uniform3(ID, Descriptor.Count, Data.AsUInt()),
                            4 => (DataChunk Data) => GL.Uniform4(ID, Descriptor.Count, Data.AsUInt()),
                            _ => throw new OpenGLUnsupportedException(UniformType.ToString())
                        };
#endif
                    }
                case EValueType.FLOAT:
                    {
#if USE_SPANS
                        return UniformType switch
                        {
                            EVariableType.FLOAT => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.Uniform1(ID, Descriptor.Count, ptr); } },
                            EVariableType.FLOAT_VEC2 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.Uniform2(ID, Descriptor.Count, ptr); } },
                            EVariableType.FLOAT_VEC3 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.Uniform3(ID, Descriptor.Count, ptr); } },
                            EVariableType.FLOAT_VEC4 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.Uniform4(ID, Descriptor.Count, ptr); } },

                            EVariableType.FLOAT_MATRIX_2_2 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix2(ID, Descriptor.Count, Transpose, ptr); } },
                            EVariableType.FLOAT_MATRIX_2_3 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix2x3(ID, Descriptor.Count, Transpose, ptr); } },
                            EVariableType.FLOAT_MATRIX_2_4 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix2x4(ID, Descriptor.Count, Transpose, ptr); } },

                            EVariableType.FLOAT_MATRIX_3_2 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix3x2(ID, Descriptor.Count, Transpose, ptr); } },
                            EVariableType.FLOAT_MATRIX_3_3 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix3(ID, Descriptor.Count, Transpose, ptr); } },
                            EVariableType.FLOAT_MATRIX_3_4 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix3x4(ID, Descriptor.Count, Transpose, ptr); } },

                            EVariableType.FLOAT_MATRIX_4_2 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix4x2(ID, Descriptor.Count, Transpose, ptr); } },
                            EVariableType.FLOAT_MATRIX_4_3 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix4x3(ID, Descriptor.Count, Transpose, ptr); } },
                            EVariableType.FLOAT_MATRIX_4_4 => (DataChunk Data) => { fixed (byte* sptr = Data.AsSpan()) { float* ptr = (float*)sptr; GL.UniformMatrix4(ID, Descriptor.Count, Transpose, ptr); } },
                            _ => throw new OpenGLUnsupportedException(UniformType.ToString())
                        };
#else
                        return UniformType switch
                        {
                            EVariableType.FLOAT => (DataChunk Data) => GL.Uniform1(ID, Descriptor.Count, Data.AsFloat()),
                            EVariableType.FLOAT_VEC2 => (DataChunk Data) => GL.Uniform2(ID, Descriptor.Count, Data.AsFloat()),
                            EVariableType.FLOAT_VEC3 => (DataChunk Data) => GL.Uniform3(ID, Descriptor.Count, Data.AsFloat()),
                            EVariableType.FLOAT_VEC4 => (DataChunk Data) => GL.Uniform4(ID, Descriptor.Count, Data.AsFloat()),

                            EVariableType.FLOAT_MATRIX_2_2 => (DataChunk Data) => GL.UniformMatrix2(ID, Descriptor.Count, Transpose, Data.AsFloat()),
                            EVariableType.FLOAT_MATRIX_2_3 => (DataChunk Data) => GL.UniformMatrix2x3(ID, Descriptor.Count, Transpose, Data.AsFloat()),
                            EVariableType.FLOAT_MATRIX_2_4 => (DataChunk Data) => GL.UniformMatrix2x4(ID, Descriptor.Count, Transpose, Data.AsFloat()),

                            EVariableType.FLOAT_MATRIX_3_2 => (DataChunk Data) => GL.UniformMatrix3x2(ID, Descriptor.Count, Transpose, Data.AsFloat()),
                            EVariableType.FLOAT_MATRIX_3_3 => (DataChunk Data) => GL.UniformMatrix3(ID, Descriptor.Count, Transpose, Data.AsFloat()),
                            EVariableType.FLOAT_MATRIX_3_4 => (DataChunk Data) => GL.UniformMatrix3x4(ID, Descriptor.Count, Transpose, Data.AsFloat()),

                            EVariableType.FLOAT_MATRIX_4_2 => (DataChunk Data) => GL.UniformMatrix4x2(ID, Descriptor.Count, Transpose, Data.AsFloat()),
                            EVariableType.FLOAT_MATRIX_4_3 => (DataChunk Data) => GL.UniformMatrix4x3(ID, Descriptor.Count, Transpose, Data.AsFloat()),
                            EVariableType.FLOAT_MATRIX_4_4 => (DataChunk Data) => GL.UniformMatrix4(ID, Descriptor.Count, Transpose, Data.AsFloat()),
                            _ => throw new OpenGLUnsupportedException(UniformType.ToString())
                        };
#endif
                    }
                default:
                    throw new OpenGLUnsupportedException(UniformType.ToString());
            }
        }
    }
}
