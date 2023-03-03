using System.Collections.Generic;

public class CappedDictionary<TKey, TValue>// : IDictionary<TKey, TValue>
{
    private Dictionary<TKey, TValue> _dictionary;
    private int _maxCapacity;

    public CappedDictionary(int capacity)
    {
        _maxCapacity = capacity;
        _dictionary = new Dictionary<TKey, TValue>(capacity);
    }

    public CappedDictionary(int capacity, Dictionary<TKey, TValue> initialValues)
    {
        _maxCapacity = capacity;
        _dictionary = new Dictionary<TKey, TValue>(initialValues);
    }

    public void Add(TKey key, TValue value)
    {
        if (_dictionary.Count == _maxCapacity) return;

        _dictionary.Add(key, value);
    }

    public void Remove(TKey key)
    {
        _dictionary.Remove(key);
    }

    public TValue this[TKey key]
    {
        get { return _dictionary[key]; }
    }
}