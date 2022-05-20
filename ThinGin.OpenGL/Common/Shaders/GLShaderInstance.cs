using ThinGin.Core.Common.Interfaces;
using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using System;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Shaders;
using System.Linq;
using System.Collections.Generic;
using ThinGin.Core.RenderHardware.Resources;

namespace ThinGin.OpenGL.Common.Shaders
{
    /// <summary>
    /// </summary>
    public class GLShaderInstance : RHIResource, IShaderInstance, IEngineObservable
    {
        #region Values
        public int Handle { get; protected set; } = 0;
        /// <summary>
        /// Tracks the handles for all currently linked shaders
        /// </summary>
        protected int[] linked = new int[Enum.GetValues(typeof(EShaderType)).Cast<int>().Max() + 1];

        //protected ShaderInclude[] Includes = new ShaderInclude[Enum.GetValues(typeof(EShaderType)).Cast<int>().Max() + 1];
        protected Dictionary<string, GLShaderInclude> Includes = new Dictionary<string, GLShaderInclude>();
        #endregion

        #region Properties
        /// <summary> <inheritdoc cref="IShader.Variables"/> </summary>
        public ReferencedDataDescriptor[] Variables { get; protected set; }
        public ReferencedDataDescriptor[] Attributes { get; protected set; }
        #endregion

        #region Accessors
        public bool IsValid { get; protected set; } = false;
        #endregion

        #region Events
        public event EventHandler OnChanged;
        #endregion


        #region Constructors
        public GLShaderInstance(IEngine Engine) : base(Engine)
        {
        }
        #endregion

        #region Includes
        public bool Include(EShaderType ShaderType, string Code, string Name)
        {
            if (!Includes.ContainsKey(Name))
            {
                var inc = new GLShaderInclude(RHI, Name, ShaderType, Code);
                Includes.Add(Name, inc);
                inc.OnChanged += include_updated;
                return true;
            }
            else if (Includes.TryGetValue(Name, out GLShaderInclude outInclude))
            {
                outInclude.Code = Code;
                return true;
            }

            return false;
        }

        public bool Update(string Name, string Code)
        {
            if (Includes.TryGetValue(Name, out GLShaderInclude outInclude))
            {
                outInclude.Code = Code;
                return true;
            }

            return false;
        }

        private void include_updated(object sender, EventArgs e)
        {
            Invalidate();
        }
        #endregion


        #region Compiling
        public bool Compile()
        {
            GL.GetError();// Throw away whatever unrelated error might exist
            IsValid = false;// first we list the shader as unuseable

            if (!EnsureInitialized())
            {
                System.Diagnostics.Trace.TraceWarning($"[SHADER]::[COMPILER]: Attempt to compile shader before it can be initialized!");
                return false;
            }

            // Go through all of the shader objects we should have linked and do any linking or unlinking needed
            foreach (GLShaderInclude include in Includes.Values)
            {
                if (!include.EnsureReady())
                {
                    System.Diagnostics.Trace.TraceWarning($"[SHADER]::[COMPILER]::[INCLUDE]: Unable to process include!");
                }

                GL.AttachShader(Handle, include.Handle);
            }

            // Run the linker on our program to compile it
            GL.LinkProgram(Handle);
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out var linkerResult);
            if (linkerResult == 0)
            {
                System.Diagnostics.Trace.TraceWarning($"[SHADER]::[COMPILER]: Failed to link shader program!");
                System.Diagnostics.Trace.TraceWarning(GL.GetProgramInfoLog(Handle));
                return false;
            }

            GL.ValidateProgram(Handle);
            GL.GetProgram(Handle, GetProgramParameterName.ValidateStatus, out var validationResult);
            if (validationResult == 0)
            {
                System.Diagnostics.Trace.TraceWarning($"[SHADER]::[COMPILER]: Failed to validate shader program!");
                System.Diagnostics.Trace.TraceWarning(GL.GetProgramInfoLog(Handle));
                return false;
            }

            var infoLog = GL.GetProgramInfoLog(Handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                System.Diagnostics.Trace.TraceInformation($"[SHADER]::[COMPILER]::[LOG]: {infoLog}");
            }

            IsValid = true;

            Update_Variable_Descriptors();
            Update_Attribute_Descriptors();

            OnChanged?.Invoke(this, new EventArgs());
            return true;
        }
        #endregion

        #region Variable Management
        protected void Update_Variable_Descriptors()
        {
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int itemCount);
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniformMaxLength, out int maxNameLength);

            var items = new ReferencedDataDescriptor[itemCount];

            for (int id = 0; id < itemCount; id++)
            {
                GL.GetActiveUniform(Handle, id, maxNameLength, out int nameLength, out int ArraySize, out ActiveUniformType outType, out string outName);

                var uType = Bridge.Translate(outType);
                EValueType valueType = Bridge.Translate(uType);

                items[id] = new ReferencedDataDescriptor(Handle: id,
                                                                        Context: uType,
                                                                        Name: outName,
                                                                        Count: ArraySize,
                                                                        ValueType: valueType);
            }

            Variables = items;
        }

        #endregion

        #region Geometry Attribute Management

        protected void Update_Attribute_Descriptors()
        {
            GL.GetProgram(Handle, GetProgramParameterName.ActiveAttributes, out int itemCount);
            GL.GetProgram(Handle, GetProgramParameterName.ActiveAttributeMaxLength, out int maxNameLength);

            var items = new ReferencedDataDescriptor[itemCount];

            for (int id = 0; id < itemCount; id++)
            {
                GL.GetActiveAttrib(Handle, id, maxNameLength, out int nameLength, out int ArraySize, out ActiveAttribType outType, out string outName);

                var uType = Bridge.Translate(outType);
                EValueType valueType = Bridge.Translate(uType);

                items[id] = new ReferencedDataDescriptor(Handle: id,
                                                                        Context: uType,
                                                                        Name: outName,
                                                                        Count: ArraySize,
                                                                        ValueType: valueType);
            }
        }

        #endregion

        #region IEngineBindable
        public void Bind()
        {
            GL.UseProgram(Handle);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public void Bound()
        {
        }

        public void Unbound()
        {
        }
        #endregion

        #region IEngineObject
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => { Handle = GL.CreateProgram(); });
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(Handle, GL.DeleteProgram);
        public override IEngineDelegate Get_Updater() => new EngineDelegate(() => Compile());
        #endregion
    }
}