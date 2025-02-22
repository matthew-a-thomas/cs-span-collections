namespace SpanCollections.Formats.Endianness;

using System;

public sealed class BigEndian : IEndianness
{
    public static bool ShouldReverseBytes => BitConverter.IsLittleEndian;
}