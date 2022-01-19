using System;

using ThinGin.Core.Common.Interfaces;
using ThinGin.Core.Common.Engine.Interfaces;
using ThinGin.Core.Common.Engine.Delegates;
using OpenTK.Graphics.OpenGL;
using ThinGin.Core.Common.Engine.Types;

namespace ThinGin.OpenGL.Common.Types
{
    public sealed class GLSyncPrimitive : GpuSyncPrimitive
    {
        #region Values
        private IntPtr sync = IntPtr.Zero;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public GLSyncPrimitive(IEngine Engine) : base(Engine)
        {
        }
        #endregion

        public override void Begin()
        {
            if (_success)
            {
                throw new Exception($"Hardware synchronization primitives cannot be reused!");
            }

            sync = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);
        }

        protected override bool Poll()
        {
            if (sync != IntPtr.Zero)
            {
                var res = GL.ClientWaitSync(sync, ClientWaitSyncFlags.SyncFlushCommandsBit, 0);
                if (res == WaitSyncStatus.AlreadySignaled || res == WaitSyncStatus.ConditionSatisfied)
                {
                    GL.DeleteSync(sync);
                    sync = IntPtr.Zero;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #region IEngineResource
        public override IEngineDelegate Get_Initializer() => null;
        // NOTE: We dont use an initializer because we don't want the engine to create the fence sync at the wrong time...
        //{
        //    return new EngineDelegate(() =>
        //    {
        //        sync = GL.FenceSync(SyncCondition.SyncGpuCommandsComplete, WaitSyncFlags.None);
        //    });
        //}


        public override IEngineDelegate Get_Releaser()
        {
            if (sync == IntPtr.Zero)
            {
                return null;
            }

            unsafe
            {
                return new EngineReleaser<IntPtr>(new IntPtr(sync.ToPointer()), GL.DeleteSync);
            }
        }
        #endregion
    }
}
