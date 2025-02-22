namespace SpanCollections.Formats.BitOrders;

public sealed class LsbFirst : IBitOrder
{
    public static bool ShouldReverseBits => true;
}