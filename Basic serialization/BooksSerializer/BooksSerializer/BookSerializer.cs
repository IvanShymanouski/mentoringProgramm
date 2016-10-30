using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BooksSerializer.Entities;

namespace BooksSerializer
{
    public class BookSerializer
    {
        public void Write(string filePath, Catalog element)
        {
            var serializer = new XmlSerializer(typeof(Catalog));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, element);
            }
        }

        public Catalog Read(string filePath)
        {
            var serializer = new XmlSerializer(typeof(Catalog));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return (Catalog)serializer.Deserialize(fs);
            }
        }
    }
}
