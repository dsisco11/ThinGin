
using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.OpenGL.Common.Shaders
{
    public class GLShaderInclude : GObject, IEngineObservable
    {
        #region Values
        private string _code;
        #endregion

        #region Properties
        public int Handle { get; protected set; } = 0;
        public readonly string Name;
        public readonly EShaderType Type;
        #endregion

        #region Accessors
        public string Code 
        { 
            get => _code; 
            set
            {
                _code = value;
                Invalidate();
            }
        }
        public bool IsValid { get { return Handle != 0; } }
        #endregion

        #region Events
        public event EventHandler OnChanged;
        #endregion

        #region Constructors
        public GLShaderInclude(EngineInstance Engine, string Name, EShaderType Type, string Code) : base(Engine)
        {
            this.Code = Code;
            this.Name = Name;
            this.Type = Type;
        }
        #endregion

        #region Compiling
        public void Compile()
        {
            if (Handle == 0)
            {
                Handle = GL.CreateShader(Bridge.Translate(Type));
            }

#if DEBUG
            System.Diagnostics.Trace.TraceInformation($"============= [{Name}] =============");
            System.Diagnostics.Trace.TraceInformation("[SHADER]::[SOURCE]:");
            System.Diagnostics.Trace.TraceInformation($"\r\n{Code}\r\n\r\n");
#endif


            GL.ShaderSource(Handle, Code);
            GL.CompileShader(Handle);

            GL.GetShader(Handle, ShaderParameter.CompileStatus, out var result);
            if (result == 0)
            {
                System.Diagnostics.Trace.TraceWarning($"[SHADER]::[COMPILER]: Failed to compile shader! (Type: {Type}, Name: {Name})!");
                System.Diagnostics.Trace.TraceWarning(GL.GetShaderInfoLog(Handle));
                // Don't leak the shader!
                GL.DeleteShader(Handle);
                Handle = 0;
                return;
            }


            string infoLog = GL.GetShaderInfoLog(Handle);
            if (!string.IsNullOrEmpty(infoLog))
            {
                System.Diagnostics.Trace.TraceInformation($"[SHADER]::[COMPILER]::[LOG]: {infoLog}");
            }

            OnChanged?.Invoke(this, new EventArgs());
        }
        #endregion


        #region IEngineObject
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => { Handle = GL.CreateShader(Bridge.Translate(Type)); });
        public override IEngineDelegate Get_Updater() => new EngineDelegate(Compile);
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(Handle, GL.DeleteShader);
        #endregion
    }
}
