using System;
using System.Numerics;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Common.Units.Types;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Common.Cameras
{
    /// <summary>
    /// Provides a 3D camera with translation/orientation and field of view.
    /// </summary>
    public class Camera : CameraBase
    {
        #region Constants
        const float fovMin = float.Epsilon * 1000;
        const float fovMax = 179.99999f;// This is about as close to 180 we can get without screwing the matrix up
        #endregion

        #region Values
        private float _fov = 75f;
        private float _zoom = 1.0f;

        private ECameraViewMode _viewMode = ECameraViewMode.FirstPerson;
        private EProjectionMode _projectionMode = EProjectionMode.Perspective;

        // Camera Shake
        private bool _trauma_enabled = false;
        private bool _traumatized = false;
        private double _trauma = 0f;
        private double _trauma_decay = 0.1d;
        private Noise.CubicNoise _trauma_noise;

        // Smoothing
        private bool _smoothing = false;
        private float _smoothing_strength = 0.2f;
        // How much time has passed since the last smoothing application
        private double _smoothing_time_backlog = 0d;
        private Vector3 _smooth_pos;
        private Quaternion _smooth_rot;
        #endregion

        #region Accessors
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

        #region Trauma
        /// <summary>
        /// Enables camera shake, formally known as "trauma". Use <see cref="Add_Trauma(double)"/> to add screen shake
        /// </summary>
        public bool EnableShake
        {
            get => _trauma_enabled;
            set
            {
                _trauma_enabled = value;
                _trauma_noise = !value ? null : new Noise.CubicNoise(Engine.World.SeedUnstable, new Noise.Squirrel3Noise(), 3);
                Invalidate();
            }
        }

        /// <summary>
        /// Controls how fast the Trauma value which controls camera shake will decay over time.
        /// </summary>
        public double Trauma_Decay
        {
            get => _trauma_decay;
            set => _trauma_decay = value;
        }
        #endregion

        #region Smoothing
        /// <summary>
        /// Enables Asymptotic camera movement
        /// </summary>
        public bool EnableSmoothing
        {
            get => _smoothing;
            set
            {
                _smoothing = value;
                _smooth_pos = Transform.Get_Transformed_Position();
                Invalidate();
            }
        }

        /// <summary>
        /// Controls how fast the cameras smoothed movement will... move...
        /// </summary>
        public float SmoothingStrength
        {
            get => _smoothing_strength;
            set => _smoothing_strength = value;
        }
        #endregion

        #region Constructors
        public Camera(EngineInstance engine) : base(engine)
        {
        }
        #endregion

        #region Matrix Computation
        protected override Matrix4x4 recalculate_view_matrix()
        {
            var camPos = Transform.Get_Transformed_Position();
            var camRot = Transform.Get_Transformed_Rotation();

            switch (ViewMode)
            {
                case ECameraViewMode.FirstPerson:
                    {
                        if (_smoothing)
                        {
                            apply_smoothing(camPos, out camPos);
                        }

                        if (_traumatized)
                        {
                            apply_trauma(camPos, camRot, out camPos, out camRot);
                        }

                        camRot = Quaternion.Conjugate(camRot);
                        var rotMatrix = Matrix4x4.CreateFromQuaternion(camRot);
                        var posMatrix = Matrix4x4.CreateTranslation(Vector3.Negate(camPos));

                        return posMatrix * rotMatrix;
                    }
                case ECameraViewMode.Orbital:
                case ECameraViewMode.Orbital_Free:
                    {
                        if (_smoothing)
                        {
                            apply_smoothing(camRot, out camRot);
                        }

                        if (_traumatized)
                        {
                            apply_trauma(camPos, camRot, out camPos, out camRot);
                        }

                        var rotMatrix = Matrix4x4.CreateFromQuaternion(camRot);
                        var posMatrix = Matrix4x4.CreateTranslation(camPos);

                        return rotMatrix * posMatrix;
                    }
                default:
                    throw new NotImplementedException(ViewMode.ToString());
            }
        }

        protected override Matrix4x4 recalculate_projection_matrix()
        {
            switch (ProjectionMode)
            {
                case EProjectionMode.Perspective:
                    {
                        var fovR = MathE.Max(MathE.ToRadians(FieldOfView), float.Epsilon);
                        var mtx = Matrix4x4.CreatePerspectiveFieldOfView(fovR, AspectRatio, _nearClip, _farClip);
                        return Engine.Space.Orientation * mtx;

                        //var correctionMatrix = Matrix4x4.CreateRotationZ(MathE.ToRadians(180f));
                        //return correctionMatrix * mtx;
                        //return mtx;
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

        #region Events
        public override void OnTick(double time)
        {
            base.OnTick(time);
            Update_Trauma(time);

            if (_smoothing)
            {
                _smoothing_time_backlog += time;
                Invalidate();
            }
        }
        #endregion


        #region Utility
        public MeshBuilder Get_Debug_Vis()
        {
            const float Size = 0.25f;

            Vector3 vu = Transform.Get_Up();
            Vector3 vr = Transform.Get_Right();
            Vector3 vf = Transform.Get_Forward();

            vu *= Size;
            vr *= Size;
            vf *= Size;

            var points = new[] {
                new DataChunk(0f, 0f, 0f),
                new DataChunk(vf.X, vf.Y, vf.Z),
                new DataChunk(vr.X, vr.Y, vr.Z),
                new DataChunk(vu.X, vu.Y, vu.Z),
            };

            var colors = new[] {
                new DataChunk(1f, 0f, 0f),
                new DataChunk(0f, 1f, 0f),
                new DataChunk(0f, 0f, 1f),
            };

            var vertexLayout = new VertexLayout(
                Position: AttributeDescriptor.FLOAT3,
                Color: AttributeDescriptor.FLOAT3
                );

            var builder = new MeshBuilder(vertexLayout, ETopology.LineList, AutoOptimize: false);

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Position), points);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Position), new[] { 0, 1, 0, 2, 0, 3 });

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Color), colors);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Color), new[] { 0, 0, 1, 1, 2, 2 });

            return builder;
        }
        #endregion

        #region Movement Smoothing
        protected void apply_smoothing(Vector3 Pos, out Vector3 outPos)
        {// x += (target - x) * strength
            var factor = _smoothing_strength * (float)_smoothing_time_backlog;

            var delta = Vector3.Subtract(Pos, _smooth_pos);
            delta = Vector3.Multiply(factor, delta);
            _smooth_pos = Vector3.Add(_smooth_pos, delta);

            outPos = _smooth_pos;
            _smoothing_time_backlog = 0d;
        }

        protected void apply_smoothing(Quaternion Rot, out Quaternion outRot)
        {
            var factor = _smoothing_strength * (float)_smoothing_time_backlog;
            outRot = _smooth_rot = Quaternion.Slerp(Rot, _smooth_rot, factor);
            _smoothing_time_backlog = 0d;
        }
        #endregion

        #region Camera Shake
        protected void apply_trauma(Vector3 Pos, Quaternion Rot, out Vector3 outPos, out Quaternion outRot)
        {
            var Time = (float)Engine.World.Time;
            switch (ProjectionMode)
            {
                case EProjectionMode.Perspective:
                    {// 3D: Rotation only (translation will cause the camera to clip into geometry)
                        // XXX: Need simplex noise
                        var xa = _trauma_noise.Sample(new Vector2(Time, 1));
                        var ya = _trauma_noise.Sample(new Vector2(Time, 2));
                        var za = _trauma_noise.Sample(new Vector2(Time, 3));

                        var xr = Quaternion.CreateFromAxisAngle(Vector3.UnitX, xa);
                        var yr = Quaternion.CreateFromAxisAngle(Vector3.UnitY, ya);
                        var zr = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, za);


                    }
                    break;
                case EProjectionMode.Orthographic:
                    {// 2D: Translation + Rotation
                    }
                    break;
                default:
                    throw new NotImplementedException(ProjectionMode.ToString());
            }
        }

        private double Get_Trauma_Factor() => (_trauma * _trauma * _trauma);

        private void Update_Trauma(double time)
        {
            var oldTrauma = _trauma;
            _trauma = Math.Max(0f, _trauma - (time * _trauma_decay));

            if (oldTrauma != _trauma)
            {
                _traumatized = (_trauma != 0);

                invalidate_view_matrix();
                Invalidate();
            }
        }

        public void Add_Trauma(double trauma)
        {
            _trauma += trauma;
            _traumatized = true;
        }
        #endregion
    }
}
