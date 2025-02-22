namespace SpanCollections;

public interface IEndianness
{
    public static abstract bool ShouldReverseBytes { get; }
}