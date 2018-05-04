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
            const string filePath = @"D:\CDP\.net_tasks\Week_13\BasicSerialization\BasicSerialization\Resources\books.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var result = serializer.Deserialize(stream);
            }
                
        }
    }
}
