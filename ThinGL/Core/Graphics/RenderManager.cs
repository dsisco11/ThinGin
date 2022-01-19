
using ThinGin.Core.Graphics;

namespace ThinGin.Core.Engine
{
    public class RenderManager
    {
        #region Values
        public readonly GraphicsObjectManager Objects = new GraphicsObjectManager();
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RenderManager()
        {
        }
        #endregion

        #region Processing
        public void Process()
        {
            Objects.Process();
        }

        /// <summary>
        /// Fired prior to frame processing
        /// </summary>
        protected void PreFrame()
        {
        }

        /// <summary>
        /// Fired after frame processing
        /// </summary>
        protected void PostFrame()
        {
        }
        #endregion
    }
}
