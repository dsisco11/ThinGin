namespace ThinGin.Core.Common.Enums
{
    [System.Flags]
    public enum EValueType : int
    {
        NULL = 0x0,

        Is8BitType = (BYTE | SBYTE),
        Is16BitType = (SHORT | USHORT),
        Is32BitType = (INT | UINT),

        IsUnsigned = (BYTE | USHORT | UINT),
        /// <summary> 8-bit unsigned integer </summary>
        BYTE = 0x1,
        /// <summary> 16-bit unsigned integer </summary>
        USHORT = 0x2,
        /// <summary> 32-bit unsigned integer </summary>
        UINT = 0x4,


        IsSigned = (SBYTE | SHORT | INT),
        /// <summary> 8-bit signed integer </summary>
        SBYTE = 0x10,
        /// <summary> 16-bit signed integer </summary>
        SHORT = 0x20,
        /// <summary> 32-bit signed integer </summary>
        INT = 0x40,


        IsFloatingPoint = (FLOAT_HALF | FLOAT | DOUBLE),
        /// <summary> 16-bit signed floating point number </summary>
        FLOAT_HALF = 0x100,
        /// <summary> 32-bit signed floating point number </summary>
        FLOAT = 0x200,
        /// <summary> 64-bit signed floating point number </summary>
        DOUBLE = 0x400,
    }
}
