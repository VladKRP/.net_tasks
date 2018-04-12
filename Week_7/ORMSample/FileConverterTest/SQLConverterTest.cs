using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlFileConverter;
using SqlFileConverter.SqlConverter;

namespace FileConverterTest
{
    [TestClass]
    public class SQLConverterTest
    {
        private const string _generatedDomainName = "ORM.Domain.Models";
        private readonly SqlConverter _sqlConverter;

        public SQLConverterTest()
        {
            _sqlConverter = new SqlConverter(_generatedDomainName, SQLTypes.MSSQLTypes);
        }

        [TestMethod]
        public void SqlConverter_Convert_UseQuotesSyntax_ReturnClassRepresentationOfTable()
        {
            string createScript = $"CREATE TABLE \"Categories\" (" +
                                 "\"CategoryID\" \"int\" IDENTITY(1, 1) NOT NULL," +
                                 "\"CategoryName\" nvarchar(15) NOT NULL," +
                                 "\"Description\" \"ntext\" NULL ," +
                                 "\"Picture\" \"image\" NULL )";

            var tablecs = _sqlConverter.Convert(createScript);

            Assert.IsNotNull(tablecs);
            Assert.AreEqual(_generatedDomainName, tablecs.Namespace);
            Assert.AreEqual("Category", tablecs.TableDefinition.TableName);
            Assert.AreEqual(4, tablecs.TableDefinition.Fields.Count());
            Assert.AreEqual("CategoryName", tablecs.TableDefinition.Fields.ElementAt(1).FieldName);
            Assert.AreEqual("string", tablecs.TableDefinition.Fields.ElementAt(1).FieldType);
        }

        [TestMethod]
        public void SqlConverter_Convert_UseSquareBracketSyntax_ReturnClassRepresentationOfTable()
        {
            string createScript = $"CREATE TABLE [Categories] (" +
                                 "[CategoryID] int IDENTITY(1, 1) NOT NULL," +
                                 "[CategoryName] nvarchar(15) NOT NULL," +
                                 "[Description] ntext NULL ," +
                                 "[Picture] image NULL )";

            var tablecs = _sqlConverter.Convert(createScript);

            Assert.IsNotNull(tablecs);
            Assert.AreEqual(_generatedDomainName, tablecs.Namespace);
            Assert.AreEqual("Category", tablecs.TableDefinition.TableName);
            Assert.AreEqual(4, tablecs.TableDefinition.Fields.Count());
            Assert.AreEqual("CategoryName", tablecs.TableDefinition.Fields.ElementAt(1).FieldName);
            Assert.AreEqual("string", tablecs.TableDefinition.Fields.ElementAt(1).FieldType);
        }

        [TestMethod]
        public void SqlConverter_Convert_dboTableNamePrefix_ReturnClassRepresentationOfTable()
        {
            string createScript = $"CREATE TABLE [dbo].[Categories] (" +
                                 "[CategoryID] int IDENTITY(1, 1) NOT NULL," +
                                 "[CategoryName] nvarchar(15) NOT NULL," +
                                 "[Description] ntext NULL ," +
                                 "[Picture] image NULL )";

            var tablecs = _sqlConverter.Convert(createScript);

            Assert.IsNotNull(tablecs);
            Assert.AreEqual(_generatedDomainName, tablecs.Namespace);
            Assert.AreEqual("Category", tablecs.TableDefinition.TableName);
            Assert.AreEqual(4, tablecs.TableDefinition.Fields.Count());
            Assert.AreEqual("CategoryName", tablecs.TableDefinition.Fields.ElementAt(1).FieldName);
            Assert.AreEqual("string", tablecs.TableDefinition.Fields.ElementAt(1).FieldType);
        }

        [TestMethod]
        public void SqlConverter_Convert_TableNameWithSpaces_ReturnClassRepresentaionOfTable()
        {
            string createScript = $"CREATE TABLE [dbo].[Products Orders] (" +
                                 "[OrderId] int NOT NULL," +
                                 "[ProductId] int NOT NULL)";

            var tablecs = _sqlConverter.Convert(createScript);

            Assert.IsNotNull(tablecs);
            Assert.AreEqual(_generatedDomainName, tablecs.Namespace);
            Assert.AreEqual("ProductsOrder", tablecs.TableDefinition.TableName);
            Assert.AreEqual(2, tablecs.TableDefinition.Fields.Count());
            Assert.AreEqual("OrderId", tablecs.TableDefinition.Fields.ElementAt(0).FieldName);
            Assert.AreEqual("int", tablecs.TableDefinition.Fields.ElementAt(0).FieldType);
        }



    }
}
