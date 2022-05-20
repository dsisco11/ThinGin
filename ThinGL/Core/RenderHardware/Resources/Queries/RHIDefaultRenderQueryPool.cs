using System.Collections.Generic;

namespace ThinGin.Core.RenderHardware.Resources.Queries
{
    public class RHIDefaultRenderQueryPool : RHIRenderQueryPool
    {
        #region Values
        protected readonly Queue<RHIPooledRenderQuery> free = null;
        protected readonly Stack<RHIPooledRenderQuery> active = null;
        #endregion

        #region Constructors
        public RHIDefaultRenderQueryPool(in IRHI rhi, in ERenderQueryType QueryType, in int TargetPoolSize) : base(rhi, QueryType, TargetPoolSize)
        {
            free = new Queue<RHIPooledRenderQuery>(TargetPoolSize);
            active = new Stack<RHIPooledRenderQuery>(TargetPoolSize);
        }
        #endregion

        protected override RHIPooledRenderQuery InstantiateQuery(in ERenderQueryType QueryType)
        {// XXX: TODO
        }

        public override RHIPooledRenderQuery AllocateQuery()
        {
            if (free.Count > 0)
            {
                var query = free.Dequeue();
                active.Push(query);
                return query;
            }
            else
            {
                var query = InstantiateQuery(QueryType);
                active.Push(query);
                return query;
            }
        }

    }
}
