using System;
using Xunit;

namespace CustomConverter.Tests
{
    public class CustomConverterExceptionsTests
    {
        [Fact]
        public void CustomConverter_PassNullString_ThrowCustomConverterException()
        {
                Assert.Throws<ArgumentNullException>(() => CustomConverter.ToInt32(null));     
        }

        [Fact]
        public void CustomConverter_PassWhiteSpaceString_ThrowCustomConverterException()
        {
            Assert.Throws<ArgumentException>(() => CustomConverter.ToInt32("   "));
        }

        [Fact]
        public void CustomConverter_PassEmptyString_ThrowCustomConverterException()
        {
            Assert.Throws<ArgumentException>(() => CustomConverter.ToInt32(""));
        }


        [Fact]
        public void CustomConverter_NegativeNumberLimitReached_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CustomConverter.ToInt32("-2532424341"));
        }

        [Fact]
        public void CustomConverter_PositiveNumberLimitReached_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CustomConverter.ToInt32("2532424341"));
        }
    }
}
