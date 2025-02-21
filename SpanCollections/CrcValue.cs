namespace SpanCollections;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct CrcValue<T>(in T value)
    where T : unmanaged
{
    public readonly T Value = value;

    public readonly uint Crc32 = Hash(in value);

    static uint Hash(in T value) => System.IO.Hashing.Crc32.HashToUInt32(MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(in value, 1)));

    public bool IsValid() => Crc32 == Hash(in Value);

    public bool TryRead(out T value)
    {
        if (IsValid())
        {
            value = Value;
            return true;
        }
        value = default;
        return false;
    }

    public static implicit operator CrcValue<T>(in T value) => new(in value);
}