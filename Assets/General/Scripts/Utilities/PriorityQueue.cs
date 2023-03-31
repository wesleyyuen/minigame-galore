using System;
using System.Collections.Generic;
using System.Linq;

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

    public TElement Peek() => _set.First();

    public TElement Pop()
    {
        if (_set.Count < 0) return default;

        var first = Peek();
        _set.Remove(first);
        return first;
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