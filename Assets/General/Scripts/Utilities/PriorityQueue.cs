using System;
using System.Collections.Generic;

public class PriorityQueue<TElement> : IPriorityQueue<TElement>
    where TElement : IComparable<TElement>
{
    private readonly SortedSet<TElement> _set;

    public PriorityQueue()
    {
        _set = new SortedSet<TElement>();
    }

    public int Count => _set.Count;
    public TElement Min => _set.Min;
    public TElement Max => _set.Max;

    public bool Add(TElement element) => _set.Add(element);

    public TElement Peek() => _set.Max;

    public TElement Pop()
    {
        if (_set.Count < 0)
        {
            return default;
        }

        var max = _set.Max;
        _set.Remove(max);
        return max;
    }

    public void Clear() => _set.Clear();
}

public interface IPriorityQueue<TElement>
{
    int Count {get;}
    TElement Min {get;}
    TElement Max {get;}
    bool Add(TElement element);
    TElement Pop();
    TElement Peek();
    void Clear();
}