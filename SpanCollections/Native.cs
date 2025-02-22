namespace SpanCollections;

using System.Runtime.InteropServices;

public sealed class Native : IFormat
{
    public static T Read<T>(in Blob<T> blob) where T : unmanaged =>
        MemoryMarshal.Cast<Blob<T>, T>(MemoryMarshal.CreateReadOnlySpan(in blob, 1))[0];

    public static void Write<T>(in T value, ref Blob<T> blob) where T : unmanaged =>
        MemoryMarshal.Cast<Blob<T>, T>(MemoryMarshal.CreateSpan(ref blob, 1))[0] = value;
}