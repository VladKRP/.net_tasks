using SqlFileConverter;
using SQLTableToCSConvertUtility.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace SQLTableToCSConvertUtility
{
    class Program
    {
        static void Main(string[] args)
        {

            var converterConfigs = ConfigurationManager.GetSection("converterConfigs") as ConverterConfigurationSection;
            string outPath = converterConfigs.OuputFilesPath;

            IConverter<string, TableClassRepresentation> converter = 
                new SqlConverter(converterConfigs.DomainNamespace, SQLTypes.MSSQLTypes);
            DirectoryInfo directory = new DirectoryInfo(converterConfigs.SqlFilesPath);

            if (!new DirectoryInfo(outPath).Exists)
                Directory.CreateDirectory(outPath);

            foreach(var sqlDefinition in directory.GetFiles())
            {
                var cstable = converter.Convert(File.ReadAllText(sqlDefinition.FullName));
                using (StreamWriter streamWriter = new StreamWriter(outPath + cstable.TableDefinition.TableName + ".cs"))
                        streamWriter.Write(cstable);
            }
        }
    }
}
