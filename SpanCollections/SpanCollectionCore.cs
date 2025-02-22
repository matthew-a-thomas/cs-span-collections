namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

readonly ref struct SpanCollectionCore<T>(Span<byte> bytes, int numPointers)
    where T : unmanaged
{
    public readonly Span<EndianValue<LittleEndian, MsbFirst, int>> Pointers = MemoryMarshal.Cast<byte, EndianValue<LittleEndian, MsbFirst, int>>(bytes)[..numPointers];
    public readonly Span<T> Values = MemoryMarshal.Cast<byte, T>(bytes[(numPointers * sizeof(int))..]);
}