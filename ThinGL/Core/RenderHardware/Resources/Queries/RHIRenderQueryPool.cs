using System.Collections.Generic;

namespace ThinGin.Core.RenderHardware.Resources.Queries
{
    public abstract class RHIRenderQueryPool : RHIUntrackedResource
    {
        #region Values
        protected readonly int TargetPoolSize;
        protected readonly ERenderQueryType QueryType;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIRenderQueryPool(in IRHI rhi, in ERenderQueryType QueryType, in int TargetPoolSize) : base(rhi)
        {
            this.QueryType = QueryType;
            this.TargetPoolSize = TargetPoolSize;
        }
        #endregion

        protected abstract RHIPooledRenderQuery InstantiateQuery(in ERenderQueryType QueryType);

        public abstract RHIPooledRenderQuery AllocateQuery();

    }
}
