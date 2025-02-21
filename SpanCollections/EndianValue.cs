namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Encapsulates a single value that is stored in memory in the given <typeparamref name="TEndianness"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct EndianValue<TEndianness, T> : IEquatable<T>, IEquatable<IReadableValue<T>>, IReadableValue<T>, IWriteableValue<T>
    where T : unmanaged
    where TEndianness : IEndianness
{
    /// <summary>
    /// The value as stored in the given <typeparamref name="TEndianness"/>.
    /// </summary>
    T _value;

    /// <summary>
    /// Gets/sets the value.
    /// </summary>
    /// <remarks>
    /// The endianness of the underlying value is swapped as needed while reading and writing, such that it is always
    /// stored in memory in the given <typeparamref name="TEndianness"/>.
    /// </remarks>
    public T Value
    {
        get
        {
            var value = _value;
            MaybeSwap(ref value);
            return value;
        }
        set
        {
            MaybeSwap(ref value);
            _value = value;
        }
    }

    public EndianValue(T value)
    {
        Value = value;
    }

    public EndianValue<TNewEndianness, T> ConvertTo<TNewEndianness>()
        where TNewEndianness : IEndianness => Value;

    public bool Equals(T other) =>
        Value.Equals(other);

    public bool Equals<TOther>(in TOther other)
        where TOther : IReadableValue<T> =>
        Value.Equals(other.Value);

    public bool Equals(IReadableValue<T>? other) =>
        other is not null && Equals(in other);

    public override bool Equals(object? obj) =>
        obj is IReadableValue<T> other && Equals(in other);

    public override int GetHashCode() => Value.GetHashCode();

    static void MaybeSwap(ref T value)
    {
        if (TEndianness.ShouldSwapEndianness)
            MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1)).Reverse();
    }

    public static bool operator ==(in EndianValue<TEndianness, T> left, in EndianValue<TEndianness, T> right) => left.Value.Equals(right.Value);
    public static bool operator !=(in EndianValue<TEndianness, T> left, in EndianValue<TEndianness, T> right) => !(left == right);
    public static implicit operator EndianValue<TEndianness, T>(T value) => new(value);
    public static implicit operator T(EndianValue<TEndianness, T> value) => value.Value;
}