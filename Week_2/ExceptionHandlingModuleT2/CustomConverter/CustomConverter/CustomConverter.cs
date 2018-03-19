using System;
using System.Linq;

namespace CustomConverter
{
    public class CustomConverter
    {
        public static int ToInt32(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new CustomConverterException(str, "Can not parse null or empty string");
            else if (str.First().Equals("-") || char.IsNumber(str.First()) && str.Skip(1).Any(ch => !char.IsDigit(ch)))
                throw new CustomConverterException(str, "Integer number can't contain letters or punctuation");
            else if (Convert.ToInt64(str) > int.MaxValue)
                throw new ArgumentOutOfRangeException(str, "The number exceed Int type right limit");
            else if (Convert.ToInt64(str) < int.MinValue)
                throw new ArgumentOutOfRangeException(str, "The number exceed Int type left limit");
            else
            {
                return Convert.ToInt32(str);
            }
        }
    }

    public class CustomConverterException : Exception
    {

        public object PassedValue { get; private set; }

        public CustomConverterException(string message) : base(message) { }
        public CustomConverterException(object passedValue, string message) : base(message)
        {
            PassedValue = passedValue;
        }

    }
}

