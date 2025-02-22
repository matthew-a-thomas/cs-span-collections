namespace SpanCollections;

using System;

public static class ByteHelpers
{
    static readonly byte[] ReversedBytesArray = new byte[256];

    public static ReadOnlySpan<byte> ReversedBytes => ReversedBytesArray;

    static ByteHelpers()
    {
        for (var i = 0; i < 256; i++)
        {
            ReversedBytesArray[i] = Reverse((byte)i);
        }
    }

    static byte Reverse(byte b)
    {
        // https://stackoverflow.com/a/2602885/3063273
        b = (byte)((b & 0xF0) >> 4 | (b & 0x0F) << 4);
        b = (byte)((b & 0xCC) >> 2 | (b & 0x33) << 2);
        b = (byte)((b & 0xAA) >> 1 | (b & 0x55) << 1);
        return b;
    }

    public static void ReverseBits(Span<byte> span)
    {
        foreach (ref var value in span)
        {
            value = ReversedBytesArray[value];
        }
    }
}