using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BasicSerialization.Models
{
    [Serializable]
    [XmlRoot(ElementName = "catalog")]
    public class Catalog
    {
        [XmlAttribute(AttributeName = "date")]
        public DateTime Date { get; set; }
        [XmlElement(ElementName = "book")]
        public List<Book> Books { get; set; }

        public Catalog()
        {

        }
    }
}
