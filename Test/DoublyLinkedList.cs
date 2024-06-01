using ClassLibrary;
using System;

using System.Collections;

using System.Collections.Generic;



namespace lab

{

    public class DoublyLinkedList<T> : IList<T> where T : Plant, new()

    {

        private class Node

        {

            public T Data;

            public Node Next;

            public Node Prev;



            public Node(T data)

            {

                Data = data;

                Next = null;

                Prev = null;

            }

        }



        private Node head;

        private Node tail;

        private int count;



        public DoublyLinkedList()

        {

            head = null;

            tail = null;

            count = 0;

        }



        public int Count => count;



        public bool IsReadOnly => false;



        public T this[int index]

        {

            get

            {

                return GetNodeAt(index).Data;

            }

            set

            {

                GetNodeAt(index).Data = value;

            }

        }



        private Node GetNodeAt(int index)

        {

            if (index < 0 || index >= count)

                throw new ArgumentOutOfRangeException();



            Node current = head;

            for (int i = 0; i < index; i++)

            {

                current = current.Next;

            }



            return current;

        }



        public void Add(T item)

        {

            Node newNode = new Node(item);

            if (head == null)

            {

                head = newNode;

                tail = newNode;

            }

            else

            {

                tail.Next = newNode;

                newNode.Prev = tail;

                tail = newNode;

            }

            count++;

        }



        public void Clear()

        {

            head = null;

            tail = null;

            count = 0;

        }



        public bool Contains(T item)

        {

            Node current = head;

            while (current != null)

            {

                if (current.Data.Equals(item))

                    return true;

                current = current.Next;

            }

            return false;

        }



        public void CopyTo(T[] array, int arrayIndex)

        {

            Node current = head;

            while (current != null)

            {

                array[arrayIndex++] = current.Data;

                current = current.Next;

            }

        }



        public IEnumerator<T> GetEnumerator()

        {

            Node current = head;

            while (current != null)

            {

                yield return current.Data;

                current = current.Next;

            }

        }



        IEnumerator IEnumerable.GetEnumerator()

        {

            return GetEnumerator();

        }



        public int IndexOf(T item)

        {

            Node current = head;

            int index = 0;

            while (current != null)

            {

                if (current.Data.Equals(item))

                    return index;

                current = current.Next;

                index++;

            }

            return -1;

        }



        public void Insert(int index, T item)

        {

            if (index < 0 || index > count)

                throw new ArgumentOutOfRangeException();



            Node newNode = new Node(item);

            if (index == 0)

            {

                newNode.Next = head;

                if (head != null)

                    head.Prev = newNode;

                head = newNode;

                if (tail == null)

                    tail = head;

            }

            else if (index == count)

            {

                tail.Next = newNode;

                newNode.Prev = tail;

                tail = newNode;

            }

            else

            {

                Node current = GetNodeAt(index);

                newNode.Next = current;

                newNode.Prev = current.Prev;

                current.Prev.Next = newNode;

                current.Prev = newNode;

            }

            count++;

        }



        public bool Remove(T item)

        {

            Node current = head;

            while (current != null)

            {

                if (current.Data.Equals(item))

                {

                    if (current.Prev != null)

                        current.Prev.Next = current.Next;

                    else

                        head = current.Next;



                    if (current.Next != null)

                        current.Next.Prev = current.Prev;

                    else

                        tail = current.Prev;



                    count--;

                    return true;

                }

                current = current.Next;

            }

            return false;

        }



        public void RemoveAt(int index)

        {

            if (index < 0 || index >= count)

                throw new ArgumentOutOfRangeException();



            Node current = GetNodeAt(index);

            if (current.Prev != null)

                current.Prev.Next = current.Next;

            else

                head = current.Next;



            if (current.Next != null)

                current.Next.Prev = current.Prev;

            else

                tail = current.Prev;



            count++;

        }

    }

}