namespace SpanCollections.Tests;

using System.IO.Hashing;
using System.Runtime.InteropServices;
using Xunit;

public class CrcValueStruct
{
    public class Crc32FieldShould
    {
        [Fact]
        public void ReflectCrc32HashOfNewValue()
        {
            var i = 42;
            var crc32 = Crc32.HashToUInt32(MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref i, 1)));
            CrcValue<int> value = i;
            Assert.Equal<uint>(crc32, value.Crc32);
        }
    }

    public class IsValidMethodShould
    {
        [Fact]
        public void ReturnTrueWhenCorrect()
        {
            CrcValue<int> value = 42;
            Assert.True(value.IsValid());
        }

        [Fact]
        public void ReturnFalseWhenCrcIsWrong()
        {
            CrcValue<int> value = 42;
            MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[0] ^= 0xff;
            Assert.False(value.IsValid());
        }
    }

    public class TryReadMethodShould
    {
        [Fact]
        public void ReturnTrueAndOutputValueWhenCorrect()
        {
            CrcValue<int> value = 42;
            Assert.True(value.TryRead(out var output));
            Assert.Equal(42, output);
        }

        [Fact]
        public void ReturnFalseWhenCrcIsWrong()
        {
            CrcValue<int> value = 42;
            MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[0] ^= 0xff;
            Assert.False(value.TryRead(out _));
        }
    }
}