using ThinGin.Core.Common;

namespace ThinGin.Core.RenderHardware.Resources
{
    public abstract class RHIShader : RHIResource
    {
        #region Values
        /// <summary>
        /// This is for debugging only, format: "Material:ShaderFile.whlsl"
        /// </summary>
        public readonly string ShaderName;
        public readonly EShaderType ShaderType;
        private ShaHash hash;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        public ShaHash Hash => hash;
        #endregion

        #region Constructors
        public RHIShader(in IRHI rhi, EShaderType shaderType) : base(rhi)
        {
            ShaderType = shaderType;
        }
        #endregion

        public void SetHash(in ShaHash Hash)
        {
            hash = Hash;
        }
    }
}
