using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlFileConverter
{
    public class SqlConverter : IConverter<string, TableClassRepresentation>
    {

        private readonly string _namespace;
        private readonly IDictionary<string, string> _types;

        public SqlConverter() { }

        public SqlConverter(string nameSpace, IDictionary<string, string> typesDictionary)
        {
            _namespace = nameSpace;
            _types = typesDictionary;
        }


        public TableClassRepresentation Convert(string source)
        {
            TableDefinition tableDefinition = new TableDefinition()
            {
                TableName = TableNameFormatter.Format(GetSqlTableName(source)?.Replace(" ", "")),
                Fields = GetSqlTableFields(source)
            };

            TableClassRepresentation tableClassRepresentation = new TableClassRepresentation(_namespace, tableDefinition);
            return tableClassRepresentation;
        }

        private string GetSqlTableName(string script)
        {
            var result = new Regex("CREATE TABLE ([\"\\[]\\w+[\"\\]].)*([\"\\[](\\w+\\u0020?\\w+?)[\\]\"])").Match(script);
            if (result.Groups?.Count > 1)
                return result.Groups[result.Groups.Count - 1].Value;
            return null;
        }

        private IEnumerable<TableField> GetSqlTableFields(string script)
        {
            ICollection<TableField> tableFields = new List<TableField>();
            var fieldsDescription = string.Join("", script.Where((x, i) => i > script.IndexOf('(') && i < script.LastIndexOf(')')))
                                                          .Split(',')
                                                          .Select(x => x.Trim())
                                                          .Select(x => x.Split(' '));

            foreach (var fieldDescription in fieldsDescription)
            {
                TableField field = new TableField();
                for (int i = 0; i < fieldDescription.Length; i++)
                {
                    string type = null;
                    var trimedDescription = GetTrimedString(fieldDescription[i]);
                    var possibleType = string.Join("", trimedDescription.TakeWhile(x => x != '('));
                    if (i == 0 && !string.IsNullOrWhiteSpace(trimedDescription))
                        field.FieldName = trimedDescription;
                    else if (_types.TryGetValue(possibleType, out type))
                        field.FieldType = type;
                }

                if (!string.IsNullOrWhiteSpace(field.FieldName) && !string.IsNullOrWhiteSpace(field.FieldType))
                    tableFields.Add(field);
            }
            return tableFields;
        }

        private string GetTrimedString(string str)
        {
            return str.Trim('\\', '\'', '\"', '"','\r','\n','\t','[',']');
        }


    }

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

    public class TableDefinition
    {
        public string TableName { get; set; }

        public IEnumerable<TableField> Fields { get; set; }
    }

    public class TableField
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
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