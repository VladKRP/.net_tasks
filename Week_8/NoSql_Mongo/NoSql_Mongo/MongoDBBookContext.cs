using MongoDB.Driver;
using NoSql_Mongo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql_Mongo
{
    public class MongoDBBookContext
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDBBookContext(string databaseName) {
             _client = new MongoClient();
            _database = _client.GetDatabase(databaseName);
        }

        public MongoDBBookContext(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public MongoDBBookContext(IMongoClient client, string databaseName)
        {
            _client = client;
            _database = _client.GetDatabase(databaseName);
        }

        public async Task InsertBooks(IEnumerable<Book> books)
        {
             await _database.GetCollection<Book>("books").InsertManyAsync(books);
        }

        public Book GetBookWithMaxCount()
        {
            var filter = Builders<Book>.Filter.Exists(x => x.Author, false);
            //SortDefinition<Book> filter = Builders<Book>.Sort.Descending(x => x.Count);
            //_database.GetCollection<Book>("books").FindAsync<Book>(book => book.Count);
            throw new NotImplementedException();
        }

        public Book GetBookWithMinCount() { throw new NotImplementedException(); }

        public IEnumerable<string> GetUniqueAuthors() { throw new NotImplementedException(); }

        public async Task<IEnumerable<Book>> GetBooksWithoutAuthors()
        {
            var filter = Builders<Book>.Filter.Exists(x => x.Author, false);
            var books = await _database.GetCollection<Book>("books").FindAsync<Book>(filter);
            return await books.ToListAsync();
        }

        public void IncreaseEachBookCountOnValue(IEnumerable<Book> books, int value) { }

        public void AddGenreToFantasyBooks(string genre) { }

        public void DeleteBooksWithCountLessThanValue(int value) { }

        public void DeleteAllBooks() { }

    }


}



