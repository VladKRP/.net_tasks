using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CustomConverter.Tests
{
    public class CustomConverterTests
    {
        [Theory]
        [InlineData("12", 12)]
        [InlineData("1000", 1000)]
        [InlineData("-10000", -10000)]
        [InlineData("253242434", 253242434)]
        public void CustomConverter_ReturnConvertedString(string str, decimal expected)
        {
            Assert.Equal(expected, CustomConverter.ToInt32(str));
        }

        [Theory]
        [InlineData(" 123", 123)]
        [InlineData(" 123 ", 123)]
        [InlineData(" -123 ", -123)]
        public void CustomConverter_PassStringWithNumberAndSpaces_ReturnNumber(string str, int expected)
        {
            Assert.Equal(expected, CustomConverter.ToInt32(str));
        }

    }
}
