namespace SpanCollections;

public sealed class NotNativeEndian : IEndianness
{
    public static bool ShouldReverseBytes => true;
}