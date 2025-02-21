namespace SpanCollections.Tests;

using Xunit;

public class NativeEndianClass
{
    public class ShouldSwapEndiannessPropertyShould
    {
        [Fact]
        public void ReturnFalse()
        {
            Assert.False(NativeEndian.ShouldSwapEndianness);
        }
    }
}