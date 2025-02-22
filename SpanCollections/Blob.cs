using System;

namespace SpanCollections;

using System.Runtime.InteropServices;

/// <summary>
/// An opaque type large enough to hold a single <typeparamref name="T"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Blob<T> : IEquatable<Blob<T>>
    where T : unmanaged
{
    readonly T _value;

    public bool Equals(Blob<T> other) =>
        _value.Equals(other._value);

    public override bool Equals(object? obj) =>
        obj is Blob<T> other && Equals(other);

    public override int GetHashCode() =>
        _value.GetHashCode();

    public static bool operator ==(Blob<T> left, Blob<T> right) =>
        left.Equals(right);

    public static bool operator !=(Blob<T> left, Blob<T> right) =>
        !left.Equals(right);
}