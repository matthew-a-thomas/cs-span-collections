namespace SpanCollections;

using System;

public sealed class BigEndian : IEndianness
{
    public static bool ShouldReverseBytes => BitConverter.IsLittleEndian;
}