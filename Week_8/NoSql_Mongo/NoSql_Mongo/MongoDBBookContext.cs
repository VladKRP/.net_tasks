using NoSql_Mongo.Models;
using System;
using System.Collections.Generic;

namespace NoSql_Mongo
{
    public class MongoDBBookContext
    {
        private readonly ICollection<Book> books = new List<Book>()
        {
            new Book(){ Name = "Hobit", Author = "Tolkien", Count = 5, Genre = new List<string>() { "fantasy" }, Year = 2014 },
            new Book(){ Name = "Lord of the rings", Author = "Tolkien", Count = 3, Genre = new List<string>() { "fantasy" }, Year = 2015 },
            new Book(){ Name = "Kolobok", Count = 10, Genre = new List<string>() { "kids" }, Year = 2000 },
            new Book(){ Name = "Repka", Count = 11, Genre = new List<string>() { "kids" }, Year = 2000 },
            new Book(){ Name = "Dyadya Stiopa", Author = "Mihalkov", Count = 1, Genre = new List<string>() { "kids" }, Year = 2001 }
        };

        public Book GetBookWithMaxCount() { throw new NotImplementedException(); }

        public Book GetBookWithMinCount() { throw new NotImplementedException(); }

        public IEnumerable<string> GetUniqueAuthors() { throw new NotImplementedException(); }

        public IEnumerable<Book> GetBooksWithoutAuthors() { throw new NotImplementedException(); }

        public void IncreaseEachBookCountOnValue(IEnumerable<Book> books, int value) { }

        public void AddGenreToFantasyBooks(string genre) { }

        public void DeleteBooksWithCountLessThanValue(int value) { }

        public void DeleteAllBooks() { }

    }


}



