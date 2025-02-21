namespace SpanCollections;

public sealed class Not<TOpposite> : IEndianness
    where TOpposite : IEndianness
{
    public static bool ShouldSwapEndianness => !TOpposite.ShouldSwapEndianness;
}