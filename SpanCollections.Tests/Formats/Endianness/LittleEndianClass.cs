namespace SpanCollections.Tests.Formats.Endianness;

using System;
using SpanCollections.Formats.Endianness;
using Xunit;

public class LittleEndianClass
{
    public class ShouldSwapEndiannessPropertyShould
    {
        [Fact]
        public void ReturnFalseIfLittleEndian()
        {
            Assert.Equal(
                !BitConverter.IsLittleEndian,
                LittleEndian.ShouldReverseBytes
            );
        }
    }
}