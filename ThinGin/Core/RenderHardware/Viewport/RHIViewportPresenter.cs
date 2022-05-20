using System;

namespace ThinGin.Core.RenderHardware.Viewport
{
    public abstract class RHIViewportPresenter
    {
        #region Events
        /// <summary>
        /// Called when rendering thread is acquired
        /// </summary>
        public event EventHandler OnAquireThreadOwnership;

        /// <summary>
        /// Called when rendering thread is released
        /// </summary>
        public event EventHandler OnReleaseThreadOwnership;

        /// <summary>
        /// Called when the viewport is resized
        /// </summary>
        public event EventHandler<System.Drawing.Size> OnBackBufferResize;
        #endregion

        #region Constructors
        public RHIViewportPresenter()
        {
        }
        #endregion

        /// <summary>
        /// In cases where we want to use a custom presenter but still let the native environment handle advancement of the backbuffer.
        /// </summary>
        public abstract bool NeedsAdvanceBackBuffer();

        /// <summary>
        /// Called from render thread to check if a native present wil lbe requested for the current frame.
        /// </summary>
        public abstract bool NeedsNativePresent();

        /// <summary>
        /// Called from the RHI thread immediately before the engine begins drawing to the viewport
        /// </summary>
        public abstract void BeginDrawing();

        /// <summary>
        /// Called from RHI thread after native Present has been called
        /// </summary>
        public abstract void PostPresent();

        /// <summary>
        /// Called by RHI thread to trigger this presenter
        /// </summary>
        public abstract void Present(int SyncIntervalMS);
    }
}
