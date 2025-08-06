using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public bool Available { get; set; } = true;
    }

    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks();
        Book? GetBookById(int id);
        Book? GetBookByISBN(string isbn);
        void AddBook(Book book);
        void RemoveBook(int id);
        void UpdateBook(Book book);
        IEnumerable<Book> SearchBooks(string searchTerm);
    }

    public interface ILibraryService
    {
        void AddBook(string title, string author, string isbn);
        void RemoveBook(int id);
        void CheckOutBook(int id);
        void ReturnBook(int id);
        void DisplayAllBooks();
        void SearchBooks(string searchTerm);
        void DisplayBookDetails(int id);
    }

    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();
        private int _nextId = 1;

        public InMemoryBookRepository()
        {
            // Add some sample books
            AddBook(new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0743273565" });
            AddBook(new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "978-0446310789" });
            AddBook(new Book { Title = "1984", Author = "George Orwell", ISBN = "978-0451524935" });
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _books.AsReadOnly();
        }

        public Book? GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public Book? GetBookByISBN(string isbn)
        {
            return _books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public void AddBook(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
        }

        public void RemoveBook(int id)
        {
            var book = GetBookById(id);
            if (book != null)
            {
                _books.Remove(book);
            }
        }

        public void UpdateBook(Book book)
        {
            var existingBook = GetBookById(book.Id);
            if (existingBook != null)
            {
                var index = _books.IndexOf(existingBook);
                _books[index] = book;
            }
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return _books.Where(b => 
                b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                b.ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;

        public LibraryService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void AddBook(string title, string author, string isbn)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("Title, author, and ISBN are required.");
            }

            if (_bookRepository.GetBookByISBN(isbn) != null)
            {
                throw new InvalidOperationException("A book with this ISBN already exists.");
            }

            var book = new Book
            {
                Title = title,
                Author = author,
                ISBN = isbn,
                Available = true
            };

            _bookRepository.AddBook(book);
            Console.WriteLine($"Book '{title}' by {author} has been added to the library.");
        }

        public void RemoveBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            _bookRepository.RemoveBook(id);
            Console.WriteLine($"Book '{book.Title}' has been removed from the library.");
        }

        public void CheckOutBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            if (!book.Available)
            {
                throw new InvalidOperationException("Book is already checked out.");
            }

            book.Available = false;
            _bookRepository.UpdateBook(book);
            Console.WriteLine($"Book '{book.Title}' has been checked out.");
        }

        public void ReturnBook(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            if (book.Available)
            {
                throw new InvalidOperationException("Book is already available.");
            }

            book.Available = true;
            _bookRepository.UpdateBook(book);
            Console.WriteLine($"Book '{book.Title}' has been returned.");
        }

        public void DisplayAllBooks()
        {
            var books = _bookRepository.GetAllBooks();
            Console.WriteLine("\n=== Library Catalog ===");
            Console.WriteLine($"{"ID",-3} {"Available",-10} {"Title",-25} {"Author",-20} {"ISBN",-15}");
            Console.WriteLine(new string('-', 80));

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Id,-3} {(book.Available ? "Yes" : "No"),-10} {book.Title,-25} {book.Author,-20} {book.ISBN,-15}");
            }
        }

        public void SearchBooks(string searchTerm)
        {
            var books = _bookRepository.SearchBooks(searchTerm);
            Console.WriteLine($"\n=== Search Results for '{searchTerm}' ===");
            
            if (!books.Any())
            {
                Console.WriteLine("No books found.");
                return;
            }

            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author}, Available: {(book.Available ? "Yes" : "No")}");
            }
        }

        public void DisplayBookDetails(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.WriteLine($"\n=== Book Details ===");
            Console.WriteLine($"ID: {book.Id}");
            Console.WriteLine($"Title: {book.Title}");
            Console.WriteLine($"Author: {book.Author}");
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine($"Available: {(book.Available ? "Yes" : "No")}");
        }
    }

    public class LibraryApp
    {
        private readonly ILibraryService _libraryService;

        public LibraryApp(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        public void Run()
        {
            Console.WriteLine("=== Library Management System ===");
            Console.WriteLine("Commands: add, remove, checkout, return, list, search, details, quit");

            while (true)
            {
                Console.Write("\nEnter command: ");
                string command = Console.ReadLine()?.ToLower() ?? string.Empty;

                try
                {
                    switch (command)
                    {
                        case "add":
                            AddBook();
                            break;
                        case "remove":
                            RemoveBook();
                            break;
                        case "checkout":
                            CheckOutBook();
                            break;
                        case "return":
                            ReturnBook();
                            break;
                        case "list":
                            _libraryService.DisplayAllBooks();
                            break;
                        case "search":
                            SearchBooks();
                            break;
                        case "details":
                            DisplayBookDetails();
                            break;
                        case "quit":
                            return;
                        default:
                            Console.WriteLine("Unknown command.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void AddBook()
        {
            Console.Write("Enter title: ");
            string title = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter author: ");
            string author = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter ISBN: ");
            string isbn = Console.ReadLine() ?? string.Empty;

            _libraryService.AddBook(title, author, isbn);
        }

        private void RemoveBook()
        {
            Console.Write("Enter book ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _libraryService.RemoveBook(id);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void CheckOutBook()
        {
            Console.Write("Enter book ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _libraryService.CheckOutBook(id);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void ReturnBook()
        {
            Console.Write("Enter book ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _libraryService.ReturnBook(id);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void SearchBooks()
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine() ?? string.Empty;
            _libraryService.SearchBooks(searchTerm);
        }

        private void DisplayBookDetails()
        {
            Console.Write("Enter book ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                _libraryService.DisplayBookDetails(id);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Set up dependency injection
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IBookRepository, InMemoryBookRepository>();
                    services.AddScoped<ILibraryService, LibraryService>();
                    services.AddScoped<LibraryApp>();
                })
                .Build();

            // Get the service and run the application
            var app = host.Services.GetRequiredService<LibraryApp>();
            app.Run();
        }
    }
} 