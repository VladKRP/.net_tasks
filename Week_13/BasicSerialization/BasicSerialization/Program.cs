using BasicSerialization.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BasicSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            Catalog catalog;

            string resourcesDirectory = Directory.GetCurrentDirectory() + @"..\..\..\Resources\";
            string initialFilePath = resourcesDirectory + "books.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
            using (var stream = new FileStream(initialFilePath, FileMode.Open))
            {
                catalog = serializer.Deserialize(stream) as Catalog;
            }

            if(catalog != null)
            {
                Book book = new Book() { Id = "1231", Publisher = "David Allen" };

                catalog.Books.Add(book);

                string serializedFilePath = resourcesDirectory + "booksSerialized.xml";

                using (var stream = new FileStream(serializedFilePath, FileMode.Create))
                {
                    serializer.Serialize(stream, catalog);
                }
            }
                  
        }
    }
}
