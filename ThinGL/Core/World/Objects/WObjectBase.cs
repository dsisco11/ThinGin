using System;

namespace ThinGin.Core.World.Objects
{
    // Base class for all world objects
    public abstract class WObjectBase : IDisposable
    {

        #region Values
        private int disposedValue;
        #endregion


        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (System.Threading.Interlocked.Exchange(ref disposedValue, 1) == 0)
            {
                if (disposing)
                {
                }
            }
        }

        ~WObjectBase()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
