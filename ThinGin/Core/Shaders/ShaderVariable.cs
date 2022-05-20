
using System;
using System.Numerics;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Shaders
{
    /// <summary>
    /// Represents a uniform variable within a shader, manages storing the ID and data for the variable to facilitate optimal performance.
    /// </summary>
    public abstract class ShaderVariable : IEngineObservable
    {
        protected delegate void DataSubmissionDelegate(DataChunk chunk);

        #region Values
        protected WeakReference<IShaderInstance> _shaderRef;
        protected ReferencedDataDescriptor Descriptor;
        private DataChunk _data = null;
        private DataSubmissionDelegate submissionDelegate = null;
        #endregion

        #region Properties
        public int ID => (int)Descriptor.Handle;
        #endregion

        #region Accessors
        public IShaderInstance Shader => _shaderRef.TryGetTarget(out var outShader) ? outShader : null;
        public string Name => Descriptor.Name;

        public DataChunk Data 
        { 
            get => _data; 
            protected set
            {
                if (ReferenceEquals(_data, value))
                    return;

                var newData = PreProcess(value);
                if (!DataChunk.Equals(_data, newData))
                {
                    _data = newData;
                    OnChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler OnChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a copy of the given uniform
        /// </summary>
        public ShaderVariable(ShaderVariable other)
        {
            _shaderRef = new WeakReference<IShaderInstance>(other.Shader);
            Descriptor = other.Descriptor;
            _data = new DataChunk(other.Data);
            OnChanged = other.OnChanged;
        }

        /// <summary>
        /// Creates a copy of the given uniform with a new descriptor
        /// </summary>
        public ShaderVariable(ShaderVariable other, ReferencedDataDescriptor Descriptor)
        {
            this.Descriptor = Descriptor;
            _shaderRef = new WeakReference<IShaderInstance>(other.Shader);
            _data = new DataChunk(other.Data);
            OnChanged = other.OnChanged;
        }

        public ShaderVariable(IShaderInstance Shader, ReferencedDataDescriptor Descriptor)
        {
            _shaderRef = new WeakReference<IShaderInstance>(Shader);
            this.Descriptor = Descriptor;
        }

        #endregion

        /// <summary>
        /// Uploads the uniform value to the shader program
        /// </summary>
        public void Submit()
        {
            if (Data is null)
            {
                return;
            }

            if (Descriptor is null)
            {
                return;
            }

            if (submissionDelegate is null)
            {
                submissionDelegate = getSubmissionDelegate();
            }

            submissionDelegate?.Invoke(this._data);
        }

        protected abstract DataSubmissionDelegate getSubmissionDelegate();

        /// <summary>
        /// Allows implementations to convert data into value types compatable for their specific implementation.
        /// </summary>
        /// <param name="dataChunk"></param>
        /// <returns></returns>
        protected abstract unsafe DataChunk PreProcess(DataChunk dataChunk);

        #region Setters
        public virtual void Set(DataChunk data) { Data = data; }
        public virtual void Set(Vector2 value) { Data = new DataChunk(value); }
        public virtual void Set(Vector3 value) { Data = new DataChunk(value); }
        public virtual void Set(Vector4 value) { Data = new DataChunk(value); }
        public virtual void Set(Matrix3x2 value) { Data = new DataChunk(value); }
        public virtual void Set(Matrix4x4 value) { Data = new DataChunk(value); }
        #endregion
    }
}
