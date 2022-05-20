using System;
using System.Text;

using ThinGin.Core.Common;

namespace ThinGin.Core.RenderHardware.Resources.Shaders
{
    public class RHIShaderLibrary : RHIResource
    {
        #region Values
        public readonly uint LibraryID;
        public readonly string LibraryName;
        public readonly EShaderPlatform Platform;
        public readonly ShaHash Hash;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public RHIShaderLibrary(IRHI rhi, EShaderPlatform platform) : base(rhi)
        {
            Platform = platform;
        }
        #endregion

        #region Lifetime
        public override RHIDelegate Get_Initializer() => null;
        public override RHIDelegate Get_Releaser() => null;
        #endregion

        public RHIHandle CreateShader(in uint ShaderIndex)
        {
        }

        public uint FindShaderIndex(in ShaHash Hash)
        {
        }

        public uint FindShaderMapIndex(in ShaHash Hash)
        {
        }

        public bool PreloadShader(uint ShaderIndex, Action Callback)
        {
        }

        public bool PreloadShaderMap(uint ShaderMapIndex, Action Callback)
        {
        }

        public void ReleasePreloadedShader(uint ShaderIndex)
        {
        }

    }
}
