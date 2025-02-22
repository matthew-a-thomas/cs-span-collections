namespace SpanCollections;

using System;

public sealed class LittleEndian : IEndianness
{
    public static bool ShouldReverseBytes => !BitConverter.IsLittleEndian;
}