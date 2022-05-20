using System.Collections.Generic;

namespace ThinGin.Core.RenderHardware.Commands
{
    /// <summary>
    /// Holds a list of RHI commands to execute together.
    /// </summary>
    public class RHICommandList : RHICommandListBase
    {
        #region Values
        private List<RHICommand> items = null;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public RHICommandList(IRHIDriver driver) : base(driver)
        {
            items = new List<RHICommand>(10);
        }
        #endregion

        public override void Push(RHICommand cmd) => items.Add(cmd);


    }
}
