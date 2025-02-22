namespace SpanCollections;

public sealed class NotNativeBitOrder : IBitOrder
{
    public static bool ShouldReverseBits => true;
}