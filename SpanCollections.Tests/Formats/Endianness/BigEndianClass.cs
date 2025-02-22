namespace SpanCollections.Tests.Formats.Endianness;

using System;
using SpanCollections.Formats.Endianness;
using Xunit;

public class BigEndianClass
{
    public class ShouldSwapEndiannessPropertyShould
    {
        [Fact]
        public void ReturnTrueIfLittleEndian()
        {
            Assert.Equal(
                BitConverter.IsLittleEndian,
                BigEndian.ShouldReverseBytes
            );
        }
    }
}