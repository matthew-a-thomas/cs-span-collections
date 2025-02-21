namespace SpanCollections;

public sealed class NativeEndian : IEndianness
{
    public static bool ShouldSwapEndianness => false;
}