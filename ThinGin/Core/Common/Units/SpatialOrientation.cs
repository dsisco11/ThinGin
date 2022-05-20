using System;
using System.Numerics;

using ThinGin.Core.Common.Enums;

namespace ThinGin.Core.Common.Units
{
    /// <summary>
    /// Describes orientation directions
    /// </summary>
    public class SpatialOrientation
    {
        #region Values
        private readonly Vector3 _up;
        private readonly Vector3 _right;
        private readonly Vector3 _forward;

        private readonly Vector3 _down;
        private readonly Vector3 _left;
        private readonly Vector3 _backward;

        private readonly Matrix4x4 _orientation;
        #endregion

        #region Accessors
        public Vector3 Up => _up;
        public Vector3 Right => _right;
        public Vector3 Forward => _forward;
        public Vector3 Down => _down;
        public Vector3 Left => _left;
        public Vector3 Backward => _backward;
        public Matrix4x4 Orientation => _orientation;
        #endregion

        #region Constructors
        public SpatialOrientation(EAxisDirection UpAxis = EAxisDirection.Z_Positive, EAxisDirection RightAxis = EAxisDirection.Y_Positive, EAxisDirection ForwardAxis = EAxisDirection.X_Positive)
        {
            _up = ToVector(UpAxis);
            _right = ToVector(RightAxis);
            _forward = ToVector(ForwardAxis);

            _down = Vector3.Negate(_up);
            _left = Vector3.Negate(_right);
            _backward = Vector3.Negate(_forward);

            var vu = _up;
            var vr = _right;
            var vf = _forward;

            vu = Vector3.Negate(vu);
            _orientation = new Matrix4x4(
                vu.X, vu.Y, vu.Z, 0f,
                vf.X, vf.Y, vf.Z, 0f,
                vr.X, vr.Y, vr.Z, 0f,
                  0f,   0f,   0f, 1f
                );

            //_orientation = new Matrix4x4(
            //    -vu.X, -vu.Y, -vu.Z, 0f,
            //    +vf.X, +vf.Y, +vf.Z, 0f,
            //    -vr.X, -vr.Y, -vr.Z, 0f,
            //       0f,    0f,    0f, 1f
            //    );
        }
        #endregion

        #region Utility
        private Vector3 ToVector(EAxisDirection axis)
        {
            return axis switch
            {
                EAxisDirection.X_Positive => new Vector3(+1f, 0f, 0f),
                EAxisDirection.X_Negative => new Vector3(-1f, 0f, 0f),
                EAxisDirection.Y_Positive => new Vector3(0f, +1f, 0f),
                EAxisDirection.Y_Negative => new Vector3(0f, -1f, 0f),
                EAxisDirection.Z_Positive => new Vector3(0f, 0f, +1f),
                EAxisDirection.Z_Negative => new Vector3(0f, 0f, -1f),
                _ => throw new NotImplementedException(),
            };
        }
        #endregion
    }
}
