using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTF.SerializedDictionary
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue> , ISerializationCallbackReceiver
    {
        [SerializeField] private List<Pair> _entries = new();

        public SerializedDictionary() { }

        public SerializedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        public void OnBeforeSerialize()
        {
            _entries.Clear();

            foreach (var pair in this)
                _entries.Add(pair);
        }

        public void OnAfterDeserialize()
        {
            Clear();

            foreach (var entry in _entries)
            {
                var key = entry.Key;
                if (ContainsKey(key))
                    key = key is string ? (TKey)(object)"" : default;

                this[key] = entry.Value;
            }
        }

        [Serializable]
        public struct Pair
        {
            public TKey Key;
            public TValue Value;

            public Pair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public readonly void Deconstruct(out TKey key, out TValue value)
            {
                key = Key;
                value = Value;
            }

            public static implicit operator KeyValuePair<TKey, TValue>(Pair Pair)
                => new(Pair.Key, Pair.Value);

            public static implicit operator Pair(KeyValuePair<TKey, TValue> pair)
                => new(pair.Key, pair.Value);
        }
    }
}
