
using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Shaders
{
    public interface IShaderInstance : ITrackedResource, IEngineBindable, IEngineObservable
    {
        #region Accessors
        int Handle { get; }
        bool IsValid { get; }
        #endregion

        #region Descriptors
        /// <summary>
        /// List of descriptors for all the shader programs variables.
        /// </summary>
        ReferencedDataDescriptor[] Variables { get; }

        /// <summary>
        /// List of descriptors for all the shader programs vertex attributes.
        /// </summary>
        ReferencedDataDescriptor[] Attributes { get; }
        #endregion

        #region Compiler
        bool Compile();
        #endregion

        #region Includes
        bool Update(string Name, string Code);
        bool Include(EShaderType ShaderType, string Code, string Name);
        #endregion
    }
}
