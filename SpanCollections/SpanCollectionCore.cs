namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

readonly ref struct SpanCollectionCore<T>
where T : unmanaged
{
    public readonly Span<int> Pointers;
    public readonly Span<T> Values;

    public SpanCollectionCore(Span<byte> bytes, int numPointers)
    {
        Pointers = MemoryMarshal.Cast<byte, int>(bytes)[..numPointers];
        Values = MemoryMarshal.Cast<byte, T>(bytes[(numPointers * sizeof(int))..]);
    }
}