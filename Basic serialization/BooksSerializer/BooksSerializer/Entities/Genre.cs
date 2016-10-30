using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BooksSerializer.Entities
{
    public enum Genre
    {
        Computer,
        Fantasy,
        [XmlEnum(Name = "Science Fiction")]
        ScienceFiction,
        Horror,
        Romance,
    }
}
