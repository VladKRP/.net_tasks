using SqlFileConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlFileConverter.SqlConverter
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

            var fieldsDescriptionBeforeComma = string.Join("", script.Where((x, i) => i > script.IndexOf('(') && i < script.LastIndexOf(')')))
                                                          .Split(new string[]{",\n",",\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(x => x.Trim());
           
            foreach (var fieldDescription in fieldsDescriptionBeforeComma)
            {
                

                TableField field = new TableField();

                var description = fieldDescription.Split(' ');
                for (int i = 0; i < description.Length; i++)
                {
                    string type = null;
                    var trimedDescription = GetTrimedString(description[i]);
                    var possibleType = string.Join("", trimedDescription.TakeWhile(x => x != '('));
                    if (i == 0 && !string.IsNullOrWhiteSpace(trimedDescription))
                        field.FieldName = trimedDescription;
                    else if (_types.TryGetValue(possibleType, out type))
                        field.FieldType = type;
                }

               

                if (!string.IsNullOrWhiteSpace(field.FieldName) && !string.IsNullOrWhiteSpace(field.FieldType))
                {
                    if (!IsNotNullField(fieldDescription) && IsValueType(field.FieldType))
                        field.FieldType += "?";
                    tableFields.Add(field);
                }
                    
            }
            return tableFields;
        }


        private bool IsNotNullField(string fieldDefinition)
        {
            return fieldDefinition.IndexOf("NOT NULL", StringComparison.CurrentCultureIgnoreCase) > 0;
        }

        private bool IsValueType(string fieldType)
        {
            bool isValueType = false;
            Type type = Type.GetType(fieldType);
            if (type.IsValueType)
                isValueType = true;
            return isValueType;

        }

        private string GetTrimedString(string str)
        {
            return str.Trim('\\', '\'', '\"', '"','\r','\n','\t','[',']');
        }


    }
}