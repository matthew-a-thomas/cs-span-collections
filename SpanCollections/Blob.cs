namespace SpanCollections;

using System.Runtime.InteropServices;

/// <summary>
/// An opaque type large enough to hold a single <typeparamref name="T"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Blob<T>
    where T : unmanaged
{
    readonly T _value;
}