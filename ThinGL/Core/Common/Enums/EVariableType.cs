namespace ThinGin.Core.Common.Enums
{
    public enum EVariableType : ulong
    {
        /// <summary> 32-bit integer </summary>
        INT,
        /// <summary> array of 32-bit integers </summary>
        INT_VEC2,
        INT_VEC3,
        INT_VEC4,

        /// <summary> 32-bit unsigned integer </summary>
        UINT,
        /// <summary> array of 32-bit unsigned integers </summary>
        UINT_VEC2,
        UINT_VEC3,
        UINT_VEC4,

        FLOAT,
        FLOAT_VEC2,
        FLOAT_VEC3,
        FLOAT_VEC4,

        FLOAT_MATRIX_2_2,
        FLOAT_MATRIX_2_3,
        FLOAT_MATRIX_2_4,

        FLOAT_MATRIX_3_2,
        FLOAT_MATRIX_3_3,
        FLOAT_MATRIX_3_4,

        FLOAT_MATRIX_4_2,
        FLOAT_MATRIX_4_3,
        FLOAT_MATRIX_4_4,

        DOUBLE,
        DOUBLE_VEC2,
        DOUBLE_VEC3,
        DOUBLE_VEC4,

        DOUBLE_MATRIX_2_2,
        DOUBLE_MATRIX_2_3,
        DOUBLE_MATRIX_2_4,

        DOUBLE_MATRIX_3_2,
        DOUBLE_MATRIX_3_3,
        DOUBLE_MATRIX_3_4,

        DOUBLE_MATRIX_4_2,
        DOUBLE_MATRIX_4_3,
        DOUBLE_MATRIX_4_4,


        BOOL,
        BOOL_VEC2,
        BOOL_VEC3,
        BOOL_VEC4,

        /// <summary> Can be any form of signed/unsigned integer </summary>
        HANDLE,
    }
}
