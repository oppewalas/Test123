using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab
{
    public class MyCollection<T> : RedBlackTree<T> where T : class, IComparable<T>, IInit, ICloneable, INullable, new()
    {
        // T? data;
        private Random rnd = new Random();
        public MyCollection()
        {
            data = null;
            left = null;
            right = null;
            parent = null;
            color = Color.Black;
        }

        public MyCollection(int length)
        {
            for (int i = 0; i < length; i++)
            {
                T item = new T();
                item.RandomInit();
                this.Add(item);
            }
        }

        public MyCollection(MyCollection<T> c)
        {
            if (this == null)
            {
                return;
            }

            Queue<RedBlackTree<T>> queue = new Queue<RedBlackTree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                RedBlackTree<T> node = queue.Dequeue();
                this.Add((T)node.Value.Clone());

                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                }

                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                }
            }

            MyCollection<T> current = c;
            while (current != null)
            {
                this.Add((T)current.data.Clone());
                current = (MyCollection<T>)current.left;
            }
            current = c;
            while (current != null)
            {
                this.Add((T)current.data.Clone());
                current = (MyCollection<T>)current.right;
            }
        }
    }
}
