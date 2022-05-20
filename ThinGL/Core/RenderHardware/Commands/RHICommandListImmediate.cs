using System.Collections.Concurrent;

namespace ThinGin.Core.RenderHardware.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class RHICommandListImmediate : RHICommandListBase
    {
        #region Values
        private ConcurrentQueue<RHICommand> items = null;
        #endregion

        #region Constructors
        public RHICommandListImmediate(IRHIDriver driver) : base(driver)
        {
            items = new ConcurrentQueue<RHICommand>();
        }
        #endregion
        public override void Push(RHICommand cmd) => items.Enqueue(cmd);

        public void Flush()
        {
            while (items.TryDequeue(out RHICommand cmd))
            {
                cmd.Execute();
            }
        }

    }
}
