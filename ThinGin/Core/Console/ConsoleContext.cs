using System;

namespace ThinGin.Core.Console
{
    public struct ConsoleContext
    {
        #region Properties
        public readonly System.WeakReference<VirtualConsole> ConsoleRef;
        public VirtualConsole Console => ConsoleRef.TryGetTarget(out var outRef) ? outRef : null;
        #endregion

        #region Constructors
        public ConsoleContext(VirtualConsole Console)
        {
            ConsoleRef = new WeakReference<VirtualConsole>(Console);
        }
        #endregion
    }
}
