using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BooksSerializer.Entities
{
    [XmlRoot("catalog", Namespace = @"http://library.by/catalog")]
    public class Catalog
    {
        [XmlElement(ElementName = "book")]
        public List<Book> Books { get; set; }

        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute(AttributeName = "date")]
        public string SerializableDate
        {
            get
            {
                return Date.ToString("yyyy-MM-dd");
            }

            set
            {
                Date = DateTime.Parse(value);
            }
        }
    }
}
