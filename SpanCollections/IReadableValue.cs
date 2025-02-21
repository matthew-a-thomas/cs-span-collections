namespace SpanCollections;

public interface IReadableValue<out T>
{
    T Value { get; }
}