namespace SpanCollections;

using System;

public ref struct SpanEnumerator<T>(
    bool backward,
    ReadOnlySpan<T> span)
{
    int? _index = null;
    readonly ReadOnlySpan<T> _span = span;

    public ref readonly T Current => ref _span[_index!.Value];

    public bool MoveNext()
    {
        if (_index is {} index)
        {
            _index = backward
                ? index - 1
                : index + 1;
        }
        else
        {
            _index = backward
                ? _span.Length - 1
                : 0;
        }
        return _index.Value >= 0 && _index.Value < _span.Length;
    }
}