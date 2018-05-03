using MongoDB.Bson;
using MongoDB.Driver;
using NoSql_Mongo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql_Mongo
{
    public class MongoDBLibraryContext
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDBLibraryContext(string databaseName) {
             _client = new MongoClient();
            _database = _client.GetDatabase(databaseName);
        }

        public MongoDBLibraryContext(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public MongoDBLibraryContext(IMongoClient client, string databaseName)
        {
            _client = client;
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoCollection<Book> Books
        {
            get
            {
                return _database.GetCollection<Book>("books");
            }
       }
    }
}



