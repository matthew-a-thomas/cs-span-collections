namespace SpanCollections.Tests;

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

public class FormattedValueStruct
{
    [Fact]
    public void ShouldHaveSameSizeAsTypeParameter()
    {
        Assert.Equal(
            Unsafe.SizeOf<Guid>(),
            Unsafe.SizeOf<FormattedValue<Native, Guid>>()
        );
    }

    [Fact]
    public void ShouldSwapUnderlyingBytesWhenNecessary()
    {
        var value = 42;
        var span = MemoryMarshal.CreateSpan(ref value, 1);
        ref var nativeEndianValue = ref MemoryMarshal.Cast<int, FormattedValue<Native, int>>(span)[0];
        ref var oppositeEndianValue = ref MemoryMarshal.Cast<int, FormattedValue<Standard<NotNativeEndian, NativeBitOrder>, int>>(span)[0];
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
            FormattedValue<Standard<NativeEndian, NativeBitOrder>, int> nativeValue = 42;
            var notNativeValue = nativeValue.As<Standard<NotNativeEndian, NativeBitOrder>>();
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
            FormattedValue<Native, int> nativeValue = 42;
            FormattedValue<Standard<NotNativeEndian, NativeBitOrder>, int> notNativeValue = 42;
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
                FormattedValue<Native, int> nativeValue = expected;
                FormattedValue<Standard<NotNativeEndian, NativeBitOrder>, int> notNativeValue = expected;
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
            FormattedValue<Native, int> left = 42;
            FormattedValue<Native, int> right = 43;
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
                FormattedValue<Standard<NotNativeEndian, NotNativeBitOrder>, int> a = expected;
                Assert.Equal(expected, a.Value);

                FormattedValue<Standard<NotNativeEndian, NotNativeBitOrder>, int> b = expected;
                Assert.Equal(expected, b.Value);

                FormattedValue<Standard<NotNativeEndian, NotNativeBitOrder>, int> c = expected;
                Assert.Equal(expected, c.Value);

                FormattedValue<Standard<NotNativeEndian, NotNativeBitOrder>, int> d = expected;
                Assert.Equal(expected, d.Value);
            }
        }
    }
}