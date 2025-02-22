namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

public readonly ref struct CrcSpan<T>
    where T : unmanaged
{
    public const int MinNumBytes = sizeof(uint);

    public readonly ref FormattedValue<X86, uint> Crc32;

    public readonly Span<T> Values;

    public CrcSpan(Span<byte> bytes)
    {
        if (bytes.Length < MinNumBytes)
            throw new ArgumentException($"Length of {nameof(bytes)} must be at least {MinNumBytes}", nameof(bytes));
        Crc32 = ref MemoryMarshal.Cast<byte, FormattedValue<X86, uint>>(bytes)[0];
        Values = MemoryMarshal.Cast<byte, T>(bytes[MinNumBytes..]);
    }

    uint HashValues() => System.IO.Hashing.Crc32.HashToUInt32(MemoryMarshal.AsBytes(Values));

    public bool IsValid() => HashValues() == Crc32;

    public void UpdateCrc32()
    {
        Crc32 = HashValues();
    }
}