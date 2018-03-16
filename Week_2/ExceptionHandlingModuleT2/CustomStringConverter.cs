using System;
using System.Linq;

namespace ExceptionHandlingModuleT2
{
    public static class CustomStringConverter
    {
        public static int ToInt32(string num){
            
            if (num.All(c => char.IsNumber(c)))
            {
                return Convert.ToInt32(num);
            }
            else if(Convert.ToInt64(num) > int.MaxValue)//too large value for int32
            {
                throw new Exception();
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
