using NoSql_Mongo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NoSql_Mongo.Tests
{
    public class MongoDBTests
    {

        private readonly IEnumerable<Book> books = new List<Book>()
        {
            new Book(){ Name = "Hobit", Author = "Tolkien", Count = 5, Genre = new List<string>() { "fantasy" }, Year = 2014 },
            new Book(){ Name = "Lord of the rings", Author = "Tolkien", Count = 3, Genre = new List<string>() { "fantasy" }, Year = 2015 },
            new Book(){ Name = "Kolobok", Count = 10, Genre = new List<string>() { "kids" }, Year = 2000 },
            new Book(){ Name = "Repka", Count = 11, Genre = new List<string>() { "kids" }, Year = 2000 },
            new Book(){ Name = "Dyadya Stiopa", Author = "Mihalkov", Count = 1, Genre = new List<string>() { "kids" }, Year = 2001 }
        };

        [Fact]
        public async Task Test1()
        {


            MongoDBBookContext context = new MongoDBBookContext("Books");

            await context.InsertBooks(books);

            var booksWithoutAuthors = await context.GetBooksWithoutAuthors();

        }
    }
}
