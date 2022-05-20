namespace ThinGin.Core.RenderHardware.Commands
{
    public abstract class RHICommandListBase
    {
        #region Values
        private readonly IRHIDriver driver;
        #endregion

        #region Constructors
        public RHICommandListBase(IRHIDriver driver)
        {
            this.driver = driver;
        }
        #endregion

        public abstract void Push(RHICommand cmd);


    }
}