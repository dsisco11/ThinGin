using System;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Shaders
{
    public interface IShader : IEngineBindable, IDisposable
    {
        IShaderInstance Instance { get; }
        bool Compile();
        bool Update(string Name, string Code);
        bool Include(EShaderType ShaderType, string Name, string Code);
    }
}
