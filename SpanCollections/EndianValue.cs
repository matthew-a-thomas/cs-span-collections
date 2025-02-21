namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Encapsulates a single value that is stored in memory in the given <typeparamref name="TEndianness"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct EndianValue<TEndianness, T>
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

    static void MaybeSwap(ref T value)
    {
        if (TEndianness.ShouldSwapEndianness)
            MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1)).Reverse();
    }

    public static implicit operator EndianValue<TEndianness, T>(T value) => new(value);
    public static implicit operator T(EndianValue<TEndianness, T> value) => value.Value;
}