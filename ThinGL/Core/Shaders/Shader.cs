using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Shaders
{
    public sealed class Shader : IShader
    {
        #region Values
        private readonly IShaderInstance _instance;
        #endregion

        #region Properties
        public IShaderInstance Instance => _instance;
        private bool disposedValue;
        #endregion

        #region Constructors
        public Shader(IRenderEngine Engine)
        {
            _instance = Engine.Provider.Shaders.Create(Engine);
        }
        #endregion


        public bool Compile() => Instance.Compile();
        public bool Update(string Name, string Code) => Instance.Update(Name, Code);
        public bool Include(EShaderType ShaderType, string Name, string Code) => Instance.Include(ShaderType, Code, Name);


        #region IEngineBindable
        public void Bind() => Instance.Bind();
        public void Unbind() => Instance.Unbind();
        public void Bound() => Instance.Bound();
        public void Unbound() => Instance.Unbound();
        #endregion


        #region IDisposable
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Instance?.Dispose();
                }

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Shader()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
        #endregion
    }
}