namespace SpanCollections.Tests;

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

public class EndianValueStruct
{
    [Fact]
    public void ShouldHaveSameSizeAsTypeParameter()
    {
        Assert.Equal(
            Unsafe.SizeOf<Guid>(),
            Unsafe.SizeOf<EndianValue<NativeEndian, Guid>>()
        );
    }

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

    public class AsMethodShould
    {
        [Fact]
        public void SwapUnderlyingBytesWhenNecessary()
        {
            EndianValue<NativeEndian, int> nativeValue = 42;
            var notNativeValue = nativeValue.As<Not<NativeEndian>>();
            Assert.Equal(42, notNativeValue.Value);
            Assert.Equal(
                MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref nativeValue, 1)).ToArray(),
                MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref notNativeValue, 1)).ToArray().Reverse()
            );
        }
    }

    public class EqualityShould
    {
        [Fact]
        public void BeBasedOnConvertedValueNotStoredValue()
        {
            EndianValue<NativeEndian, int> nativeValue = 42;
            EndianValue<Not<NativeEndian>, int> notNativeValue = 42;
            Assert.True(nativeValue.Equals(in notNativeValue));
            Assert.True(nativeValue == notNativeValue);
            Assert.True(nativeValue.Equals(notNativeValue.Value));
            Assert.True(nativeValue.Equals((IReadableValue<int>)notNativeValue));
        }
    }

    public class GetHashCodeMethodShould
    {
        [Fact]
        public void BeBasedOnConvertedValueNotStoredValue()
        {
            var random = new Random(0);
            for (var i = 0; i < 1000; i++)
            {
                var expected = random.Next();
                EndianValue<NativeEndian, int> nativeValue = expected;
                EndianValue<Not<NativeEndian>, int> notNativeValue = expected;
                Assert.Equal(
                    expected.GetHashCode(),
                    nativeValue.GetHashCode()
                );
                Assert.Equal(
                    expected.GetHashCode(),
                    notNativeValue.GetHashCode()
                );
            }
        }
    }

    public class InequalityShould
    {
        [Fact]
        public void BeOppositeOfEquality()
        {
            EndianValue<NativeEndian, int> left = 42;
            EndianValue<NativeEndian, int> right = 43;
            Assert.False(left == right);
            Assert.True(left != right);
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