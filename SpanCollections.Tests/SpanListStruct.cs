namespace SpanCollections.Tests;

using System;
using System.Linq;
using Xunit;

public class SpanListStruct
{
    [Fact]
    public void MiscTests()
    {
        var bytes = new byte[6];
        var list = new SpanList<byte>(bytes);
        Assert.Equal(0, list.Count);
        Assert.Equal(2, list.Capacity);
        list.Add(1);
        Assert.Equal(1, list.Count);
        Assert.Equal(1, list[0]);
        list.Add(2);
        Assert.Equal(2, list.Count);
        Assert.Equal(2, list[1]);
        var values = list[..2];
        values[0] = 3;
        Assert.Equal(3, list[0]);
        Assert.Equal(3, bytes[4]);
        values[1] = 4;
        Assert.Equal(4, list[1]);
        Assert.Equal(4, bytes[5]);
        list.RemoveAt(0);
        Assert.Equal(1, list.Count);
        Assert.Equal(4, list[0]);
        Assert.Equal(0, values[1]);
        Assert.Equal(0, bytes[5]);
        list.Add(5);
        Assert.Equal(2, list.Count);
        Assert.Equal(5, bytes[5]);
        list.Clear();
        Assert.Equal(
            Enumerable.Repeat<byte>(0, bytes.Length),
            bytes
        );
        Assert.Equal(0, list.Count);
        list.Add(6);
        var enumerator = list.GetEnumerator();
        Assert.True(enumerator.MoveNext());
        ref readonly var current = ref enumerator.Current;
        Assert.Equal(6, current);
        bytes[4] = 7;
        Assert.Equal(7, current);
        Assert.False(enumerator.MoveNext());
        list.Add(8);
        enumerator = list.GetEnumerator();
        Assert.True(enumerator.MoveNext());
        Assert.Equal(7, enumerator.Current);
        Assert.True(enumerator.MoveNext());
        Assert.Equal(8, enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    public class ConstructorShould
    {
        [Fact]
        public void ThrowWhenTooSmall()
        {
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                _ = new SpanList<byte>(Array.Empty<byte>());
            });
        }
    }
}