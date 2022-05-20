namespace ThinGin.Core.Common.Enums
{
    public enum EColorWriteMask : byte
    {
        RED = 0x01,
        GREEN = 0x02,
        BLUE = 0x04,
        ALPHA = 0x08,

        RG = RED | GREEN,
        BA = BLUE | ALPHA,
        RGB = RED | GREEN | BLUE,
        RGBA = RED | GREEN | BLUE | ALPHA,
    }
}
