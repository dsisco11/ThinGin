namespace ThinGin.Core.Console
{
    [System.Flags]
    public enum ECArgType : int
    {
        NULL = 0x0,
        AUTO = 0x1,

        KEYWORD = 0x2,
        STRING = 0x4,

        /// <summary>
        /// An integer is one or more decimal digits ‘0’ through ‘9’ and corresponds to a subset of the NUMBER token in the grammar. Integers may be immediately preceded by ‘-’ or ‘+’ to indicate the sign.
        /// </summary>
        INTEGER = 0x200,
        /// <summary>
        /// An unsigned integer
        /// </summary>
        UNSIGNED = 0x400,
        /// <summary>
        /// A number is either an integer, or zero or more decimal digits followed by a dot (.) followed by one or more decimal digits.
        /// </summary>
        NUMBER = 0x800,
        /// <summary>
        /// A percentage value is denoted by <percentage>, consists of a <number> immediately followed by a percent sign ‘%’.
        /// Percentage values are always relative to another value, for example a length.
        /// </summary>
        PERCENT = 0x2000,

        COLOR = 0x4000,
        POSITION = 0x8000,
        OBJECT_ID = 0x20000,

        COLLECTION = 0x40000,

        /// <summary> An execution of logic, a dynamic function call of some sort. </summary>
        SELECTOR = 0x80000,
    }
}
