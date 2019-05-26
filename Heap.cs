using System;
using System.Text;
using System.Collections.Generic;

namespace DataStructures
{
    public delegate bool PriorityOrder<T>(T t1, T t2);

    public class Heap<T>
    {
        private List<T> items;
        private PriorityOrder<T> isGreaterThan;

        public Heap(PriorityOrder<T> P)
        {
            items = new List<T>();
            isGreaterThan = P;
        }

        public Heap(int n, PriorityOrder<T> P)
        {
            items = new List<T>(n);
            isGreaterThan = P;
        }

        public void Add(T item)
        {
            items.Add(item);
            bubbleUp(items.Count - 1);
        }

        public T GetNext()
        {
            if (IsEmpty())
                throw new Exception(
                        "Attempted to remove item from an empty heap!");
            T tmp = items[0];
            swap(0, items.Count - 1);
            items.RemoveAt(items.Count - 1);
            if (!IsEmpty())
                siftDown(0);
            return tmp;
        }

        public bool IsEmpty()
        {
            return items.Count < 1;
        }

        public override string ToString()
        {
            if (IsEmpty())
                return "Empty Heap";
            StringBuilder sb = new StringBuilder(2*items.Count - 1);
            for (int i=0;i<items.Count - 1;i++)
            {
                sb.Append(items[i] + " ");
            }
            sb.Append(items[items.Count - 1]);
            return sb.ToString();
        }

        private void swap(int i, int j)
        {
            T tmp = items[i];
            items[i] = items[j];
            items[j] = tmp;
        }

        private void bubbleUp(int child)
        {
            if (child < 1 || child >= items.Count)
                return;
            int parent = (child - 1)/2;
            if (isGreaterThan(items[child], items[parent]))
            {
                swap(parent, child);
                bubbleUp(parent);
            }
        }

        private void siftDown(int parent)
        {
            if (parent < 0)
                return;
            int child = 2*parent + 1;
            if (child < items.Count)
            {
                if (child + 1 < items.Count &&
                        isGreaterThan(items[child + 1], items[child]))
                    child += 1;
                if (isGreaterThan(items[child], items[parent]))
                {
                    swap(parent, child);
                    siftDown(child);
                }
            }
        }
    }    
}
