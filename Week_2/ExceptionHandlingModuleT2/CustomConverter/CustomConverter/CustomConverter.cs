using System;
using System.Linq;
using System.Runtime.Serialization;

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

    [Serializable]
    public class CustomConverterException : ApplicationException
    {

        public string PassedValue { get; private set; }

        public CustomConverterException(string message) : base(message) { }
        public CustomConverterException(string message, Exception innerException) : base(message, innerException) { }
        public CustomConverterException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context) { }
        public CustomConverterException(string passedValue, string message) : base(message)
        {
            PassedValue = passedValue;
        }

    }
}

