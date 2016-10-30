using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using BooksSerializer.Entities;

namespace BooksSerializer
{
    class Program
    {
        private const string FileName = "Books.xml";
        private const string FileNameForWtite = "MyBooks.xml";

        static void Main(string[] args)
        {
            var serializer = new BookSerializer();
            Catalog res = serializer.Read(FileName);
            serializer.Write(FileNameForWtite, res);
        }
    }
}
