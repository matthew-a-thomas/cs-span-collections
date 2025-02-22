namespace SpanCollections;

public interface IFormat
{
    /// <summary>
    /// Reads the memory representation from the given <paramref name="blob"/>.
    /// </summary>
    public static abstract T Read<T>(in Blob<T> blob)
        where T : unmanaged;

    /// <summary>
    /// Writes the memory representation from the given <paramref name="value"/> into the given <paramref name="blob"/>.
    /// </summary>
    public static abstract void Write<T>(in T value, ref Blob<T> blob)
        where T : unmanaged;
}