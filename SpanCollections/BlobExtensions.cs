namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

public static class BlobExtensions
{
    public static Span<byte> AsBytes<T>(ref this Blob<T> blob)
        where T : unmanaged =>
        MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref blob, 1));
}