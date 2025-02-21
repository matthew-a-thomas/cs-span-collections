namespace SpanCollections.Tests;

using Xunit;

public class NotClass
{
    public class ShouldSwapEndiannessPropertyShould
    {
        [Fact]
        public void ReturnOppositeOfOtherEndianness()
        {
            Assert.NotEqual(
                BigEndian.ShouldSwapEndianness,
                Not<BigEndian>.ShouldSwapEndianness
            );
            Assert.NotEqual(
                LittleEndian.ShouldSwapEndianness,
                Not<LittleEndian>.ShouldSwapEndianness
            );
            Assert.NotEqual(
                NativeEndian.ShouldSwapEndianness,
                Not<NativeEndian>.ShouldSwapEndianness
            );
        }

        [Fact]
        public void NegateItself()
        {
            Assert.Equal(
                BigEndian.ShouldSwapEndianness,
                Not<Not<BigEndian>>.ShouldSwapEndianness
            );
            Assert.Equal(
                LittleEndian.ShouldSwapEndianness,
                Not<Not<LittleEndian>>.ShouldSwapEndianness
            );
            Assert.Equal(
                NativeEndian.ShouldSwapEndianness,
                Not<Not<NativeEndian>>.ShouldSwapEndianness
            );

            Assert.NotEqual(
                Not<BigEndian>.ShouldSwapEndianness,
                Not<Not<BigEndian>>.ShouldSwapEndianness
            );
            Assert.NotEqual(
                Not<LittleEndian>.ShouldSwapEndianness,
                Not<Not<LittleEndian>>.ShouldSwapEndianness
            );
            Assert.NotEqual(
                Not<NativeEndian>.ShouldSwapEndianness,
                Not<Not<NativeEndian>>.ShouldSwapEndianness
            );
        }
    }
}