using System;

using System.Collections;

using System.Collections.Generic;



namespace lab

{

    public class HashTable<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>

    {

        private int capacity;
        private int count;
        private KeyValuePair<TKey, TValue>?[] array;

        public int Capacity { get { return capacity; } }
        public int Count { get { return count; } }
        public bool IsReadOnly => false;



        public HashTable()
        {
            this.capacity = 0;
            this.count = 0;
            this.array = new KeyValuePair<TKey, TValue>?[0];
        }

        public HashTable(int capacity)
        {
            this.capacity = capacity;
            this.count = 0;
            this.array = new KeyValuePair<TKey, TValue>?[capacity];
        }

        public void Add(TKey key, TValue value)
        {
            if (capacity == 0)
            {
                throw new InvalidOperationException("The hash table has zero capacity.");
            }

            int hash = Math.Abs(key.GetHashCode()) % capacity;
            int start = hash;

            do
            {
                if (array[hash] == null)
                {
                    array[hash] = new KeyValuePair<TKey, TValue>(key, value);
                    count++;
                    return;
                }

                hash = (hash + 1) % capacity; // продвижение по кругу 
            } while (hash != start);

            throw new InvalidOperationException("The hash table is full.");
        }

        public bool ContainsKey(TKey key)
        {
            return FindIndex(key) >= 0;
        }

        public bool Remove(TKey key)
        {
            int position = FindIndex(key);
            if (position == -1)
            {
                return false;
            }
            array[position] = null;
            count--;
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int position = FindIndex(key);
            if (position >= 0 && array[position].HasValue)
            {
                value = array[position].Value.Value;
                return true;
            }

            value = default;
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                int position = FindIndex(key);
                if (position >= 0 && array[position].HasValue)
                {
                    return array[position].Value.Value;
                }
                throw new KeyNotFoundException("The key was not found in the hash table.");
            }

            set
            {
                int position = FindIndex(key);
                if (position >= 0 && array[position].HasValue)
                {
                    array[position] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keys = new List<TKey>();
                foreach (var pair in array)
                {
                    if (pair.HasValue)
                    {
                        keys.Add(pair.Value.Key);
                    }
                }
                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>();
                foreach (var pair in array)
                {
                    if (pair.HasValue)
                    {
                        values.Add(pair.Value.Value);
                    }
                }
                return values;
            }
        }

        private int FindIndex(TKey key)
        {
            if (capacity == 0)
            {
                return -2;
            }
            int hash = Math.Abs(key.GetHashCode()) % capacity;
            int start = hash;
            do
            {
                if (array[hash].HasValue && array[hash].Value.Key.Equals(key))
                {
                    return hash;
                }
                hash = (hash + 1) % capacity;
            } while (hash != start);
            return -1; // Такого элемента нет
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var pair in array)
            {
                if (pair.HasValue)
                {
                    yield return pair.Value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            array = new KeyValuePair<TKey, TValue>?[capacity];
            count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int position = FindIndex(item.Key);
            if (position >= 0 && array[position].HasValue)
            {
                return EqualityComparer<TValue>.Default.Equals(array[position].Value.Value, item.Value);
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var pair in this.array)
            {
                if (pair.HasValue)
                {
                    array[arrayIndex++] = pair.Value;
                }
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int position = FindIndex(item.Key);
            if (position >= 0 && array[position].HasValue && EqualityComparer<TValue>.Default.Equals(array[position].Value.Value, item.Value))
            {
                array[position] = null;
                count--;
                return true;
            }
            return false;
        }
    }
}