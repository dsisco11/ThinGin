using System;
using System.Numerics;

using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Cameras
{
    /// <summary>
    /// Provides a 3D camera with translation/orientation and field of view.
    /// </summary>
    public class CameraEuler : CameraObject
    {
        #region Constants
        const float fovMin = float.Epsilon * 1000;
        const float fovMax = 179.99999f;// This is about as close to 180 we can get without screwing the matrix up
        #endregion

        #region Values
        private Vector3 _pos;
        private Vector3 _rot;

        private float _fov = 65f;
        private float _zoom = 1.0f;
        private ECameraViewMode _viewMode = ECameraViewMode.FirstPerson;
        private EProjectionMode _projectionMode = EProjectionMode.Perspective;
        #endregion

        #region Properties
        public ECameraViewMode ViewMode
        {
            get => _viewMode;
            set
            {
                if (_viewMode != value)
                {
                    _viewMode = value;
                    invalidate_view_matrix();
                }
            }
        }

        public EProjectionMode ProjectionMode
        {
            get => _projectionMode;
            set
            {
                if (_projectionMode != value)
                {
                    _projectionMode = value;
                    invalidate_projection_matrix();
                }
            }
        }


        /// <summary>
        /// Field of view angular value (in degrees) for perspective projection
        /// </summary>
        public float FieldOfView
        {
            get => _fov;
            set
            {
                if (!MathE.AlmostEquals(_fov, value))
                {
                    _fov = MathE.Clamp(value, fovMin, fovMax);

                    if (ProjectionMode != EProjectionMode.Orthographic)
                        invalidate_projection_matrix();
                }
            }
        }

        /// <summary>
        /// Zoom scalar value for orthographic projection.
        /// </summary>
        public float Zoom 
        { 
            get => _zoom; 
            set
            {

                if (!MathE.AlmostEquals(_zoom, value))
                {
                    _zoom = value;

                    if (ProjectionMode == EProjectionMode.Orthographic)
                        invalidate_projection_matrix();
                }
            }
        }
        #endregion

        #region Translation
        /// <summary>
        /// <inheritdoc cref="ICamera.Position"/>
        /// </summary>
        public Vector3 Position
        {
            get => _pos;
            set
            {
                var changed = !_pos.Equals(value);
                if (changed)
                {
                    _pos = value;
                    invalidate_view_matrix();
                }
            }
        }
        #endregion

        #region Orientation
        public Vector3 Rotation
        {
            get => _rot;
            set
            {
                var changed = !_rot.Equals(value);
                if (changed)
                {
                    _rot = value;
                    invalidate_view_matrix();
                }
            }
        }

        #endregion

        #region Constructors
        public CameraEuler(EngineInstance Engine) : base(Engine)
        {
        }
        #endregion

        #region Matrix Computation
        protected override Matrix4x4 recalculate_view_matrix()
        {
            var r = Rotation;
            var q = Quaternion.CreateFromYawPitchRoll(MathE.ToRadians(r.Z),
                                                      MathE.ToRadians(r.X),
                                                      MathE.ToRadians(r.Y));

            var rotMatrix = Matrix4x4.CreateFromQuaternion(q);
            var posMatrix = Matrix4x4.CreateTranslation(Vector3.Negate(_pos));

            switch (ViewMode)
            {
                case ECameraViewMode.FirstPerson:
                    {
                        return rotMatrix * posMatrix;
                    }
                case ECameraViewMode.Orbital_Free:
                    {
                        return posMatrix * rotMatrix;
                    }
                default:
                    throw new NotImplementedException(ViewMode.ToString());
            }

            //var forward = Vector3.Transform(Vector3.UnitX, q);
            //return Matrix4x4.CreateLookAt(_pos, _pos + forward, Vector3.UnitZ);
        }

        protected override Matrix4x4 recalculate_projection_matrix()
        {
            switch (ProjectionMode)
            {
                case EProjectionMode.Perspective:
                    {
                        var fovR = MathE.Max(MathE.ToRadians(FieldOfView), float.Epsilon);
                        return Matrix4x4.CreatePerspectiveFieldOfView(fovR, AspectRatio, _nearClip, _farClip);
                    }
                case EProjectionMode.Orthographic:
                    {
                        var vp = Engine.Get_Viewport();

                        return Matrix4x4.CreateOrthographicOffCenter(-1, 1, -1, 1, _nearClip, _farClip);
                        //return Matrix4x4.CreateOrthographic(vp.Width, vp.Height, _nearClip, _farClip);
                    }
                default:
                    throw new NotImplementedException(ProjectionMode.ToString());
            }
        }

        #endregion
    }
}
