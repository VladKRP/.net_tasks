using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlFileConverter.Models
{
    public class TableClassRepresentation
    {
        public TableClassRepresentation(string nameSpace, TableDefinition tableDefinition)
        {
            Namespace = nameSpace;
            TableDefinition = tableDefinition;
        }

        public string Namespace { get; set; }

        public TableDefinition TableDefinition { get; set; }

        public override string ToString()
        {
            string libs = null;

            if (TableDefinition.Fields.Any(x => x.FieldType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase)))
                libs = "using System;";

            StringBuilder sb = new StringBuilder(libs + "\r\n\r\nnamespace " + Namespace +
                            " { \r\n\r\n\tpublic class " + TableDefinition.TableName + " {\r\n\r\n");


            foreach (var field in TableDefinition.Fields)
            {
                sb.Append("\t\tpublic " + field.FieldType + " " + field.FieldName + " { get; set; }\r\n\r\n");
            }

            sb.Append("\t}\r\n\r\n}");
            return sb.ToString();
        }
    }

    public static class TableNameFormatter
    {
        public static string Format(string name)
        {
            if (name.EndsWith("ies"))
                name = name.Substring(0, name.Length - "ies".Length) + "y";
            else if (name.EndsWith("s"))
                name = name.Substring(0, name.Length - 1);
            return name;
        }
    }
}



