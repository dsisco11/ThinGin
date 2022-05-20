using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using ThinGin.Core.Common.Engine.Types;

namespace ThinGin.Core.Graphics
{
    public class GraphicsObjectManager : IEnumerable<GObject>
    {
        #region Values
        private List<GObject> items = new List<GObject>();

        protected ConcurrentQueue<GObject> update_queue = new ConcurrentQueue<GObject>();
        protected ConcurrentQueue<GObject> release_queue = new ConcurrentQueue<GObject>();
        protected ConcurrentQueue<GObject> initialization_queue = new ConcurrentQueue<GObject>();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public GraphicsObjectManager()
        {
        }
        #endregion


        #region Registration
        public bool Register(GObject obj)
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

        public bool Unregister(GObject obj)
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
        public void LazyInit(GObject obj)
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
        public void LazyRelease(GObject obj)
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
        public void LazyUpdate(GObject obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            update_queue.Enqueue(obj);
        }

        private bool _try_process_object_queue(ref ConcurrentQueue<GObject> queue, Action<GObject> Callback)
        {
            while (queue.TryDequeue(out var obj))
            {
                Callback.Invoke(obj);
            }

            return true;
        }
        #endregion

        #region IEnumerable
        public IEnumerator<GObject> GetEnumerator()
        {
            return ((IEnumerable<GObject>)items).GetEnumerator();
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
            while (initialization_queue.TryDequeue(out GObject obj))
            {
                obj.TryInitialize();
            }

            while (update_queue.TryDequeue(out GObject obj))
            {
                obj.TryUpdate();
            }

            while (release_queue.TryDequeue(out GObject obj))
            {
                obj.TryRelease();
            }
        }
        #endregion
    }
}
