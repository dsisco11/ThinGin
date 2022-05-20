
using ThinGin.Core.Common.Interfaces;
using System;
using System.Numerics;
using ThinGin.Core.Common.Units.Types;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common
{
    /// <summary>
    /// Provides a framework for camera objects
    /// </summary>
    public abstract class CameraObject : ICamera
    {
        #region Values
        private WeakReference<EngineInstance> _engineRef = new WeakReference<EngineInstance>(null);

        protected float _farClip = 500f;// x meters is a good default yea?
        protected float _nearClip = 0.01f;

        // Note: We use lazy computation for matrix objects to save computing resources, matricies are only actually built once they are requested
        private Matrix4x4? _matrix = null;
        private Matrix4x4? _viewMatrix = null;
        private Matrix4x4? _projectionMatrix = null;
        private Transform _transform;
        #endregion

        #region Accessors
        public Transform Transform => _transform;
        public IEngine Engine => _engineRef.TryGetTarget(out var outEngine) ? outEngine : null;
        public float AspectRatio => MathE.Max(float.Epsilon, Engine.Get_AspectRatio());
        #endregion

        #region Matrix Accessors
        /// <summary>
        /// The full camera transformation matrix (combined view & projection matricies)
        /// </summary>
        public Matrix4x4 Matrix
        {
            get
            {
                validate_transform();
                if (!_matrix.HasValue)
                {
                    var mat = recalculate_transformation_matrix();
                    _matrix = mat;
                    return mat;
                }

                return _matrix.Value;
            }
        }

        /// <summary>
        /// The translation/orientation portion of the camera matrix
        /// </summary>
        public Matrix4x4 ViewMatrix
        {
            get
            {
                validate_transform();
                if (!_viewMatrix.HasValue)
                {
                    var mat = recalculate_view_matrix();
                    _viewMatrix = mat;
                    return mat;
                }
                return _viewMatrix.Value;
            }
        }

        /// <summary>
        /// The projection portion of the camera matrix
        /// </summary>
        public Matrix4x4 ProjectionMatrix
        {
            get
            {
                if (!_projectionMatrix.HasValue)
                {
                    var mat =recalculate_projection_matrix();
                    _projectionMatrix = mat;
                    return mat;
                }

                return _projectionMatrix.Value;
            }
        }

        #endregion

        #region Clipping region
        public float FarClippingPlane
        {
            get => _farClip;
            set
            {
                if (!MathF.Equals(value, _farClip))
                {
                    var a = value;
                    var b = _nearClip;

                    _farClip = MathE.Max(a, b);
                    _nearClip = MathE.Min(a, b);

                    invalidate_projection_matrix();
                }
            }
        }

        public float NearClippingPlane
        {
            get => _nearClip;
            set
            {
                if (!MathF.Equals(value, _nearClip))
                {
                    var a = value;
                    var b = _farClip;

                    _farClip = MathE.Max(a, b);
                    _nearClip = MathE.Min(a, b);

                    invalidate_projection_matrix();
                }
            }
        }
        #endregion

        #region Constructors
        protected CameraObject(EngineInstance Engine)
        {
            _engineRef.SetTarget(Engine);
            _transform = new Transform(Engine);
        }
        #endregion

        #region Invalidation

        protected void validate_transform()
        {
            if (_transform.Has_Flag(ETransformFlags.Changed))
            {
                invalidate_view_matrix();
                _transform.Clear_Flags(ETransformFlags.Changed);
            }
        }

        protected void invalidate_view_matrix()
        {
            _matrix = null;
            _viewMatrix = null;
        }

        protected void invalidate_projection_matrix()
        {
            _matrix = null;
            _projectionMatrix = null;
        }

        /// <summary>
        /// Forces the camera to recalculate its entire matrix stack (lazily, as in not immediately).
        /// </summary>
        public void Invalidate()
        {
            _matrix = null;
            _viewMatrix = null;
            _projectionMatrix = null;
        }
        #endregion

        #region Matrix Calculation
        protected abstract Matrix4x4 recalculate_view_matrix();
        protected abstract Matrix4x4 recalculate_projection_matrix();
        protected virtual Matrix4x4 recalculate_transformation_matrix()
        {
            return Matrix4x4.Multiply(ViewMatrix, ProjectionMatrix);
        }
        #endregion
    }
}
