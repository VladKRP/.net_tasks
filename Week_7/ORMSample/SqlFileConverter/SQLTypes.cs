using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFileConverter
{
    public static class SQLTypes
    {
        public static IDictionary<string, string> MSSQLTypes = new Dictionary<string, string>
        {
            {"bit", "bool"},
            {"char", "string"},
            {"varchar", "string"},
            {"text", "string"},
            {"nvarchar", "string"},
            {"nchar", "string"},
            {"ntext", "string"},
            {"binary", "byte[]"},
            {"varbinary", "byte[]"},
            {"image", "byte[]"},
            {"date", "DateTime"},
            {"datetime", "DateTime"},
            {"datetime2", "DateTime"},
            {"datetimeoffset", "DateTimeOffset"},
            {"time", "TimeSpan" },
            {"int", "int"},
            {"smallint", "int"},
            {"bigint","long" },
            {"tinyint", "int"},
            {"float", "float" },
            {"real", "float" },
            {"smallmoney", "decimal" } ,
            {"money", "decimal" } ,
            {"decimal", "decimal" }
        };

    }

}

