namespace SpanCollections;

public interface IWriteableValue<in T>
{
    T Value { set; }
}