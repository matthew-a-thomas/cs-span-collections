namespace SpanCollections.Formats.Endianness;

public sealed class NativeEndian : IEndianness
{
    public static bool ShouldReverseBytes => false;
}