namespace SpanCollections.Tests;

using System;
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
                LittleEndian.ShouldSwapEndianness
            );
        }
    }
}