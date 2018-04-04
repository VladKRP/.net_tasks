using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CustomConverter
{
    public class CustomConverter
    {
        public static int ToInt32(string str)
        {
            if (str == null)
                throw new ArgumentNullException();

            str = str.Trim();

            if (str == "")
                throw new ArgumentException("Can not parse empty string to number");

            bool isNegativeNumber = IsNegative(str);
            int nonDigitCharacters = CountNonDigitCharacters(str);

            if (isNegativeNumber && nonDigitCharacters != 1 || !isNegativeNumber && nonDigitCharacters > 0)
                throw new CustomConverterException(str, "Integer number can't contain letters or punctuation");

            long number;
            if (!TryParse(str, isNegativeNumber, out number))
            {
                if (isNegativeNumber)
                    throw new ArgumentOutOfRangeException(str, "The number exceed Int type right limit");
                else
                    throw new ArgumentOutOfRangeException(str, "The number exceed Int type left limit");
            }

            return (int)number;
        }

        private static bool TryParse(IEnumerable<char> str, bool isNegative, out long number)
        {
            long result = 0;

            if (isNegative)
            {
                str = str.Skip(1);
            }

            for (int j = str.Count() - 1; j >= 0; j--)
            {
                result += result * 10 + (int)(char.GetNumericValue(str.ElementAt(j)));
                if (result > int.MaxValue)
                {
                    if (isNegative)
                    {
                        result = result * (-1) - 1;
                        if (result < int.MinValue)
                        {
                            number = int.MinValue;
                            return false;
                        }
                    }
                    number = int.MaxValue;
                    return false;
                }
            }

            if (isNegative)
                result *= -1;

            number = result;
            return true;
        }


        private static bool IsNegative(IEnumerable<char> str)
        {
            return str.FirstOrDefault() == '-';
        }

        private static int CountNonDigitCharacters(IEnumerable<char> str)
        {
            return str.Count(c => !char.IsDigit(c));
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

