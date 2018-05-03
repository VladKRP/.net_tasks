using NoSql_Mongo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NoSql_Mongo.Tests
{
    public class MongoDBTests
    {

        private readonly IEnumerable<Book> _books = new List<Book>()
        {
            new Book(){ Name = "Hobit", Author = "Tolkien", Count = 5, Genre = new List<string>() { "fantasy" }, Year = 2014 },
            new Book(){ Name = "Lord of the rings", Author = "Tolkien", Count = 3, Genre = new List<string>() { "fantasy" }, Year = 2015 },
            new Book(){ Name = "Kolobok", Count = 10, Genre = new List<string>() { "kids" }, Year = 2000 },
            new Book(){ Name = "Repka", Count = 11, Genre = new List<string>() { "kids" }, Year = 2000 },
            new Book(){ Name = "Dyadya Stiopa", Author = "Mihalkov", Count = 1, Genre = new List<string>() { "kids" }, Year = 2001 }
        };

        private readonly MongoDBLibraryContext _context;
        private readonly MongoDBBookRepository _bookRepository;

        public MongoDBTests()
        {
            _context = new MongoDBLibraryContext("Library");
            _bookRepository = new MongoDBBookRepository(_context);
        }

        [Fact]
        public async Task MongoDBBookRepository_GetBooksAsync_Test()
        {
            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var insertedBooks = await _bookRepository.GetBooksAsync();

            Assert.NotEmpty(insertedBooks);
            Assert.Equal(_books.Count(), insertedBooks.Count());
            Assert.Equal(_books.ElementAt(1).Author, insertedBooks.ElementAt(1).Author);
        }

        [Fact]
        public async Task MongoDBBookRepository_InsertBooksAsync_Test()
        {
            await _bookRepository.DeleteAllBooksAsync();

            var books = await _bookRepository.GetBooksAsync();

            Assert.Empty(books);

            await _bookRepository.InsertBooksAsync(_books);

            var insertedBooks =  await _bookRepository.GetBooksAsync();

            Assert.NotEmpty(insertedBooks);
            Assert.Equal(_books.Count(), insertedBooks.Count());
            Assert.Equal(_books.ElementAt(1).Author, insertedBooks.ElementAt(1).Author);
        }

        [Fact]
        public async Task MongoDBBookRepository_GetTop3OrderedBookNamesWithBookCountMoreThan1_Test()
        {
            const int expectedCount = 3;

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var books = await _bookRepository.GetTop3OrderedBookNamesWithBookCountMoreThan1();

            Assert.NotEmpty(books);
            Assert.Equal(expectedCount, books.Count());
        }

        [Fact]
        public async Task MongoDBBookRepository_GetBooksWithoutAuthor_Test()
        {
            const int booksWithoutAuthorsAmount = 2;

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var books = await _bookRepository.GetBooksWithoutAuthors();

            Assert.NotEmpty(books);
            Assert.Equal(booksWithoutAuthorsAmount, books.Count());

            books.ToList().ForEach(book => Assert.Null(book.Author));
        }

        [Fact]
        public async Task MongoDBBookRepository_GetUniqueAuthorsAsync_Test()
        {
            const int uniqueAuthorsAmount = 2;

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var authorsName = await _bookRepository.GetUniqueAuthorsAsync();

            Assert.NotEmpty(authorsName);
            Assert.Equal(uniqueAuthorsAmount, authorsName.Count());
            Assert.Equal(authorsName, authorsName.Distinct());
        }

        [Fact]
        public async Task MongoDBBookRepository_DeleteBooksWithCountLessThanValueAsync_Test()
        {
            const int amountOfBooksWithCountMoreThanSix = 2;

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var books = await _bookRepository.GetBooksAsync();

            Assert.Equal(_books.Count(), books.Count());

            await _bookRepository.DeleteBooksWithCountLessThanValueAsync(6);

            books = await _bookRepository.GetBooksAsync();

            Assert.Equal(amountOfBooksWithCountMoreThanSix, books.Count());
        }

        [Fact]
        public async Task MongoDBBookRepository_DeleteAllBooksAsync_Test()
        {
            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var books = await _bookRepository.GetBooksAsync();

            Assert.NotEmpty(books);

            await _bookRepository.DeleteAllBooksAsync();

            books = await _bookRepository.GetBooksAsync();

            Assert.Empty(books);
        }

        [Fact]
        public async Task MongoDBBookRepository_IncreaseAllBooksCountAsync_IncreaseBy1_Test()
        {
            const int increaseValue = 1;

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            await _bookRepository.IncreaseEachBookCountOnValue(increaseValue);

            var books = await _bookRepository.GetBooksAsync();

            var isBooksCountEqual = _books.Zip(books, (expected, actual) => expected.Count + increaseValue == actual.Count).All(x => x == true);

            Assert.True(isBooksCountEqual);
        }

        [Fact]
        public async Task MongoDBBookRepository_AddGenreToFantasyBooks_Test()
        {
            const string favorityGenre = "favority";
            const string fantasyGenre = "fantasy";

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            await _bookRepository.AddGenreToFantasyBooks(favorityGenre);

            var fantasyBooks = (await _bookRepository.GetBooksByGenreAsync(fantasyGenre)).ToList();

            fantasyBooks.ForEach(book => Assert.Contains(favorityGenre, book.Genre));

            fantasyBooks = (await _bookRepository.GetBooksByGenreAsync(fantasyGenre)).ToList();

            await _bookRepository.AddGenreToFantasyBooks(favorityGenre);

            fantasyBooks.ForEach(book => Assert.Single(book.Genre.Where(genre => string.Equals(genre, favorityGenre))));

        }

        [Fact]
        public async Task MongoDBBookRepository_GetBookWithMinCount_Test()
        {
            const int expectedCount = 1;

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var book = await _bookRepository.GetBookWithMinCountAsync();

            Assert.NotNull(book);
            Assert.Equal(expectedCount, book.Count);

        }

        [Fact]
        public async Task MongoDBBookRepository_GetBookWithMaxCount_Test()
        {
            const int expectedCount = 11;

            await _bookRepository.DeleteAllBooksAsync();
            await _bookRepository.InsertBooksAsync(_books);

            var book = await _bookRepository.GetBookWithMaxCountAsync();

            Assert.NotNull(book);
            Assert.Equal(expectedCount, book.Count);
        }
    }
}
