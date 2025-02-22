namespace SpanCollections.Tests;

using System;
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