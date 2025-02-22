namespace SpanCollections;

using System;

public readonly ref struct SpanStack<T>
where T : unmanaged
{
    public const int MinNumBytes = sizeof(int) * NumPointers;
    const int NumPointers = 1;

    readonly SpanCollectionCore<T> _core;

    public SpanStack(Span<byte> bytes)
    {
        if (bytes.Length < MinNumBytes)
            throw new ArgumentException($"This stack requires at least {MinNumBytes} bytes", nameof(bytes));
        _core = new SpanCollectionCore<T>(bytes, NumPointers);
    }

    public int Capacity => Values.Length;

    public int Count => Math.Max(0, Math.Min(Capacity, FreePointer));

    ref EndianValue<LittleEndian, MsbFirst, int> FreePointer => ref _core.Pointers[0];

    Span<T> Values => _core.Values;

    public SpanEnumerator<T> GetEnumerator() => new(true, Values[..FreePointer.Value]);

    public void Push(in T value)
    {
        ref var free = ref FreePointer;
        Values[free] = value;
        free++;
    }

    public bool TryPeek(out T value)
    {
        ref var free = ref FreePointer;
        if (free > 0 && free <= Values.Length)
        {
            value = Values[free - 1];
            return true;
        }
        value = default;
        return false;
    }

    public bool TryPop(out T value)
    {
        ref var free = ref FreePointer;
        if (free > 0 && free <= Values.Length)
        {
            free--;
            value = Values[free];
            Values[free] = default;
            return true;
        }
        value = default;
        return false;
    }
}