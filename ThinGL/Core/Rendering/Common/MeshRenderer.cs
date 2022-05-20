
using System;

using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.Core.Engine.Common.Core;

namespace ThinGin.Core.Rendering.Common
{
    public abstract class MeshRenderer : GObject, IMeshRenderer
    {
        #region Properties
        protected WeakReference<Mesh> _meshRef = null;
        #endregion

        #region Accessors
        protected Mesh mesh => _meshRef.TryGetTarget(out Mesh meshOut) ? meshOut : null;
        #endregion

        #region Constructors
        public MeshRenderer(EngineInstance Engine, Mesh mesh) : base(Engine)
        {
            _meshRef = new WeakReference<Mesh>(mesh);
        }
        #endregion

        #region Disposal
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_meshRef is object)
            {
                _meshRef = null;
            }
        }
        #endregion

        #region Rendering
        public abstract void Render(ETopology Primitive);
        #endregion
    }
}
