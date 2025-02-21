namespace SpanCollections;

using System;

public sealed class LittleEndian : IEndianness
{
    public static bool ShouldSwapEndianness => !BitConverter.IsLittleEndian;
}