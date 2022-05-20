namespace ThinGin.Core.Console
{
    [System.Flags]
    public enum EVArgType : int
    {
        /// <summary> A common string </summary>
        String = 0x1,
        /// <summary> A well known string identifier, a constant from a predetermined set </summary>
        Ident = 0x2,

        Numeric = (Unsigned | Integer | Decimal),
        /// <summary> Unsigned 64-bit integer </summary>
        Unsigned = 0x002,
        /// <summary> Signed 64-bit integer </summary>
        Integer = 0x004,
        /// <summary> Signed 64-bit decimal </summary>
        Decimal = 0x008,

        /// <summary> The individual argument is composed of multiple sub values of possibly different types. </summary>
        Complex = 0x0002
    }
}
