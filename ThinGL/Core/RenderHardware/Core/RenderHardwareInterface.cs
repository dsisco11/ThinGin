using System.Collections.Concurrent;
using System.Collections.Generic;

using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.Core.RenderHardware
{
    /// <summary>
    /// Abstracts the process of working with the RHI driver
    /// </summary>
    public class RenderHardwareInterface : IRHI
    {
        #region Values
        private readonly IRHIDriver driver = null;
        private readonly RHIResourceManager resources = null;
        private uint current_frame = 0;
        //private ConcurrentBag<RHIFenceObject>
        #endregion

        #region Properties
        public RHIResourceManager Resources => resources;
        //System.Threading.Channels.Channel
        public uint CurrentFrame => current_frame;
        #endregion

        #region Constructors
        public RenderHardwareInterface(IRHIDriver driver)
        {
            this.driver = driver;
            Resources = new RHIResourceManager();
        }
        #endregion

        #region Lifespan
        /// <summary>
        /// Shutdown the RHI; handle shutdown and resource destruction before the RHI's actual destructor is called (so that all resources of the RHI are still available for shutdown).
        /// </summary>
        public void Shutdown()
        {
        }
        #endregion

        #region Resource Instantiation
        // Textures
        // Shaders
        // Buffers
        // GBuffers
        // Fences
        // 
        #endregion
    }
}
