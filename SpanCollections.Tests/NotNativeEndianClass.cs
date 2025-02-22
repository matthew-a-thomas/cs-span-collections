namespace SpanCollections.Tests;

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