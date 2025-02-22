namespace SpanCollections;

public sealed class NativeEndian : IEndianness
{
    public static bool ShouldReverseBytes => false;
}