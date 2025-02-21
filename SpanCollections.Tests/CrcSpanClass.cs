namespace SpanCollections.Tests;

using System;
using Xunit;

public class CrcSpanClass
{
    [Fact]
    public void MiscTests()
    {
        var bytes = new byte[8];
        var crcSpan = new CrcSpan<int>(bytes);
        Assert.False(crcSpan.IsValid());
        crcSpan.Values[0] = 42;
        Assert.False(crcSpan.IsValid());
        crcSpan.UpdateCrc32();
        Assert.True(crcSpan.IsValid());
        crcSpan.Values[0] = 43;
        Assert.False(crcSpan.IsValid());
        crcSpan.UpdateCrc32();
        Assert.True(crcSpan.IsValid());
        var span2 = new CrcSpan<int>(bytes);
        Assert.Equal(43, span2.Values[0]);
    }

    public class ConstructorShould
    {
        [Fact]
        public void ThrowWhenTooSmall()
        {
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                _ = new CrcSpan<int>(Array.Empty<byte>());
            });
        }
    }

    public class Crc32FieldShould
    {
        [Fact]
        public void ChangeAfterUpdates()
        {
            var crcSpan = new CrcSpan<int>(new byte[8]);
            crcSpan.Values[0] = 42;
            var before = crcSpan.Crc32;
            crcSpan.UpdateCrc32();
            var after = crcSpan.Crc32;
            Assert.NotEqual(before, after);
        }

        [Fact]
        public void CauseIsValidToReturnFalseWhenChanged()
        {
            var crcSpan = new CrcSpan<int>(new byte[8]);
            crcSpan.UpdateCrc32();
            Assert.True(crcSpan.IsValid());
            crcSpan.Crc32 ^= ~0U;
            Assert.False(crcSpan.IsValid());
        }

        [Fact]
        public void PersistChangesToUnderlyingSpan()
        {
            var bytes = new byte[8];
            new CrcSpan<int>(bytes).Crc32 = 42U;
            Assert.Equal(42U, new CrcSpan<int>(bytes).Crc32);
        }
    }

    public class ValuesFieldShould
    {
        [Fact]
        public void PersistChangesToUnderlyingSpan()
        {
            var bytes = new byte[8];

            {
                var crcSpan = new CrcSpan<int>(bytes);
                crcSpan.Values[0] = 42;
                crcSpan.UpdateCrc32();
                Assert.True(crcSpan.IsValid());
            }

            Assert.Equal(
                42,
                new CrcSpan<int>(bytes).Values[0]
            );
        }
    }
}