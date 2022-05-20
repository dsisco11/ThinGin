using ThinGin.Core.Common.Enums;
using ThinGin.Core.RenderHardware.Enums;

namespace ThinGin.Core.RenderHardware
{
    /// <summary>
    /// Represents the access state of the depth and stencil buffers. This state is exclusive and may be either write-only or read-only
    /// </summary>
    public class ExclusiveDepthStencilAccess
    {
        #region Values
        private EDepthStencilAccess access;
        #endregion

        #region Accessors
        public EDepthStencilAccess Access => access;
        #endregion

        #region Constructors
        public ExclusiveDepthStencilAccess(EDepthStencilAccess Access)
        {
            access = Access;
        }
        #endregion

        public void SetDepthReadable() => access = ((access & EDepthStencilAccess.StencilMask) | EDepthStencilAccess.DepthRead);
        public void SetDepthWriteable() => access = ((access & EDepthStencilAccess.StencilMask) | EDepthStencilAccess.DepthWrite);

        public void SetStencilReadable() => access = ((access & EDepthStencilAccess.DepthMask) | EDepthStencilAccess.StencilRead);
        public void SetStencilWriteable() => access = ((access & EDepthStencilAccess.DepthMask) | EDepthStencilAccess.StencilWrite);

        public ERHIAccess GetDepth()
        {
            if (access.HasFlag(EDepthStencilAccess.DepthNop))
                return ERHIAccess.None;
            else if (access.HasFlag(EDepthStencilAccess.DepthRead))
                return ERHIAccess.ReadOnlyExclusiveMask;
            else
                return ERHIAccess.WriteOnlyExclusiveMask;
        }

        public ERHIAccess GetStencil()
        {
            if (access.HasFlag(EDepthStencilAccess.StencilNop))
                return ERHIAccess.None;
            else if (access.HasFlag(EDepthStencilAccess.StencilRead))
                return ERHIAccess.ReadOnlyExclusiveMask;
            else
                return ERHIAccess.WriteOnlyExclusiveMask;
        }


        public bool IsAnyWriteable() => access.HasFlag(EDepthStencilAccess.DepthWrite) | access.HasFlag(EDepthStencilAccess.StencilWrite);

        /// <summary>
        /// Returns whether the depth buffer is flagged as readable or writeable
        /// </summary>
        public bool IsUsingDepth() => 0 != (access | EDepthStencilAccess.DepthMask);
        /// <summary>
        /// Returns whether the stencil buffer is flagged as readable or writeable
        /// </summary>
        public bool IsUsingStencil() => 0 != (access | EDepthStencilAccess.StencilMask);
        /// <summary>
        /// Returns whether the depth and stencil buffers are flagged as readable or writeable
        /// </summary>
        public bool IsUsingDepthStencil() => 0 != (access | EDepthStencilAccess.DepthMask | EDepthStencilAccess.StencilMask);


        public bool IsDepthReadable() => access.HasFlag(EDepthStencilAccess.DepthRead);
        public bool IsDepthWriteable() => access.HasFlag(EDepthStencilAccess.DepthWrite);

        public bool IsStencilReadable() => access.HasFlag(EDepthStencilAccess.StencilRead);
        public bool IsStencilWriteable() => access.HasFlag(EDepthStencilAccess.StencilWrite);
    }
}
