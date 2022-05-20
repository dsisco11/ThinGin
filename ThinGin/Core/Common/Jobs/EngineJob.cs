using System;
using System.Threading;
using System.Threading.Tasks;

using ThinGin.Core.Common.Interfaces;

namespace ThinGin.Core.Common.Engine
{
    /// <summary>
    /// Implements an abstract task being run by the engine which involves eventually yielding back to the main thread.
    /// The job system is a kind of work-then-confirm architecture for performing work around graphics APIs which the GPU will then need to confirm or be told to react to.
    /// However; graphics APIs are thread dependent and so the command for their reaction must be timed correctly by the engine, 
    /// so the job system allows a task to complete and wait in limbo for the engine to react to it such that other threads may then also react to that by performing additional work.
    /// <note>For example; transferring data into GPU memory.</note>
    /// </summary>
    public abstract class EngineJob
    {
        #region Values
        private Task _task;
        protected WeakReference<IEngine> _engineRef;
        #endregion

        #region Properties

        public readonly CancellationTokenSource TokenSource;
        #endregion

        #region Accessors
        public IEngine Engine => _engineRef.TryGetTarget(out IEngine outEngine) ? outEngine : null;
        #endregion

        #region Events
        public event Action Complete;
        #endregion

        #region Constructors
        public EngineJob(IEngine Engine)
        {
            TokenSource = new CancellationTokenSource();
            _engineRef = new WeakReference<IEngine>(Engine);
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Performs the actual work that this specific job is meant to do...
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Confirms the job executed correctly, eg; by executing a GPU synchronization command such as a fence sync for memory transfers.
        /// </summary>
        public abstract bool TryConfirm();
        #endregion

        #region State Management
        public void Start()
        {
            _task = Task.Run(Run, TokenSource.Token);
            _task.ContinueWith((Task t) => 
            {
                Engine.FinishJob(this);
            });
        }

        /// <summary>
        /// Polls the job to check if its worker task has finished
        /// </summary>
        /// <returns></returns>
        public bool Poll()
        {
            return _task?.IsCompleted ?? false;
            //return _task.Status >= TaskStatus.RanToCompletion;
        }

        internal void Finish()
        {
            Complete?.Invoke();
        }

        public void Cancel()
        {
            if (TokenSource is object)
            {
                TokenSource.Cancel();
            }
        }
        #endregion
    }
}
