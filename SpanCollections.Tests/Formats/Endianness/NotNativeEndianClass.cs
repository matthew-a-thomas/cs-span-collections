namespace SpanCollections.Tests.Formats.Endianness;

using SpanCollections.Formats.Endianness;
using Xunit;

public class NotNativeEndianClass
{
    public class ShouldSwapEndiannessPropertyShould
    {
        [Fact]
        public void ReturnTrue()
        {
            Assert.True(NotNativeEndian.ShouldReverseBytes);
        }
    }
}