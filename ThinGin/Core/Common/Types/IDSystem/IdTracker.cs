using System.Collections.Generic;

namespace ThinGin.Core.Common.Types.IDSystem
{
    /// <summary>
    /// Manages allocating and tracking a set of unique reusable id numbers
    /// </summary>
    public class IdTracker
    {
        #region Values
        private object lockObject = new object();
        private SortedSet<id_interval> free;
        #endregion

        #region Constructors
        public IdTracker()
        {
            free = new SortedSet<id_interval>();
            free.Add(new id_interval(1, int.MaxValue));
        }
        #endregion

        public bool IsFree(int id)
        {
            var it = free.GetEnumerator();
            while (it.Current is object)
            {
                if (it.Current.lower > id)// we passed it, so the id does not exist within a free range (it is taken)
                    return false;

                if (it.Current.Contains(id))
                    return true;
            }

            return false;
        }

        public int Allocate()
        {
            int free_id;
            lock (lockObject)
            {
                var first = free.Min;
                free_id = first.lower;
                // remove this immutable interval from our set of free ids so we can adjust it
                free.Remove(first);
                // increase the intervals lower bound by 1, then if the interval is still not empty (size of zero) then add it back to our set
                if (first.lower + 1 <= first.upper)
                {
                    free.Add(new id_interval(first.lower+1, first.upper));
                }
            }

            return free_id;
        }

        public bool MarkAsUsed(int id)
        {
            lock (lockObject)
            {
                var _interval = find_interval(id);
                if (_interval is null) return false;

                // 'snip' this id out of the interval
                free.Remove(_interval);
                if (_interval.lower < id)
                {
                    free.Add(new id_interval(_interval.lower, id - 1));
                }

                if (id + 1 <= _interval.upper)
                {
                    free.Add(new id_interval(id + 1, _interval.upper));
                }

                return true;
            }
        }

        public void Free(int id)
        {
            lock (lockObject)
            {
                var _interval = find_interval(id);
                if (_interval is object && _interval.lower <= id && _interval.upper > id)
                    return;// id is already free

                _interval = upper_bound(id);
                if (_interval is null)// no interval comes after this id
                {
                    free.Add(new id_interval(id, id));
                    return;
                }

                // find if this id lies right on the edge of an interval, if so we can expand said interval by 1 to encompass it,
                // otherwise we have to add a new interval just for this id!
                var merge = find_merge_candidates(id);
                if (merge.left is null && merge.right is null)
                {// cannot merge the id, create a new interval for it instead
                    free.Add(new id_interval(id, id));
                }
                else if (merge.left is object && merge.right is object)
                {// we have two intervals directly bordering the id, merge them into a single one
                    free.Remove(merge.left);
                    free.Remove(merge.right);

                    free.Add(new id_interval(merge.left.lower, merge.right.upper));
                }
                else
                {// okay we only have a single bordering interval
                    if (merge.right is object)
                    {
                        free.Remove(merge.right);
                        free.Add(new id_interval(id, merge.right.upper));
                    }
                    else if (merge.left is object)
                    {
                        free.Remove(merge.left);
                        free.Add(new id_interval(merge.left.lower, id));
                    }
                }
            }
        }

        #region Utility
        private id_interval find_interval(int id)
        {
            var it = free.GetEnumerator();
            while (it.Current is object)
            {
                if (it.Current.Contains(id))
                    return it.Current;
            }

            return null;
        }

        /// <summary>
        /// Returns the first interval that has a lower or upper bound with a distance of 1 from the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private (id_interval left, id_interval right) find_merge_candidates(int id)
        {
            var idl = id - 1;
            var idu = id + 1;

            id_interval left = null;
            id_interval right = null;

            var it = free.GetEnumerator();
            while (it.Current is object && left is null && right is null)
            {
                //if (it.Current.lower == idl || it.Current.upper == idu)
                //    return it.Current;

                if (it.Current.lower == idl)
                    left = it.Current;

                if (it.Current.upper == idu)
                    right = it.Current;
            }

            return (left, right);
        }


        /// <summary>
        /// Finds the first interval which would go BEFORE the given id
        /// </summary>
        private id_interval lower_bound(int id)
        {
            var it = free.GetEnumerator();
            while (it.Current is object)
            {
                if (it.Current.upper < id)
                    return it.Current;
            }

            return null;
        }

        /// <summary>
        /// Finds the first interval which would go AFTER the given id
        /// </summary>
        private id_interval upper_bound(int id)
        {
            var it = free.GetEnumerator();
            while (it.Current is object)
            {
                if (it.Current.lower > id)
                    return it.Current;
            }

            return null;
        }
        #endregion
    }
}
