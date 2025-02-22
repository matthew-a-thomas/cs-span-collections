namespace SpanCollections.Formats.BitOrders;

public sealed class NotNativeBitOrder : IBitOrder
{
    public static bool ShouldReverseBits => true;
}