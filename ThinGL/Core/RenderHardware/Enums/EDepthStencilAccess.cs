using System;

namespace ThinGin.Core.RenderHardware.Enums
{
    [Flags]
    public enum EDepthStencilAccess : byte
    {
        DepthNop                 = 0x00,
        DepthWrite               = 0x01,
        DepthRead                = 0x02,
        DepthMask                = 0x0F,

        StencilNop               = 0x00,
        StencilWrite             = 0x10,
        StencilRead              = 0x20,
        StencilMask              = 0xF0,

        DepthNop_StencilNop      = DepthNop | StencilNop,
        DepthRead_StencilNop     = DepthRead | StencilNop,
        DepthWrite_StencilNop    = DepthWrite | StencilNop,

        DepthNop_StencilRead     = DepthNop | StencilRead,
        DepthRead_StencilRead    = DepthRead | StencilRead,
        DepthWrite_StencilRead   = DepthWrite | StencilRead,

        DepthNop_StencilWrite    = DepthNop | StencilWrite,
        DepthRead_StencilWrite   = DepthRead | StencilWrite,
        DepthWrite_StencilWrite  = DepthWrite | StencilWrite,
    }
}
