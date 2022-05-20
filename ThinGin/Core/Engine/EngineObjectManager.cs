using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using ThinGin.Core.Common.Objects;
using ThinGin.Core.Engine.Common.Core;
using ThinGin.Core.World;

namespace ThinGin.Core.Engine
{
    public class EngineObjectManager : IEnumerable<EngineObject>
    {
        #region Values
        private IdMapper<EngineObject> items;

        protected ConcurrentQueue<EngineObjectBase> update_queue = new ConcurrentQueue<EngineObjectBase>();
        protected ConcurrentQueue<EngineObjectBase> release_queue = new ConcurrentQueue<EngineObjectBase>();
        protected ConcurrentQueue<EngineObjectBase> initialization_queue = new ConcurrentQueue<EngineObjectBase>();

        private readonly WeakReference<EngineInstance> _engineRef = null;
        #endregion

        #region Accessors
        public EngineInstance Engine => _engineRef.TryGetTarget(out var outRef) ? outRef : null;
        #endregion

        #region Constructors
        public EngineObjectManager(EngineInstance engine)
        {
            _engineRef = new WeakReference<EngineInstance>(engine);
        }
        #endregion

        #region Registration
        public ObjectID Register(EngineObject obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            initialization_queue.Enqueue(obj);
            var idNum = items.Register(obj);

            obj.Registered();
            return new ObjectID(idNum, obj);
        }
        #endregion

        #region Unregistering
        public bool Unregister(ObjectID id)
        {
            if (id is null)
            {
                return false;
            }

            if (!items.Contains(id.Value))
            {
                return false;
            }

            items.Unregister(id.Value);
            release_queue.Enqueue(id.Object);

            id.Object.Unregistered();
            return true;
        }

        public bool Unregister(EngineObject obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (!items.Contains(obj.Id.Value))
            {
                return false;
            }

            items.Unregister(obj.Id.Value);
            release_queue.Enqueue(obj);
            obj.Unregistered();
            return true;
        }
        #endregion

        #region Object Management
        public void DeferUpdate(EngineObject obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            update_queue.Enqueue(obj);
        }

        public void DeferUpdate(EngineObjectBase obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            update_queue.Enqueue(obj);
        }
        #endregion

        #region IEnumerable
        public IEnumerator<EngineObject> GetEnumerator()
        {
            return ((IEnumerable<EngineObject>)items).GetEnumerator();
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
            while (initialization_queue.TryDequeue(out var obj))
            {
                obj.TryInitialize();
            }

            while (update_queue.TryDequeue(out var obj))
            {
                obj.TryUpdate();
            }

            while (release_queue.TryDequeue(out var obj))
            {
                obj.TryRelease();
            }
        }
        #endregion
    }
}
