
using System;
using System.Numerics;

using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Units.Types
{
    #region Flag Constants
    [System.Flags]
    public enum ETransformFlags
    {
        None = 0x0,
        /// <summary> The transformation has a parent </summary>
        IsParented = 0x1,
        /// <summary> Notice for the owning object of the transformation that it has been changed </summary>
        Changed = 0x2,

        Invalid = (ParentChanged | PositionChanged | OrientationChanged),
        ParentChanged = 0x02,
        PositionChanged = 0x04,
        OrientationChanged = 0x08,
    }
    #endregion

    /// <summary>
    /// Handles positioning and orientation for an object
    /// </summary>
    public class Transform
    {
        #region Events
        public event Action OnChange;
        #endregion

        #region Values
        WeakReference<Transform> _parentRef;
        WeakReference<IRenderEngine> _engineRef;
        private Vector3 _pos;
        private Quaternion _orientation;
        private Matrix4x4 _matrix;
        #endregion

        #region Properties
        #endregion

        #region Flags
        protected ETransformFlags flags = ETransformFlags.None;
        public void Set_Flags(ETransformFlags Flags) => this.flags |= Flags;
        public void Clear_Flags(ETransformFlags Flags) => this.flags &= ~Flags;
        public bool Has_Flag(ETransformFlags Flag) => this.flags.HasFlag(Flag);

        /// <summary>
        /// Tests for a set of flags, returning those which are set.
        /// </summary>
        public ETransformFlags Get_Flags(ETransformFlags Flags) => (this.flags & Flags);
        #endregion

        #region Accessors
        public IRenderEngine Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        public Transform Parent => _parentRef.TryGetTarget(out var outRef) ? outRef : null;

        public bool IsParented => this.flags.HasFlag(ETransformFlags.IsParented);
        public bool IsInvalidated => this.flags.HasFlag(ETransformFlags.Invalid);
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
                    Set_Flags(ETransformFlags.PositionChanged | ETransformFlags.Changed);
                    OnChange?.Invoke();
                }
            }
        }
        #endregion

        #region Orientation
        public Quaternion Orientation
        {
            get => _orientation;
            set
            {
                var changed = !_orientation.Equals(value);
                if (changed)
                {
                    _orientation = value;
                    Set_Flags(ETransformFlags.OrientationChanged | ETransformFlags.Changed);
                    OnChange?.Invoke();
                }
            }
        }
        #endregion

        #region Constructors
        public Transform(IRenderEngine Engine)
        {
            _parentRef = new WeakReference<Transform>(null);
            _engineRef = new WeakReference<IRenderEngine>(Engine);
            _pos = Vector3.Zero;
            _orientation = Quaternion.Identity;
            _matrix = Matrix4x4.Identity;
        }
        #endregion

        #region Directions
        public Vector3 Get_Up() => Vector3.Transform(Engine.Space.Up, (Get_Transformed_Rotation()));
        public Vector3 Get_Down() => Vector3.Transform(Engine.Space.Down, (Get_Transformed_Rotation()));
        public Vector3 Get_Forward() => Vector3.Transform(Engine.Space.Forward, (Get_Transformed_Rotation()));
        public Vector3 Get_Back() => Vector3.Transform(Engine.Space.Backward, (Get_Transformed_Rotation()));
        public Vector3 Get_Right() => Vector3.Transform(Engine.Space.Right, (Get_Transformed_Rotation()));
        public Vector3 Get_Left() => Vector3.Transform(Engine.Space.Left, (Get_Transformed_Rotation()));
        #endregion

        #region Rotation
        public void Rotate(Vector3 Axis, float Angle)
        {
            if (float.IsSubnormal(Angle) || Angle == 0f)
                return;

            var qA = Quaternion.CreateFromAxisAngle(Axis, MathE.ToRadians(Angle));
            var qR = Quaternion.Multiply(qA, _orientation);

            Orientation = Quaternion.Normalize(qR);
        }
        #endregion

        #region Matrix Accessors
        /// <summary>
        /// The full camera transformation matrix (combined view & projection matricies)
        /// </summary>
        public Matrix4x4 Matrix
        {
            get
            {
                if (IsInvalidated)
                {
                    var mat = calculate_matrix();
                    _matrix = mat;
                    Clear_Flags(ETransformFlags.Invalid);
                    Set_Flags(ETransformFlags.Changed);
                    return mat;
                }

                return _matrix;
            }
        }

        protected virtual Matrix4x4 calculate_matrix()
        {
            var posMatrix = Matrix4x4.CreateTranslation(_pos);
            var rotMatrix = Matrix4x4.CreateFromQuaternion(_orientation);

            var mtx = Matrix4x4.Multiply(posMatrix, rotMatrix);
            if (IsParented)
            {
                mtx *= Parent.Matrix;
            }

            return mtx;
        }
        #endregion

        #region Parenting
        /// <summary>
        /// Unparents the transform form its current parent
        /// </summary>
        private void Unparent()
        {
            if (!IsParented) return;
            if (Parent is object)
            {
                Parent.OnChange -= _on_parent_change;
            }

            _parentRef.SetTarget(null);
            Clear_Flags(ETransformFlags.IsParented);
            Set_Flags(ETransformFlags.ParentChanged | ETransformFlags.Changed);
            OnChange?.Invoke();
        }

        public void Set_Parent(Transform parent)
        {
            if (parent is null)
            {
                throw new ArgumentNullException(nameof(parent));
            }
            else
            {
                if (ReferenceEquals(this, parent)) return;
                Unparent();

                _parentRef.SetTarget(parent);
                parent.OnChange += _on_parent_change;

                Set_Flags(ETransformFlags.IsParented | ETransformFlags.ParentChanged | ETransformFlags.Changed);
            }
        }

        private void _on_parent_change()
        {
            if (Parent is object)
            {
                Set_Flags(ETransformFlags.Changed | Parent.Get_Flags(ETransformFlags.Invalid));
            }
            else
            {
                Set_Flags(ETransformFlags.Changed);
            }
            OnChange?.Invoke();
        }
        #endregion

        #region Transformed Values

        /// <summary>
        /// Gets the position with any parent transformations applied
        /// </summary>
        /// <returns></returns>
        public Vector3 Get_Transformed_Position()
        {
            if (IsParented)
            {
                return Vector3.Transform(_pos, Parent.Matrix);
            }

            return _pos;
        }

        public Quaternion Get_Transformed_Rotation()
        {
            if (IsParented)
            {
                return Quaternion.Multiply(Parent.Orientation, _orientation);
            }

            return _orientation;
        }
        #endregion
    }
}
