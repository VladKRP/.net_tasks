using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoSql_Mongo.Models
{
    public class Book
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string  Name { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("genres")]
        public IEnumerable<string> Genre { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }
    }
}
