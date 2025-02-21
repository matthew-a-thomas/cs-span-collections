namespace SpanCollections;

using System;

public sealed class BigEndian : IEndianness
{
    public static bool ShouldSwapEndianness => BitConverter.IsLittleEndian;
}