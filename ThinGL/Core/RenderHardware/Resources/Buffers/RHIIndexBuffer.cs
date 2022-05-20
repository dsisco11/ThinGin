﻿using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHIIndexBuffer : RHIRamResource
    {
        #region Values
        public readonly uint Size;
        public readonly uint Stride;
        public readonly EBufferUpdate Usage;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIIndexBuffer(in IRHI rhi, in uint Size, in uint Stride, in EBufferUpdate Usage) : base(rhi)
        {
            this.Size = Size;
            this.Stride = Stride;
            this.Usage = Usage;
        }
        #endregion


        public abstract void Swap(in RHIIndexBuffer other);
    }
}
