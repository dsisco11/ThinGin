namespace ThinGin.OpenGL.Common.Framebuffers
{
    public struct FrameBufferOptions
    {
        #region Static
        public static FrameBufferOptions Default = new FrameBufferOptions(EnableWrite: true);
        public static FrameBufferOptions ReadOnly = new FrameBufferOptions(EnableRead: true, EnableWrite: false);
        public static FrameBufferOptions ReadWrite = new FrameBufferOptions(EnableRead: true, EnableWrite: true);
        #endregion


        #region Values

        /// <summary> If <see langword="true"/> then the framebuffer will be able to be read from </summary>
        public bool EnableRead;
        /// <summary> If <see langword="true"/> then the framebuffer will be able to be written to </summary>
        public bool EnableWrite;

        /// <summary> If <see langword="true"/> then creates a color-buffer</summary>
        public bool ColorBuffer;
        /// <summary> If <see langword="true"/> then creates a depth-buffer</summary>
        public bool DepthBuffer;
        /// <summary> If <see langword="true"/> then creates a stencil-buffer</summary>
        public bool StencilBuffer;

        /// <summary> If <see langword="true"/> then the stencil and depth buffers will be made seperate attachments</summary>
        public bool SeperateDepthStencil;

        /// <summary> If <see langword="true"/> then color buffer will be created as a renderbuffer object with Multi-Sample Anti-Aliasing enabled</summary>
        public bool EnableMSAA;

        /// <summary> Number of samples to use when MSAA is enabled, defaults to 4 </summary>
        public int MSAA_Samples;
        #endregion

        public FrameBufferOptions(bool EnableRead = false, bool EnableWrite = true, bool ColorBuffer = true, bool DepthBuffer = false, bool StencilBuffer = false, bool SeperateDepthStencil = false, bool EnableMSAA = false, int Samples = 4)
        {
            this.EnableRead = EnableRead;
            this.EnableWrite = EnableWrite;

            this.ColorBuffer = ColorBuffer;
            this.DepthBuffer = DepthBuffer;
            this.StencilBuffer = StencilBuffer;
            this.SeperateDepthStencil = SeperateDepthStencil;

            this.EnableMSAA = EnableMSAA;
            MSAA_Samples = Samples;
        }
    }
}
