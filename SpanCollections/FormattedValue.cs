namespace SpanCollections;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Encapsulates a single value that is stored in memory in the given <typeparamref name="TFormat"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FormattedValue<TFormat, T> :
    IEquatable<FormattedValue<TFormat, T>>,
    IEquatable<T>,
    IEquatable<IReadableValue<T>>,
    IReadableValue<T>,
    IWriteableValue<T>
    where TFormat : IFormat
    where T : unmanaged
{
    /// <summary>
    /// A blob holding the value in the given <typeparamref name="TFormat"/>.
    /// </summary>
    public Blob<T> Blob;

    /// <summary>
    /// Gets/sets the value.
    /// </summary>
    public T Value
    {
        get => TFormat.Read(in Blob);
        set => TFormat.Write(in value, ref Blob);
    }

    /// <summary>
    /// Creates a new <see cref="FormattedValue{TFormat,T}"/> that will have the given value.
    /// </summary>
    public FormattedValue(T value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new <see cref="FormattedValue{TFormat,T}"/> using the given underlying blob.
    /// </summary>
    public FormattedValue(Blob<T> blob)
    {
        Blob = blob;
    }

    /// <summary>
    /// Returns a new <see cref="FormattedValue{TFormat,T}"/> where the underlying format has been changed to
    /// <typeparamref name="TNewFormat"/>.
    /// </summary>
    public FormattedValue<TNewFormat, T> As<TNewFormat>()
        where TNewFormat : IFormat =>
        new(Value);

    public bool Equals<TOther>(in TOther other)
        where TOther : IReadableValue<T> =>
        Value.Equals(other.Value);

    public bool Equals(FormattedValue<TFormat, T> other) =>
        other.Blob == Blob;

    public bool Equals(T other) =>
        Value.Equals(other);

    public bool Equals(IReadableValue<T>? other) =>
        other is not null && Equals(in other);

    public override bool Equals(object? obj) =>
        obj is IReadableValue<T> other && Equals(in other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(in FormattedValue<TFormat, T> left, in FormattedValue<TFormat, T> right) => left.Value.Equals(right.Value);
    public static bool operator !=(in FormattedValue<TFormat, T> left, in FormattedValue<TFormat, T> right) => !(left == right);
    public static implicit operator FormattedValue<TFormat, T>(T value) => new(value);
    public static implicit operator T(FormattedValue<TFormat, T> value) => value.Value;
}