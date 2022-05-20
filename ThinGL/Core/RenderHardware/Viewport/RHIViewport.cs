using System;
using System.Drawing;

using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.RenderHardware.Viewport
{
    public abstract class RHIViewport
    {
        #region Values
        private Size size = Size.Empty;
        private bool fullscreen = false;
        private readonly IRHIDriver driver = null;
        private readonly EPixelFormat format = EPixelFormat.PF_Unknown;
        #endregion

        #region Accessors
        public IRHIDriver Driver => driver;
        public virtual Size Size => size;
        public virtual EPixelFormat Format => format;
        public virtual bool IsFullscreen => fullscreen;
        public virtual RHIViewportPresenter CustomPresenter { get; set; }
        #endregion

        #region Constructors
        protected RHIViewport(IRHIDriver driver, Size size, bool fullscreen, EPixelFormat format)
        {
            this.driver = driver;
            this.size = size;
            this.fullscreen = fullscreen;
            this.format = format;
        }
        #endregion

        public abstract void Resize(Size size, bool FullscreenMode);

        #region Timing
        public abstract void Tick(double DeltaTime);
        public abstract void IssueFrameEvent();
        public abstract void WaitForFrameEventCompletion();
        #endregion

        #region Native Accessors
        public abstract object GetNativeBackBufferRT();
        public abstract object GetNativeBackBufferTexture();
        public abstract object GetNativeSwapChain();
        public abstract object GetNativeWindow();
        #endregion
    }
}
