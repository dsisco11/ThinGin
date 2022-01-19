using System;
using System.Numerics;

using ThinGin.Core.Common.Data;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Common.Units.Types;

namespace ThinGin.Core.Common.Cameras
{
    // XXX: Need to abstract the position/orientation stuff here into a new "Transformation" class that handles it and provides "Rotation" and "Position" accessors.
    // XXX: Need a new "Rotation" object that handles quaternions and relative orientation vectors

    /// <summary>
    /// Provides a 3D camera with translation/orientation and field of view.
    /// </summary>
    public class Camera : CameraObject
    {
        #region Constants
        const float fovMin = float.Epsilon * 1000;
        const float fovMax = 179.99999f;// This is about as close to 180 we can get without screwing the matrix up
        #endregion

        #region Values
        private Vector3 _pos = Vector3.Zero;
        private Quaternion _orientation = Quaternion.Identity;

        private float _fov = 75f;
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

        #region Constructors
        public Camera(IEngine Engine) : base(Engine)
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
                        camRot = Quaternion.Conjugate(camRot);
                        var rotMatrix = Matrix4x4.CreateFromQuaternion(camRot);
                        var posMatrix = Matrix4x4.CreateTranslation(Vector3.Negate(camPos));
                        //var posMatrix = Matrix4x4.CreateTranslation((camPos));

                        return posMatrix * rotMatrix;
                    }
                case ECameraViewMode.Orbital:
                case ECameraViewMode.Orbital_Free:
                    {
                        //camRot = Quaternion.Inverse(camRot);
                        var rotMatrix = Matrix4x4.CreateFromQuaternion(camRot);
                        var posMatrix = Matrix4x4.CreateTranslation(camPos);
                        //var posMatrix = Matrix4x4.CreateTranslation(Vector3.Negate(_pos));

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


        #region Utility

        public void Rotate(Vector3 Axis, float Angle)
        {
            if (float.IsSubnormal(Angle) || Angle == 0f)
                return;

            var qA = Quaternion.CreateFromAxisAngle(Axis, MathE.ToRadians(Angle));
            var qR = Quaternion.Multiply(qA, _orientation);
            //Orientation = Quaternion.Normalize(qR);
        }

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

            var builder = new MeshBuilder(vertexLayout, ETopology.Lines, AutoOptimize: false);

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Position), points);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Position), new[] { 0, 1, 0, 2, 0, 3 });

            builder.Push_Data(vertexLayout.IndexOf(EVertexAttribute.Color), colors);
            builder.Push_Indices(vertexLayout.IndexOf(EVertexAttribute.Color), new[] { 0, 0, 1, 1, 2, 2 });

            return builder;
        }
        #endregion
    }
}
