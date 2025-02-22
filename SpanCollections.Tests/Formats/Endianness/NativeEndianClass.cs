namespace SpanCollections.Tests.Formats.Endianness;

using SpanCollections.Formats.Endianness;
using Xunit;

public class NativeEndianClass
{
    public class ShouldSwapEndiannessPropertyShould
    {
        [Fact]
        public void ReturnFalse()
        {
            Assert.False(NativeEndian.ShouldReverseBytes);
        }
    }
}