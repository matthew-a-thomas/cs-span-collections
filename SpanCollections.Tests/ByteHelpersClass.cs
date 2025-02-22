using Xunit;

namespace SpanCollections.Tests;

public class ByteHelpersClass
{
    public class ReversedBytesPropertyShould
    {
        [Fact]
        public void Have256Elements()
        {
            Assert.Equal(256, ByteHelpers.ReversedBytes.Length);
        }

        [Fact]
        public void RoundTrip()
        {
            for (var i = 0; i < 256; i++)
            {
                var expected = (byte)i;
                var reversed = ByteHelpers.ReversedBytes[expected];
                var actual = ByteHelpers.ReversedBytes[reversed];
                Assert.Equal(expected, actual);
            }
        }
    }
}