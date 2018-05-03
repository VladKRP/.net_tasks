using MongoDB.Bson;
using MongoDB.Driver;
using NoSql_Mongo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson.Serialization;

namespace NoSql_Mongo
{
    public class MongoDBBookRepository
    {
        private readonly MongoDBLibraryContext _context;

        public MongoDBBookRepository(MongoDBLibraryContext context)
        {
            _context = context;
        }

        public async Task InsertBooksAsync(IEnumerable<Book> books)
        {
            await _context.Books.InsertManyAsync(books);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var result = await _context.Books.FindAsync(Builders<Book>.Filter.Empty);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre)
        {
            var result = await _context.Books.FindAsync(book => string.Equals(book.Genre,genre));
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetTop3OrderedBookNamesWithBookCountMoreThan1()//clarify last requirement
        {
            return (await _context.Books.Find(book => book.Count > 1)
                                       .SortBy(book => book.Name)
                                       .Limit(3)
                                       .ToListAsync()).Select(x => x.Name);
        }


        public async Task<Book> GetBookWithMinCountAsync() {
            return await _context.Books.Find(Builders<Book>.Filter.Empty)
                                       .SortBy(book => book.Count).FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookWithMaxCountAsync()
        {

            return await _context.Books.Find(Builders<Book>.Filter.Empty)
                                       .SortByDescending(book => book.Count).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<string>> GetUniqueAuthorsAsync()
        {
            var uniqueAuthorsCursor = await _context.Books.DistinctAsync<string>("author", new BsonDocument());
            var uniqueAuthors = await uniqueAuthorsCursor.ToListAsync();
            uniqueAuthors.Remove(null);
            return uniqueAuthors;
        }

        public async Task<IEnumerable<Book>> GetBooksWithoutAuthors()
        {
            var booksCursor = await _context.Books.FindAsync(book => string.Equals(book.Author, null));
            return await booksCursor.ToListAsync();
        }

        public async Task IncreaseEachBookCountOnValue(int value)
        {
            await _context.Books.UpdateManyAsync(new BsonDocument(), Builders<Book>.Update.Inc(x => x.Count, value));
        }

        public async Task AddGenreToFantasyBooks(string genre)
        {
            const string fantasyGenre = "fantasy";
            var fantasyBooksFilter = Builders<Book>.Filter.Where(x => x.Genre.Contains(fantasyGenre) && !x.Genre.Contains(genre));
            await _context.Books.UpdateManyAsync(fantasyBooksFilter, Builders<Book>.Update.Push(x => x.Genre, genre));
        }

        public async Task DeleteBooksWithCountLessThanValueAsync(int value)
        {
            await _context.Books.DeleteManyAsync(book => book.Count < value);
        }

        public async Task DeleteAllBooksAsync()
        {
            await _context.Books.DeleteManyAsync<Book>(x => x.Id != null);
        }
    }
}
