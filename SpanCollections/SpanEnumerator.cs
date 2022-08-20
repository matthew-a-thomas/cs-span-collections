namespace SpanCollections;

using System;

public ref struct SpanEnumerator<T>
{
    readonly bool _backward;
    int? _index;
    readonly ReadOnlySpan<T> _span;

    public SpanEnumerator(
        bool backward,
        ReadOnlySpan<T> span)
    {
        _backward = backward;
        _index = null;
        _span = span;
    }

    public ref readonly T Current => ref _span[_index!.Value];

    public bool MoveNext()
    {
        if (_index is {} index)
        {
            _index = _backward
                ? index - 1
                : index + 1;
        }
        else
        {
            _index = _backward
                ? _span.Length - 1
                : 0;
        }
        return _index.Value >= 0 && _index.Value < _span.Length;
    }
}