using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFileConverter.SqlConverter
{
    public static class SQLTypes
    {
        public static IDictionary<string, string> MSSQLTypes = new Dictionary<string, string>
        {
            {"bit", "System.Boolean"},
            {"char", "System.Char"},
            {"varchar", "System.String"},
            {"text", "System.String"},
            {"nvarchar", "System.String"},
            {"nchar", "System.String"},
            {"ntext", "System.String"},
            {"binary", "System.Byte[]"},
            {"varbinary", "System.Byte[]"},
            {"image", "System.Byte[]"},
            {"date", "System.DateTime"},
            {"datetime", "System.DateTime"},
            {"datetime2", "System.DateTime"},
            {"datetimeoffset", "System.DateTimeOffset"},
            {"time", "System.TimeSpan" },
            {"int", "System.Int32"},
            {"smallint", "System.Int32"},
            {"bigint","System.Int64" },
            {"tinyint", "System.Int32"},
            {"float", "System.Single" },
            {"real", "System.Single" },
            {"smallmoney", "System.Decimal" } ,
            {"money", "System.Decimal" } ,
            {"decimal", "System.Decimal" }
        };

    }

}

