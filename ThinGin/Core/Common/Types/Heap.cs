using System;

namespace ThinGin.Core.Common.Types
{
    public class Heap<T> where T : IHeapItem<T>
    {
        #region Values
        T[] items;
        int ItemCount;
        #endregion

        #region Accessors
        public int Count => ItemCount;
        #endregion

        #region Constructors
        public Heap(int maxSize)
        {
            items = new T[maxSize];
        }
        #endregion

        public void UpdateItem(T item)
        {
            SortUp(item);
            SortDown(item);
        }

        public void Add(T item)
        {
            item.Index = ItemCount;
            items[ItemCount] = item;
            SortUp(item);
            ItemCount++;
        }

        public T RemoveFirst()
        {
            var first = items[0];
            ItemCount--;
            items[0] = items[ItemCount];
            items[0].Index = 0;
            SortDown(items[0]);

            return items[0];
        }

        public bool Contains(T item)
        {
            return Equals(items[item.Index], item);
        }

        void SortDown(T item)
        {
            while (true)
            {
                int idxL = (item.Index >> 1) + 1;
                int idxR = (item.Index >> 1) + 2;
                int swapIndex = 0;

                if (idxL < ItemCount)
                {
                    swapIndex = idxL;

                    if (idxR < ItemCount)
                    {
                        if (items[idxL].CompareTo(items[idxR]) < 0)
                        {
                            swapIndex = idxR;
                        }
                    }

                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        void SortUp(T item)
        {
            int parentIndex = (item.Index << 1) - 1;
            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }

                parentIndex = (item.Index << 1) - 1;
            }
        }

        void Swap(T left, T right)
        {
            items[left.Index] = right;
            items[right.Index] = left;

            var idxL = left.Index;
            var idxR = right.Index;

            left.Index = idxR;
            right.Index = idxL;
        }

    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int Index { get; set; }
    }
}
