using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Mongo.Common;
using Mongo.CRUD.Models;

namespace Mongo.Access.ViewModel
{
    public class HttpClientSample : Notifier
    {
        private readonly Serilog.ILogger? _logger;
        private Book? _selectedItem;

        public Book? SelectedItem { get { return _selectedItem; } set { _selectedItem = value; OnPropertyChanged(); } }

        public ObservableCollection<Book> BookList { get; set; } = new();

        public Command SearchCommand { get; }

        public HttpClient httpClient { get; }

        public HttpClientSample(Serilog.ILogger logger) 
        {
            _logger = logger;
            httpClient = new HttpClient();

            SearchCommand = new Command(RunAsync);
        }

        public void Showbook(Book book)
        {
            Console.WriteLine($"Name: {book.BookName}\tPrice: " +
                $"{book.Price}\tCategory: {book.Category}");
        }

        public async Task<Uri> CreatebookAsync(Book book)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "api/books", book);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        public async Task<Book> GetbookAsync(string path)
        {
            Book book = null;
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                book = await response.Content.ReadFromJsonAsync<Book>();
            }
            return book;
        }

        public async Task<Book> UpdatebookAsync(Book book)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(
                $"api/books/{book.Id}", book);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated book from the response body.
            book = await response.Content.ReadFromJsonAsync<Book>();
            return book;
        }

        public async Task<HttpStatusCode> DeletebookAsync(string id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(
                $"api/books/{id}");
            return response.StatusCode;
        }
        public async void RunAsync(object? obj = null)
        {
            // Update port # in the following line.
            httpClient.BaseAddress = new Uri("http://localhost:7121/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new book
                Book book = new Book
                {
                    BookName = "Gizmo",
                    Price = 100,
                    Category = "Widgets"
                };

                book = await GetbookAsync("api/Books");

                var url = await CreatebookAsync(book);
                Console.WriteLine($"Created at {url}");

                // Get the book
                book = await GetbookAsync(url.PathAndQuery);
                Showbook(book);

                // Update the book
                Console.WriteLine("Updating price...");
                book.Price = 80;
                await UpdatebookAsync(book);

                // Get the updated book
                book = await GetbookAsync(url.PathAndQuery);
                Showbook(book);

                // Delete the book
                var statusCode = await DeletebookAsync(book.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }
    }
}
