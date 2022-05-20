using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using ThinGin.Core.Common.Types.IDSystem;

namespace ThinGin.Core.World
{
    /// <summary>
    /// Manages a unique set of integer ids associated with objects, facilitates reuse of id numbers and object lookup via id number
    /// </summary>
    public class IdMapper<T>
    {
        #region Values
        private IdTracker tracker = new IdTracker();
        private ConcurrentDictionary<int, T> map = new ConcurrentDictionary<int, T>();
        #endregion

        #region Properties
        #endregion

        #region Accessors
        #endregion

        #region Constructors
        public IdMapper()
        {
        }
        #endregion

        #region Lookup
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int id)
        {
            return !tracker.IsFree(id);
        }

        public T Lookup(int id)
        {
            if (map.TryGetValue(id, out T outValue))
            {
                return outValue;
            }

            return default;
        }
        #endregion

        #region Registration
        public int Register(T obj)
        {
            var id = tracker.Allocate();
            map.TryAdd(id, obj);

            return id;
        }

        public void Unregister(int id)
        {
            tracker.Free(id);
            map.TryRemove(id, out _);
        }
        #endregion

    }
}
