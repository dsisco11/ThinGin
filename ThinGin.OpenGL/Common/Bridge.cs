using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Enums;
using ThinGin.OpenGL.Common.Exceptions;

namespace ThinGin.OpenGL.Common
{
    internal static class Bridge
    {

        public static EnableCap Translate(EEngineCap Cap)
        {
            return Cap switch
            {
                EEngineCap.Blending => EnableCap.Blend,
                EEngineCap.AlphaTest => EnableCap.AlphaTest,
                EEngineCap.DepthTest => EnableCap.DepthTest,
                EEngineCap.StencilTest => EnableCap.StencilTest,
                _ => throw new NotImplementedException(),
            };
        }


        public static EVariableType Translate(ActiveAttribType Type)
        {
            return Type switch
            {
                ActiveAttribType.Int => EVariableType.INT,
                ActiveAttribType.IntVec2 => EVariableType.INT_VEC2,
                ActiveAttribType.IntVec3 => EVariableType.INT_VEC3,
                ActiveAttribType.IntVec4 => EVariableType.INT_VEC4,

                ActiveAttribType.UnsignedInt => EVariableType.UINT,
                ActiveAttribType.UnsignedIntVec2 => EVariableType.UINT_VEC2,
                ActiveAttribType.UnsignedIntVec3 => EVariableType.UINT_VEC3,
                ActiveAttribType.UnsignedIntVec4 => EVariableType.UINT_VEC4,

                ActiveAttribType.Float => EVariableType.FLOAT,
                ActiveAttribType.FloatVec2 => EVariableType.FLOAT_VEC2,
                ActiveAttribType.FloatVec3 => EVariableType.FLOAT_VEC3,
                ActiveAttribType.FloatVec4 => EVariableType.FLOAT_VEC4,
                ActiveAttribType.FloatMat2 => EVariableType.FLOAT_MATRIX_2_2,
                ActiveAttribType.FloatMat3 => EVariableType.FLOAT_MATRIX_3_3,
                ActiveAttribType.FloatMat4 => EVariableType.FLOAT_MATRIX_4_4,
                ActiveAttribType.FloatMat2x3 => EVariableType.FLOAT_MATRIX_2_3,
                ActiveAttribType.FloatMat2x4 => EVariableType.FLOAT_MATRIX_2_4,
                ActiveAttribType.FloatMat3x2 => EVariableType.FLOAT_MATRIX_3_2,
                ActiveAttribType.FloatMat3x4 => EVariableType.FLOAT_MATRIX_3_4,
                ActiveAttribType.FloatMat4x2 => EVariableType.FLOAT_MATRIX_4_2,
                ActiveAttribType.FloatMat4x3 => EVariableType.FLOAT_MATRIX_4_3,

                ActiveAttribType.Double => EVariableType.DOUBLE,
                ActiveAttribType.DoubleVec2 => EVariableType.DOUBLE_VEC2,
                ActiveAttribType.DoubleVec3 => EVariableType.DOUBLE_VEC3,
                ActiveAttribType.DoubleVec4 => EVariableType.DOUBLE_VEC4,
                ActiveAttribType.DoubleMat2 => EVariableType.DOUBLE_MATRIX_2_2,
                ActiveAttribType.DoubleMat3 => EVariableType.DOUBLE_MATRIX_3_3,
                ActiveAttribType.DoubleMat4 => EVariableType.DOUBLE_MATRIX_4_4,
                ActiveAttribType.DoubleMat2x3 => EVariableType.DOUBLE_MATRIX_2_3,
                ActiveAttribType.DoubleMat2x4 => EVariableType.DOUBLE_MATRIX_2_4,
                ActiveAttribType.DoubleMat3x2 => EVariableType.DOUBLE_MATRIX_3_2,
                ActiveAttribType.DoubleMat3x4 => EVariableType.DOUBLE_MATRIX_3_4,
                ActiveAttribType.DoubleMat4x2 => EVariableType.DOUBLE_MATRIX_4_2,
                ActiveAttribType.DoubleMat4x3 => EVariableType.DOUBLE_MATRIX_4_3,

                _ => throw new NotImplementedException(),
            };
        }

        public static EVariableType Translate(ActiveUniformType uType)
        {
            switch (uType)
            {
                case ActiveUniformType.Int: return EVariableType.INT;
                case ActiveUniformType.IntVec2: return EVariableType.INT_VEC2;
                case ActiveUniformType.IntVec3: return EVariableType.INT_VEC3;
                case ActiveUniformType.IntVec4: return EVariableType.INT_VEC4;

                case ActiveUniformType.UnsignedInt: return EVariableType.UINT;
                case ActiveUniformType.UnsignedIntVec2: return EVariableType.UINT_VEC2;
                case ActiveUniformType.UnsignedIntVec3: return EVariableType.UINT_VEC3;
                case ActiveUniformType.UnsignedIntVec4: return EVariableType.UINT_VEC4;

                case ActiveUniformType.Float: return EVariableType.FLOAT;
                case ActiveUniformType.FloatVec2: return EVariableType.FLOAT_VEC2;
                case ActiveUniformType.FloatVec3: return EVariableType.FLOAT_VEC3;
                case ActiveUniformType.FloatVec4: return EVariableType.FLOAT_VEC4;

                case ActiveUniformType.FloatMat2: return EVariableType.FLOAT_MATRIX_2_2;
                case ActiveUniformType.FloatMat2x3: return EVariableType.FLOAT_MATRIX_2_3;
                case ActiveUniformType.FloatMat2x4: return EVariableType.FLOAT_MATRIX_2_4;

                case ActiveUniformType.FloatMat3x2: return EVariableType.FLOAT_MATRIX_3_2;
                case ActiveUniformType.FloatMat3: return EVariableType.FLOAT_MATRIX_3_3;
                case ActiveUniformType.FloatMat3x4: return EVariableType.FLOAT_MATRIX_3_4;

                case ActiveUniformType.FloatMat4x2: return EVariableType.FLOAT_MATRIX_4_2;
                case ActiveUniformType.FloatMat4x3: return EVariableType.FLOAT_MATRIX_4_3;
                case ActiveUniformType.FloatMat4: return EVariableType.FLOAT_MATRIX_4_4;

                case ActiveUniformType.Double: return EVariableType.DOUBLE;
                case ActiveUniformType.DoubleVec2: return EVariableType.DOUBLE_VEC2;
                case ActiveUniformType.DoubleVec3: return EVariableType.DOUBLE_VEC3;
                case ActiveUniformType.DoubleVec4: return EVariableType.DOUBLE_VEC4;


                //case ActiveUniformType.DoubleMat2: return return EUniformType.DOUBLE_MATRIX_2_2;
                //case ActiveUniformType.DoubleMat2x3: return return EUniformType.DOUBLE_MATRIX_2_3;
                //case ActiveUniformType.DoubleMat2x4: return return EUniformType.DOUBLE_MATRIX_2_4;


                //case ActiveUniformType.DoubleMat3x2: return return EUniformType.DOUBLE_MATRIX_3_2;
                //case ActiveUniformType.DoubleMat3: return return EUniformType.DOUBLE_MATRIX_3_3;
                //case ActiveUniformType.DoubleMat3x4: return return EUniformType.DOUBLE_MATRIX_3_4;

                //case ActiveUniformType.DoubleMat4x2: return return EUniformType.DOUBLE_MATRIX_4_2;
                //case ActiveUniformType.DoubleMat4x3: return return EUniformType.DOUBLE_MATRIX_4_3;
                //case ActiveUniformType.DoubleMat4: return return EUniformType.DOUBLE_MATRIX_4_4;


                case ActiveUniformType.Bool: return EVariableType.BOOL;
                case ActiveUniformType.BoolVec2: return EVariableType.BOOL_VEC2;
                case ActiveUniformType.BoolVec3: return EVariableType.BOOL_VEC3;
                case ActiveUniformType.BoolVec4: return EVariableType.BOOL_VEC4;

                case ActiveUniformType.UnsignedIntAtomicCounter: return EVariableType.UINT;

                case ActiveUniformType.Image1D:
                case ActiveUniformType.Image2D:
                case ActiveUniformType.Image3D:
                case ActiveUniformType.Image2DRect:
                case ActiveUniformType.ImageCube:
                case ActiveUniformType.Sampler1D:
                case ActiveUniformType.Sampler2D:
                case ActiveUniformType.Sampler3D:
                case ActiveUniformType.SamplerCube:
                case ActiveUniformType.Sampler1DShadow:
                case ActiveUniformType.Sampler2DShadow:
                case ActiveUniformType.Sampler2DRect:
                case ActiveUniformType.Sampler2DRectShadow:
                case ActiveUniformType.SamplerCubeShadow:
                case ActiveUniformType.Sampler1DArray:
                case ActiveUniformType.Sampler2DArray:
                case ActiveUniformType.SamplerBuffer:
                case ActiveUniformType.Sampler1DArrayShadow:
                case ActiveUniformType.Sampler2DArrayShadow:
                case ActiveUniformType.IntSampler1D:
                case ActiveUniformType.IntSampler2D:
                case ActiveUniformType.IntSampler3D:
                case ActiveUniformType.IntSamplerCube:
                case ActiveUniformType.IntSampler2DRect:
                case ActiveUniformType.IntSamplerBuffer:
                case ActiveUniformType.IntSampler1DArray:
                case ActiveUniformType.IntSampler2DArray:
                case ActiveUniformType.UnsignedIntSampler1D:
                case ActiveUniformType.UnsignedIntSampler2D:
                case ActiveUniformType.UnsignedIntSampler3D:
                case ActiveUniformType.UnsignedIntSamplerCube:
                case ActiveUniformType.UnsignedIntSampler2DRect:
                case ActiveUniformType.UnsignedIntSamplerBuffer:
                case ActiveUniformType.UnsignedIntSampler1DArray:
                case ActiveUniformType.UnsignedIntSampler2DArray:
                case ActiveUniformType.SamplerCubeMapArray:
                case ActiveUniformType.SamplerCubeMapArrayShadow:
                case ActiveUniformType.IntSamplerCubeMapArray:
                case ActiveUniformType.UnsignedIntSamplerCubeMapArray:
                case ActiveUniformType.ImageBuffer:
                case ActiveUniformType.Image2DMultisample:
                case ActiveUniformType.Image1DArray:
                case ActiveUniformType.Image2DArray:
                case ActiveUniformType.ImageCubeMapArray:
                case ActiveUniformType.Image2DMultisampleArray:
                case ActiveUniformType.IntImage1D:
                case ActiveUniformType.IntImage2D:
                case ActiveUniformType.IntImage3D:
                case ActiveUniformType.IntImage2DRect:
                case ActiveUniformType.IntImageCube:
                case ActiveUniformType.IntImageBuffer:
                case ActiveUniformType.IntImage2DMultisample:
                case ActiveUniformType.IntImage1DArray:
                case ActiveUniformType.IntImage2DArray:
                case ActiveUniformType.IntImageCubeMapArray:
                case ActiveUniformType.IntImage2DMultisampleArray:
                case ActiveUniformType.UnsignedIntImage1D:
                case ActiveUniformType.UnsignedIntImage2D:
                case ActiveUniformType.UnsignedIntImage3D:
                case ActiveUniformType.UnsignedIntImage2DRect:
                case ActiveUniformType.UnsignedIntImageCube:
                case ActiveUniformType.UnsignedIntImageBuffer:
                case ActiveUniformType.UnsignedIntImage1DArray:
                case ActiveUniformType.UnsignedIntImage2DArray:
                case ActiveUniformType.UnsignedIntImageCubeMapArray:
                case ActiveUniformType.UnsignedIntImage2DMultisample:
                case ActiveUniformType.UnsignedIntImage2DMultisampleArray:
                case ActiveUniformType.Sampler2DMultisample:
                case ActiveUniformType.IntSampler2DMultisample:
                case ActiveUniformType.UnsignedIntSampler2DMultisample:
                case ActiveUniformType.Sampler2DMultisampleArray:
                case ActiveUniformType.IntSampler2DMultisampleArray:
                case ActiveUniformType.UnsignedIntSampler2DMultisampleArray:
                    return EVariableType.INT;

                default:
                    throw new OpenGLUnsupportedException(uType.ToString());
            }
        }

        public static EValueType Translate(EVariableType uType)
        {
            switch (uType)
            {
                case EVariableType.INT:
                case EVariableType.INT_VEC2:
                case EVariableType.INT_VEC3:
                case EVariableType.INT_VEC4:
                    return EValueType.Int32;

                case EVariableType.UINT:
                case EVariableType.UINT_VEC2:
                case EVariableType.UINT_VEC3:
                case EVariableType.UINT_VEC4:
                    return EValueType.UInt32;

                case EVariableType.FLOAT:
                case EVariableType.FLOAT_VEC2:
                case EVariableType.FLOAT_VEC3:
                case EVariableType.FLOAT_VEC4:
                    return EValueType.Float32;

                case EVariableType.FLOAT_MATRIX_2_2:
                case EVariableType.FLOAT_MATRIX_2_3:
                case EVariableType.FLOAT_MATRIX_2_4:
                case EVariableType.FLOAT_MATRIX_3_2:
                case EVariableType.FLOAT_MATRIX_3_3:
                case EVariableType.FLOAT_MATRIX_3_4:
                case EVariableType.FLOAT_MATRIX_4_2:
                case EVariableType.FLOAT_MATRIX_4_3:
                case EVariableType.FLOAT_MATRIX_4_4:
                    return EValueType.Float32;

                case EVariableType.DOUBLE:
                case EVariableType.DOUBLE_VEC2:
                case EVariableType.DOUBLE_VEC3:
                case EVariableType.DOUBLE_VEC4:
                    return EValueType.DOUBLE;

                case EVariableType.DOUBLE_MATRIX_2_2:
                case EVariableType.DOUBLE_MATRIX_2_3:
                case EVariableType.DOUBLE_MATRIX_2_4:
                case EVariableType.DOUBLE_MATRIX_3_2:
                case EVariableType.DOUBLE_MATRIX_3_3:
                case EVariableType.DOUBLE_MATRIX_3_4:
                case EVariableType.DOUBLE_MATRIX_4_2:
                case EVariableType.DOUBLE_MATRIX_4_3:
                case EVariableType.DOUBLE_MATRIX_4_4:
                    return EValueType.DOUBLE;

                // OpenGL wants to receive boolean values as whole integers anyways so lets treat them as ints from the start
                case EVariableType.BOOL:
                case EVariableType.BOOL_VEC2:
                case EVariableType.BOOL_VEC3:
                case EVariableType.BOOL_VEC4:
                    return EValueType.Int32;

                case EVariableType.HANDLE:
                    return EValueType.Int32;

                default:
                    throw new NotImplementedException(uType.ToString());
            };
        }
        public static ShaderType Translate(EShaderType Type)
        {
            return Type switch
            {
                EShaderType.Vertex => ShaderType.VertexShader,
                EShaderType.Fragment => ShaderType.FragmentShader,
                EShaderType.Geometry => ShaderType.GeometryShader,
                EShaderType.Tesselation => ShaderType.TessControlShader,
                EShaderType.Compute => ShaderType.ComputeShader,

                EShaderType.None => throw new System.NotImplementedException(),
                _ => throw new System.NotImplementedException(),
            };
        }

        /// <summary>
        /// Translates an engine primitive type constant to the equivalent OpenGL one
        /// </summary>
        public static PrimitiveType Translate(ETopology Type)
        {
            switch (Type)
            {
                case ETopology.PointList:
                    return PrimitiveType.Points;
                case ETopology.LineList:
                    return PrimitiveType.Lines;
                case ETopology.LineLoop:
                    return PrimitiveType.LineLoop;
                case ETopology.LineStrip:
                    return PrimitiveType.LineStrip;
                case ETopology.Triangles:
                    return PrimitiveType.Triangles;
                case ETopology.TriangleStrip:
                    return PrimitiveType.TriangleStrip;
                case ETopology.TriangleFan:
                    return PrimitiveType.TriangleFan;
                case ETopology.Quads:
                    return PrimitiveType.Quads;
                case ETopology.QuadStrip:
                    return PrimitiveType.QuadStrip;
                case ETopology.Polygon:
                    return PrimitiveType.Polygon;
                default:
                    throw new System.NotImplementedException($"Translation value unknown for {nameof(ETopology)}.{Type}");
            }
        }

        /// <summary>
        /// Translates an engine value type to its equivalent OpenGL vertex attribute type
        /// </summary>
        public static VertexAttribPointerType Translate(EValueType DataType)
        {
            switch (DataType)
            {
                case EValueType.UInt8: return VertexAttribPointerType.UnsignedByte;
                case EValueType.Int8: return VertexAttribPointerType.Byte;
                case EValueType.Int16: return VertexAttribPointerType.Short;
                case EValueType.UInt16: return VertexAttribPointerType.UnsignedShort;
                case EValueType.Float32: return VertexAttribPointerType.Float;
                case EValueType.Float16: return VertexAttribPointerType.HalfFloat;
                case EValueType.DOUBLE: return VertexAttribPointerType.Double;
                case EValueType.Int32: return VertexAttribPointerType.Int;
                case EValueType.UInt32: return VertexAttribPointerType.UnsignedInt;
                default:
                    throw new System.NotImplementedException($"Translation value unknown for {nameof(EValueType)}.{DataType}");
            }
        }
    }
}
