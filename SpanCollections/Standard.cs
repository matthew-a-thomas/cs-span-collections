namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

public class Standard<TEndianness, TBitOrder> : IFormat
    where TEndianness : IEndianness
    where TBitOrder : IBitOrder
{
    public static T Read<T>(in Blob<T> blob)
        where T : unmanaged
    {
        var span = MemoryMarshal.CreateReadOnlySpan(in blob, 1);
        if (!TEndianness.ShouldReverseBytes && !TBitOrder.ShouldReverseBits)
        {
            return MemoryMarshal.Cast<Blob<T>, T>(span)[0];
        }

        var clone = blob;
        var bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref clone, 1));

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

    public static void Write<T>(in T value, ref Blob<T> blob)
        where T : unmanaged
    {
        var span = MemoryMarshal.CreateSpan(ref blob, 1);
        MemoryMarshal.Cast<Blob<T>, T>(span)[0] = value;
        if (!TEndianness.ShouldReverseBytes && !TBitOrder.ShouldReverseBits)
        {
            return;
        }

        var bytes = MemoryMarshal.AsBytes(span);
        if (TEndianness.ShouldReverseBytes)
        {
            bytes.Reverse();
        }
        if (TBitOrder.ShouldReverseBits)
        {
            ByteHelpers.ReverseBits(bytes);
        }
    }
}