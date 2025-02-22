namespace SpanCollections.Formats.BitOrders;

public sealed class MsbFirst : IBitOrder
{
    public static bool ShouldReverseBits => false;
}