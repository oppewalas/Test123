using ClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lab
{
    public enum Color
    {
        Red,
        Black
    }
    public class RedBlackTree<T> : IEnumerable<T>, ICollection<T> where T : class, IComparable<T>, IInit, ICloneable, new()
    {
        protected Color color;
        protected RedBlackTree<T> left;
        protected RedBlackTree<T> right;
        protected RedBlackTree<T> parent;
        protected T data;

        public Color Color { get { return color; } }
        public RedBlackTree<T> Left { get { return left; } set { left = value; } }
        public RedBlackTree<T> Right { get { return right; } set { right = value; } }
        public RedBlackTree<T> Parent { get { return parent; } set { parent = value; } }
        public T Value { get { return data; } }

        public RedBlackTree()
        {
            color = Color.Red;
        }

        public RedBlackTree(Color color, RedBlackTree<T> left, RedBlackTree<T> right, RedBlackTree<T> parent, T data)
        {
            this.color = color;
            this.left = left;
            this.right = right;
            this.parent = parent;
            this.data = data;
        }

        public RedBlackTree(T data, RedBlackTree<T> parent)
        {
            this.data = data;
            if (parent == null)
                this.color = Color.Black;
            else
                this.color = Color.Red;
            this.parent = parent;
        }

        private void Balance()
        {


            if (right != null &&
                right.color == Color.Red &&
                (left == null || left.color == Color.Black))
            {
                this.RotateLeft();
            }
            if ((right == null || right.color == Color.Black) &&
                left != null &&
                left.color == Color.Red &&
                left.left != null &&
                left.left.color == Color.Red)
            {
                this.RotateRight();
            }
            if (left != null &&
                left.color == Color.Red &&
                right != null &&
                right.color == Color.Red)
            {
                this.SwapColor();
            }
            //if ((left == null || left.color == Color.Black) &&
            //    right != null &&
            //    right.color == Color.Red)
            //{
            //    this.RotateLeft();
            //}
        }
        private void RotateLeft()
        {
            RedBlackTree<T> newNode = new RedBlackTree<T>(Color.Red, this.left, this.right.left, this, (T)this.data);
            this.left = newNode;
            this.right.color = this.color;
            this.data = this.right.data;
            this.right = this.right.right;
        }

        private void RotateRight()
        {
            RedBlackTree<T> newNode = new RedBlackTree<T>(Color.Red, this.left.right, this.right, this, (T)this.data);
            this.right = newNode;
            this.left.color = this.color;
            this.data = this.left.data;
            this.left = this.left.left;
        }

        private void SwapColor()
        {
            this.left.color = Color.Black;
            this.right.color = Color.Black;
            if (this.parent != null)
                this.color = Color.Red;
        }

        public override string ToString()
        {
            string toReturn = "";
            if (left != null)
                toReturn += left.ToString();
            if (right != null)
                toReturn += right.ToString();
            return toReturn + this.data.ToString() + ' ';
        }

        public void Delete(T data)
        {
            //спуск
            RedBlackTree<T> current = this;
            while (current.data != null && data.CompareTo((T)current.data) != 0)
            {
                if (data.CompareTo((T)current.data) < 0)
                    current = current.left;
                else
                    current = current.right;
            }
            if (data.CompareTo((T)current.data) == 0)
            {
                //К2 и Ч2
                if (current.left != null && current.right != null)
                {
                    RedBlackTree<T> temp = current.left;
                    //Ищем соседний по значению элемент (я выбрал тот что меньше) и меняем их значения местами, а после удаляем
                    while (temp.right != null)
                    {
                        temp = temp.right;
                    }
                    current.data = temp.data;
                    temp.data = data;
                    if (temp.color == Color.Red)
                        temp = null;
                    else if (temp.left == null && temp.right == null)
                        temp.DeleteBlack0();
                    else
                        temp.DeleteBlack1();
                }

                //К1 не существует

                //Ч1
                else if (current.color == Color.Black && (current.left != null || current.right != null))
                {
                    current.DeleteBlack1();
                }

                //К0
                else if (current.color == Color.Red)
                {
                    current.ClearNode();
                }

                //Ч0
                else
                {
                    current.DeleteBlack0();
                }
            }
        }

        private void DeleteBlack1()
        {
            if (this.left != null)
            {
                this.data = this.left.data;
                this.left = null;
            }
            else
            {
                this.data = this.right.data;
                this.right = null;
            }
        }

        private void DeleteBlack0() //п*зд*, з*луп* и *чк*
        {
            var parent = this.parent;
            this.ClearNode();
            if (parent != null)
            {
                if (parent.left == this)
                {
                    //КЧ1 Красный родитель, правый брат черный с черными детьми (черные nil-узлы)
                    if (parent.color == Color.Red &&
                        parent.right.color == Color.Black &&
                        (parent.right.right == null || parent.right.right.color == Color.Black) &&
                        (parent.right.left == null || parent.right.left.color == Color.Black))
                    {
                        parent.color = Color.Black;
                        parent.right.color = Color.Red;
                    }
                    //КЧ2 Родитель красный, правый брат черный с красным левым ребенком
                    else if (parent.color == Color.Red &&
                        parent.right.color == Color.Black &&
                        parent.right.right != null &&
                        parent.right.right.color == Color.Red)
                    {
                        parent.RotateLeft();
                    }
                    //ЧК3 Родитель чёрный, правый сын красный, у правого внука чёрные правнуки
                    else if (parent.color == Color.Black &&
                        parent.right.color == Color.Red &&
                        parent.right.left != null &&
                        (parent.right.left.right == null || parent.right.left.right.color == Color.Black) &&
                        (parent.right.left.left == null || parent.right.left.left.color == Color.Black))
                    {
                        parent.right.left.color = Color.Red;
                        parent.RotateLeft();
                    }
                    //ЧК4 Родитель чёрный, правый сын красный, у правого внука правый правнук красный
                    else if (parent.color == Color.Black &&
                        parent.right.color == Color.Red &&
                        parent.right.left != null &&
                        parent.right.left.right != null &&
                        parent.right.left.right.color == Color.Red)
                    {
                        parent.right.RotateRight();
                        parent.RotateLeft();
                        parent.right.left.color = Color.Red;
                    }
                    //ЧЧ5 Родитель чёрный, правый сын чёрный с правым красным внуком //НЕВОЗМОЖНО
                    else if (parent.color == Color.Black &&
                        parent.right.color == Color.Black &&
                        parent.right.left != null &&
                        parent.right.left.color == Color.Red)
                    {
                        parent.right.left.color = Color.Black;
                        parent.right.RotateRight();
                        parent.RotateLeft();
                        parent.Balance();
                    }
                    //ЧЧ6 Родитель чёрный, правый сын чёрный, его внуки тоже чёрные, в случае левостороннего дерева это возможно только если в дереве всего 3 ноды и удаляем не корень
                    else
                    {
                        parent.right.color = Color.Red;
                    }

                }
                else
                {
                    //КЧ1 Красный родитель, левый брат черный с черными детьми (черные nil-узлы)
                    if (parent.color == Color.Red &&
                        parent.left.color == Color.Black &&
                        (parent.left.left == null || parent.left.left.color == Color.Black) &&
                        (parent.left.right == null || parent.left.right.color == Color.Black))
                    {
                        parent.color = Color.Black;
                        parent.left.color = Color.Red;
                    }
                    //КЧ2 Родитель красный, левый брат черный с красным левым ребенком
                    else if (parent.color == Color.Red &&
                        parent.left.color == Color.Black &&
                        parent.left.left != null &&
                        parent.left.left.color == Color.Red)
                    {
                        parent.RotateRight();
                    }
                    //ЧК3 Родитель чёрный, левый сын красный, у правого внука чёрные правнуки
                    else if (parent.color == Color.Black &&
                        parent.left.color == Color.Red &&
                        parent.left.right != null &&
                        (parent.left.right.left == null || parent.left.right.left.color == Color.Black) &&
                        (parent.left.right.right == null || parent.left.right.right.color == Color.Black))
                    {
                        parent.left.right.color = Color.Red;
                        parent.RotateRight();
                    }
                    //ЧК4 Родитель чёрный, левый сын красный, у правого внука левый правнук красный
                    else if (parent.color == Color.Black &&
                        parent.left.color == Color.Red &&
                        parent.left.right != null &&
                        parent.left.right.left != null &&
                        parent.left.right.left.color == Color.Red)
                    {
                        parent.left.RotateLeft();
                        parent.RotateRight();
                        parent.left.right.color = Color.Black;
                    }
                    //ЧЧ5 Родитель чёрный, левый сын чёрный с правым красным внуком //НЕВОЗМОЖНО
                    else if (parent.color == Color.Black &&
                        parent.left.color == Color.Black &&
                        parent.left.right != null &&
                        parent.left.right.color == Color.Red)
                    {
                        parent.left.right.color = Color.Black;
                        parent.left.RotateLeft();
                        parent.RotateRight();
                        parent.Balance();
                    }
                    //ЧЧ6 Родитель чёрный, левый сын чёрный, его внуки тоже чёрные, в случае левостороннего дерева это возможно только если в дереве всего 3 ноды и удаляем не корень
                    else
                    {
                        parent.left.color = Color.Red;
                    }
                }
            }
        }

        public void Add(T data)
        {
            // Спуск к месту вставки
            RedBlackTree<T> current = this;
            RedBlackTree<T> parent = null;

            while (current != null && current.data != null)
            {
                parent = current;

                if (data.CompareTo((T)current.data) == 0)
                    return;

                if (data.CompareTo((T)current.data) < 0)
                    current = current.left;
                else
                    current = current.right;
            }

            // Создаем новый узел
            RedBlackTree<T> newNode = new RedBlackTree<T>(data, parent);

            // Определяем позицию вставки нового узла
            if (parent == null)
            {
                this.data = data; // Если это корень дерева
                this.color = Color.Black;
            }
            else if (data.CompareTo((T)parent.data) < 0)
            {
                parent.left = newNode; // Вставка слева
            }
            else
            {
                parent.right = newNode; // Вставка справа
            }

            // После вставки вызываем балансировку
            if (newNode.parent != null)
            {
                newNode.parent.Balance();
                if (newNode.parent.parent != null)
                {
                    newNode.parent.parent.Balance();
                    if (newNode.parent.parent.parent != null)
                        newNode.parent.parent.parent.Balance();
                }

            }

        }

        public double? AverageNode()
        {
            if (left != null || right != null)
            {
                return SumAllNodes() / CountNodes();
            }
            else
            {
                return null;
            }
        }

        public double SumAllNodes()
        {
            double sum = 0;
            //sum = data.Height;
            if (left != null)
            {
                sum += left.CountNodes();
            }
            if (right != null)
            {
                sum += right.CountNodes();
            }
            return sum;
        }

        public int CountNodes()
        {
            int count = 1;
            if (left != null)
            {
                count += left.CountNodes();
            }
            if (right != null)
            {
                count += right.CountNodes();
            }
            return count;
        }

        public void DeleteTree()
        {
            if (left != null)
                left.DeleteTree();
            if (right != null)
                right.DeleteTree();
            this.ClearNode();
        }

        private void ClearNode()
        {
            if (this.parent != null)
            {
                if (this.parent.left == this)
                    this.parent.left = null;
                else
                    this.parent.right = null;
            }
            this.left = null;
            this.right = null;
            this.parent = null;
            this.data = null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal(this).GetEnumerator();
        }
        private IEnumerable<T> InOrderTraversal(RedBlackTree<T> node)
        {
            if (node != null)
            {
                foreach (var item in InOrderTraversal(node.left))
                    yield return item;

                yield return node.data;

                foreach (var item in InOrderTraversal(node.right))
                    yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            DeleteTree();
        }

        public bool Contains(T item)
        {
            RedBlackTree<T> current = this;
            while (current.data != null && item.CompareTo((T)current.data) != 0)
            {
                if (data.CompareTo((T)current.data) < 0)
                    current = current.left;
                else
                    current = current.right;
            }
            if (item.CompareTo((T)current.data) == 0)
                return true;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
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
                array[arrayIndex] = ((T)node.data.Clone());

                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                }

                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                }
                arrayIndex++;
            }
        }

        public bool Remove(T item)
        {
            if (Contains(item))
            {
                Delete(item);
                return true;
            }
            else
                return false;
        }

        public int Count => CountNodes();

        public bool IsReadOnly => false;
    }
}