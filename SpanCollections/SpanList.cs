namespace SpanCollections;

using System;

public readonly ref struct SpanList<T>
where T : unmanaged
{
    public const int MinNumBytes = sizeof(int) * NumPointers;
    const int NumPointers = 1;

    readonly SpanCollectionCore<T> _core;

    public SpanList(Span<byte> bytes)
    {
        if (bytes.Length < MinNumBytes)
            throw new ArgumentException($"This list requires at least {MinNumBytes} bytes", nameof(bytes));
        _core = new SpanCollectionCore<T>(bytes, NumPointers);
    }

    public int Capacity => Values.Length;

    public int Count => Math.Max(0, Math.Min(Capacity, FreePointer));

    ref int FreePointer => ref _core.Pointers[0];

    Span<T> Values => _core.Values;

    public ref T this[Index index] => ref Values[..FreePointer][index];

    public Span<T> this[Range range] => Values[..FreePointer][range];

    public void Add(in T value)
    {
        ref var free = ref FreePointer;
        Values[free] = value;
        free++;
    }

    public void Clear()
    {
        Values.Clear();
        ref var free = ref FreePointer;
        free = 0;
    }

    public SpanEnumerator<T> GetEnumerator() => new(false, Values[..FreePointer]);

    public void RemoveAt(int index)
    {
        ref var free = ref FreePointer;
        for (; index < free - 1; ++index)
        {
            Values[index] = Values[index + 1];
        }
        Values[free - 1] = default;
        free--;
    }
}