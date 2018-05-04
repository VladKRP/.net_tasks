using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BasicSerialization.Models
{
    [Serializable]
    [XmlType(TypeName = "book")]
    public class Book
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "isbn")]
        public string Isbn { get; set; }
        [XmlElement(ElementName = "author")]
        public string Author { get; set; }
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        //Little cheat
        [XmlElement(ElementName = "genre")]
        public string _genre;

        [XmlIgnore]
        public Genre Genre
        {
            get
            {
                var genreWithoutSpaces = _genre?.Replace(" ", "");
                if (Genre.TryParse(genreWithoutSpaces, out Genre genre))
                    return genre;
                else
                    return Genre.None;
            }
        }
        //

        //[XmlElement(ElementName = "genre")]
        //public Genre Genre { get; set; }

        [XmlElement(ElementName = "publisher")]
        public string Publisher { get; set; }
        [XmlElement(ElementName = "publish_date")]
        public DateTime PublishDate { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "registration_date")]
        public DateTime RegistrationDate { get; set; }


        public Book()
        {

        }
    }
}
