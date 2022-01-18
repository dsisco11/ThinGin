
using OpenTK.Graphics.OpenGL;

using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Enums;
using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Meshes;
using ThinGin.OpenGL.Common.Types;

namespace ThinGin.OpenGL.Common.Renderers
{
    /// <summary>
    /// Implements an OpenGL v1.0 compiled draw-list for mesh data.
    /// <para>
    /// NOTE: Display lists are depreciated as of OpenGL v3.2.
    /// As they are an extremely crude form of rendering, barely surpassing immediate mode rendering on modern hardware.
    /// </para>
    /// </summary>
    public class DisplayListRenderer : ImmediateModeRenderer
    {
        #region Properties
        protected GLDisplayList displayList;
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public DisplayListRenderer(IRenderEngine Engine, Mesh mesh) : base(Engine, mesh)
        {
            displayList = new GLDisplayList(Engine, Initiate_Trainwreck);
        }
        #endregion

        #region Disposal
        protected override void Dispose(bool disposing)
        {
            if (displayList is object)
            {
                displayList?.Dispose();
                displayList = null;
            }
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => null;
        public override IEngineDelegate Get_Releaser() => null;
        public override IEngineDelegate Get_Updater() => new EngineDelegate(_sub_update_display_list);

        private void _sub_update_display_list()
        {
            if (displayList is object)
            {
                displayList.Invalidate();
                displayList.EnsureUpdated();
            }
        }
        #endregion

        #region Rendering
        public override void Render(ETopology Topology)
        {
            var primitive = Common.Bridge.Translate(Topology);
            GL.Begin(primitive);
            GL.CallList(displayList.Handle);
            GL.End();
        }
        #endregion
    }
}
