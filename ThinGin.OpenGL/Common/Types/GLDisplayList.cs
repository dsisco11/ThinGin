
using OpenTK.Graphics.OpenGL;

using System;

using ThinGin.Core.Common.Engine.Delegates;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Types;
using ThinGin.Core.Common.Interfaces;

namespace ThinGin.OpenGL.Common.Types
{
    public class GLDisplayList : EngineObject
    {
        #region Values
        protected int _handle = 0;
        protected Action _command_list_delegate = null;
        #endregion

        #region Properties
        #endregion

        #region Accessors
        public int Handle => _handle;
        #endregion

        #region Constructors
        public GLDisplayList(IRenderEngine Engine) : base(Engine)
        {
        }
        /// <summary>
        /// Creates an engine resource which wraps around and manages a single OpenGL display list
        /// </summary>
        /// <param name="Engine"></param>
        /// <param name="CommandBuilder"></param>
        public GLDisplayList(IRenderEngine Engine, Action CommandBuilder) : base(Engine)
        {
            _command_list_delegate = CommandBuilder;
        }
        #endregion

        #region Updating
        /// <summary>
        /// Updates the display list with the OpenGL commands called from within the provided <paramref name="commandList"/> delegate method.
        /// </summary>
        /// <param name="commandList"></param>
        public void Set_Command_List(Action commandList)
        {
            if (!ReferenceEquals(commandList, _command_list_delegate))
            {
                _command_list_delegate = commandList;
                Invalidate();
            }
        }
        #endregion

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => new EngineDelegate(() => { _handle = GL.GenLists(1); });
        public override IEngineDelegate Get_Releaser() => new EngineReleaser<int>(_handle, (int id) => { GL.DeleteLists(id, 1); });
        public override IEngineDelegate Get_Updater() => new EngineDelegate(() => 
        {
            GL.NewList(_handle, ListMode.Compile);
            _command_list_delegate?.Invoke();
            GL.EndList();
        });
        #endregion

    }
}
