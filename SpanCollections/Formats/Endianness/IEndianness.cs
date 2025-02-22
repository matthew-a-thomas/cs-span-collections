namespace SpanCollections.Formats.Endianness;

public interface IEndianness
{
    public static abstract bool ShouldReverseBytes { get; }
}