namespace SpanCollections.Formats.Endianness;

public sealed class NotNativeEndian : IEndianness
{
    public static bool ShouldReverseBytes => true;
}