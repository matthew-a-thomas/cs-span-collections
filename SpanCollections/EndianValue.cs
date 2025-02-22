namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Encapsulates a single value that is stored in memory in the given <typeparamref name="TEndianness"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct EndianValue<TEndianness, TBitOrder, T> : IEquatable<T>, IEquatable<IReadableValue<T>>, IReadableValue<T>, IWriteableValue<T>
    where TEndianness : IEndianness
    where TBitOrder : IBitOrder
    where T : unmanaged
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
            if (!TEndianness.ShouldReverseBytes && !TBitOrder.ShouldReverseBits)
            {
                return UnsafeBlobContents;
            }

            var clone = Blob;
            var bytes = clone.AsBytes();

            if (TEndianness.ShouldReverseBytes)
            {
                bytes.Reverse();
            }

            if (TBitOrder.ShouldReverseBits)
            {
                ByteHelpers.ReverseBits(bytes);
            }

            return MemoryMarshal.Cast<byte, T>(bytes)[0];
        }
        set
        {
            if (!TEndianness.ShouldReverseBytes && !TBitOrder.ShouldReverseBits)
            {
                UnsafeBlobContents = value;
                return;
            }

            Blob<T> blob = default;
            var bytes = blob.AsBytes();
            MemoryMarshal.Cast<byte, T>(bytes)[0] = value;

            if (TEndianness.ShouldReverseBytes)
            {
                bytes.Reverse();
            }

            if (TBitOrder.ShouldReverseBits)
            {
                ByteHelpers.ReverseBits(bytes);
            }

            Blob = blob;
        }
    }

    /// <summary>
    /// Creates a new <see cref="EndianValue{TEndianness,TBitOrder,T}"/> that will have the given value.
    /// </summary>
    public EndianValue(T value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new <see cref="EndianValue{TEndianness,TBitOrder,T}"/> using the given underlying blob.
    /// </summary>
    public EndianValue(Blob<T> blob)
    {
        Blob = blob;
    }

    /// <summary>
    /// Returns a new <see cref="EndianValue{TNewEndianness,TNewBitOrder,T}"/> where the underlying endianness has been changed to
    /// <typeparamref name="TNewEndianness"/>.
    /// </summary>
    public EndianValue<TNewEndianness, TBitOrder, T> As<TNewEndianness, TNewBitOrder>()
        where TNewEndianness : IEndianness
        where TNewBitOrder : IBitOrder =>
        TNewEndianness.ShouldReverseBytes == TEndianness.ShouldReverseBytes &&
        TNewBitOrder.ShouldReverseBits == TBitOrder.ShouldReverseBits
            ? new EndianValue<TNewEndianness, TBitOrder, T>(Blob)
            : new EndianValue<TNewEndianness, TBitOrder, T>(Value);

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

    public static bool operator ==(in EndianValue<TEndianness, TBitOrder, T> left, in EndianValue<TEndianness, TBitOrder, T> right) => left.Value.Equals(right.Value);
    public static bool operator !=(in EndianValue<TEndianness, TBitOrder, T> left, in EndianValue<TEndianness, TBitOrder, T> right) => !(left == right);
    public static implicit operator EndianValue<TEndianness, TBitOrder, T>(T value) => new(value);
    public static implicit operator T(EndianValue<TEndianness, TBitOrder, T> value) => value.Value;
}