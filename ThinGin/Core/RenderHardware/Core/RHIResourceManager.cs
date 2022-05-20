using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ThinGin.Core.RenderHardware.Resources
{
    public class RHIResourceManager : IEnumerable<RHIResource>
    {
        #region Values
        private List<RHIResource> items = new List<RHIResource>();

        protected ConcurrentQueue<RHIResource> update_queue = new ConcurrentQueue<RHIResource>();
        protected ConcurrentQueue<RHIResource> release_queue = new ConcurrentQueue<RHIResource>();
        protected ConcurrentQueue<RHIResource> initialization_queue = new ConcurrentQueue<RHIResource>();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public RHIResourceManager()
        {
        }
        #endregion


        #region Registration
        public bool Register(RHIResource obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (items.Contains(obj))
            {
                return false;
            }

            items.Add(obj);
            LazyInit(obj);
            return true;
        }

        public bool Unregister(RHIResource obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (!items.Contains(obj))
            {
                return false;
            }

            items.Remove(obj);
            LazyRelease(obj);
            return true;
        }
        #endregion

        #region Object Management
        /// <summary>
        /// Queues the specified object for lazy initialization by the engine, meaning that its initialization logic will be triggered when the engine next enters its processing loop.
        /// </summary>
        /// <param name="obj"></param>
        public void LazyInit(RHIResource obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            initialization_queue.Enqueue(obj);
        }

        /// <summary>
        /// Queues the specified object for release on the engine side.
        /// </summary>
        /// <param name="obj"></param>
        public void LazyRelease(RHIResource obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            release_queue.Enqueue(obj);
        }

        /// <summary>
        /// Queues the specified resource object for a delayed update by the engine.
        /// </summary>
        /// <param name="obj"></param>
        public void LazyUpdate(RHIResource obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            update_queue.Enqueue(obj);
        }
        #endregion

        #region IEnumerable
        public IEnumerator<RHIResource> GetEnumerator()
        {
            return ((IEnumerable<RHIResource>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
        #endregion

        #region Processing
        /// <summary>
        /// Performs queue processing
        /// </summary>
        public void Process()
        {
            while (initialization_queue.TryDequeue(out RHIResource obj))
            {
                obj.TryInitialize();
            }

            while (update_queue.TryDequeue(out RHIResource obj))
            {
                obj.TryUpdate();
            }

            while (release_queue.TryDequeue(out RHIResource obj))
            {
                obj.TryRelease();
            }
        }
        #endregion
    }
}
