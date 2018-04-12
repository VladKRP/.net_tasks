using System;
using System.Collections.Generic;
using System.Text;

namespace NoSql_Mongo.Models
{
    public class Book
    {
        public string  Name { get; set; }

        public string Author { get; set; }

        public int Count { get; set; }

        public IEnumerable<string> Genre { get; set; }

        public int Year { get; set; }
    }
}
