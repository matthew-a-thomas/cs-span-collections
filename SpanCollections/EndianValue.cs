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
    /// A blob holding the value in the given <typeparamref name="TEndianness"/>.
    /// </summary>
    public Blob<T> Blob;

    ref T UnsafeBlobContents => ref MemoryMarshal.Cast<Blob<T>, T>(MemoryMarshal.CreateSpan(ref Blob, 1))[0];

    /// <summary>
    /// Gets/sets the value.
    /// </summary>
    public T Value
    {
        get
        {
            if (!TEndianness.ShouldSwapEndianness)
            {
                return UnsafeBlobContents;
            }

            var clone = Blob;
            var bytes = clone.AsBytes();
            bytes.Reverse();
            return MemoryMarshal.Cast<byte, T>(bytes)[0];
        }
        set
        {
            if (!TEndianness.ShouldSwapEndianness)
            {
                UnsafeBlobContents = value;
                return;
            }

            Blob<T> blob = default;
            var bytes = blob.AsBytes();
            MemoryMarshal.Cast<byte, T>(bytes)[0] = value;
            bytes.Reverse();
            Blob = blob;
        }
    }

    /// <summary>
    /// Creates a new <see cref="EndianValue{TEndianness,T}"/> that will have the given value.
    /// </summary>
    public EndianValue(T value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new <see cref="EndianValue{TEndianness,T}"/> using the given underlying blob.
    /// </summary>
    public EndianValue(Blob<T> blob)
    {
        Blob = blob;
    }

    /// <summary>
    /// Returns a new <see cref="EndianValue{TNewEndianness,T}"/> where the underlying endianness has been changed to
    /// <typeparamref name="TNewEndianness"/>.
    /// </summary>
    public EndianValue<TNewEndianness, T> As<TNewEndianness>()
        where TNewEndianness : IEndianness =>
        TNewEndianness.ShouldSwapEndianness == TEndianness.ShouldSwapEndianness
            ? new EndianValue<TNewEndianness, T>(Blob)
            : new EndianValue<TNewEndianness, T>(Value);

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

    public static bool operator ==(in EndianValue<TEndianness, T> left, in EndianValue<TEndianness, T> right) => left.Value.Equals(right.Value);
    public static bool operator !=(in EndianValue<TEndianness, T> left, in EndianValue<TEndianness, T> right) => !(left == right);
    public static implicit operator EndianValue<TEndianness, T>(T value) => new(value);
    public static implicit operator T(EndianValue<TEndianness, T> value) => value.Value;
}