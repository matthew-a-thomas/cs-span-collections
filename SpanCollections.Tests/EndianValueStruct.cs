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
            Unsafe.SizeOf<EndianValue<NativeEndian, NativeBitOrder, Guid>>()
        );
    }

    [Fact]
    public void ShouldSwapUnderlyingBytesWhenNecessary()
    {
        var value = 42;
        var span = MemoryMarshal.CreateSpan(ref value, 1);
        ref var nativeEndianValue = ref MemoryMarshal.Cast<int, EndianValue<NativeEndian, NativeBitOrder, int>>(span)[0];
        ref var oppositeEndianValue = ref MemoryMarshal.Cast<int, EndianValue<NotNativeEndian, NativeBitOrder, int>>(span)[0];
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
            EndianValue<NativeEndian, NativeBitOrder, int> nativeValue = 42;
            var notNativeValue = nativeValue.As<NotNativeEndian, NativeBitOrder>();
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
            EndianValue<NativeEndian, NativeBitOrder, int> nativeValue = 42;
            EndianValue<NotNativeEndian, NativeBitOrder, int> notNativeValue = 42;
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
                EndianValue<NativeEndian, NativeBitOrder, int> nativeValue = expected;
                EndianValue<NotNativeEndian, NativeBitOrder, int> notNativeValue = expected;
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
            EndianValue<NativeEndian, NativeBitOrder, int> left = 42;
            EndianValue<NativeEndian, NativeBitOrder, int> right = 43;
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
                EndianValue<BigEndian, NativeBitOrder, int> a = expected;
                Assert.Equal(expected, a.Value);

                EndianValue<LittleEndian, NativeBitOrder, int> b = expected;
                Assert.Equal(expected, b.Value);

                EndianValue<NativeEndian, NativeBitOrder, int> c = expected;
                Assert.Equal(expected, c.Value);

                EndianValue<NotNativeEndian, NativeBitOrder, int> d = expected;
                Assert.Equal(expected, d.Value);
            }
        }
    }
}