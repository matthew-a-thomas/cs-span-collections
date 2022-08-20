namespace SpanCollections;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public readonly ref struct SpanStack<T>
where T : unmanaged
{
    public const int MinNumBytes = sizeof(int) * NumPointers;
    const int NumPointers = 1;

    readonly Span<T> _values;
    readonly Span<int> _pointers;

    public SpanStack(Span<byte> bytes)
    {
        if (bytes.Length < MinNumBytes)
            throw new ArgumentException($"This stack requires at least {MinNumBytes} bytes", nameof(bytes));
        _pointers = MemoryMarshal.Cast<byte, int>(bytes)[..NumPointers];
        bytes = bytes[(Unsafe.SizeOf<int>() * NumPointers)..];
        _values = MemoryMarshal.Cast<byte, T>(bytes);
    }

    public int Capacity => _values.Length;

    public int Count => Math.Max(0, Math.Min(Capacity, FreePointer));

    ref int FreePointer => ref _pointers[0];

    public SpanEnumerator<T> GetEnumerator() => new(true, _values[..FreePointer]);

    public void Push(in T value)
    {
        ref var free = ref FreePointer;
        _values[free] = value;
        free++;
    }

    public bool TryPeek(out T value)
    {
        ref var free = ref FreePointer;
        if (free > 0 && free <= _values.Length)
        {
            value = _values[free - 1];
            return true;
        }
        value = default;
        return false;
    }

    public bool TryPop(out T value)
    {
        ref var free = ref FreePointer;
        if (free > 0 && free <= _values.Length)
        {
            free--;
            value = _values[free];
            _values[free] = default;
            return true;
        }
        value = default;
        return false;
    }
}