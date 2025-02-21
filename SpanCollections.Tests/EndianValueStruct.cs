namespace SpanCollections.Tests;

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

public class EndianValueStruct
{
    [Fact]
    public void ShouldSwapUnderlyingBytesWhenNecessary()
    {
        var value = 42;
        var span = MemoryMarshal.CreateSpan(ref value, 1);
        ref var nativeEndianValue = ref MemoryMarshal.Cast<int, EndianValue<NativeEndian, int>>(span)[0];
        ref var oppositeEndianValue = ref MemoryMarshal.Cast<int, EndianValue<Not<NativeEndian>, int>>(span)[0];
        Assert.Equal<int>(42, nativeEndianValue);
        Assert.NotEqual<int>(42, oppositeEndianValue);
        oppositeEndianValue = 42;
        Assert.Equal<int>(42, oppositeEndianValue);
        Assert.NotEqual<int>(42, nativeEndianValue);
    }

    public class ConvertToMethodShould
    {
        [Fact]
        public void SwapUnderlyingBytesWhenNecessary()
        {
            EndianValue<NativeEndian, int> nativeValue = 42;
            var notNativeValue = nativeValue.ConvertTo<Not<NativeEndian>>();
            Assert.Equal(42, notNativeValue.Value);
            Assert.Equal(
                MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref nativeValue, 1)).ToArray(),
                MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref notNativeValue, 1)).ToArray().Reverse()
            );
        }
    }

    public class ValuePropertyShould
    {
        [Fact]
        public void RoundTrip()
        {
            var random = new Random(0);
            for (var i = 0; i < 1000; i++)
            {
                var expected = random.Next();
                EndianValue<BigEndian, int> a = expected;
                Assert.Equal(expected, a.Value);

                EndianValue<LittleEndian, int> b = expected;
                Assert.Equal(expected, b.Value);

                EndianValue<NativeEndian, int> c = expected;
                Assert.Equal(expected, c.Value);

                EndianValue<Not<NativeEndian>, int> d = expected;
                Assert.Equal(expected, d.Value);
            }
        }
    }
}