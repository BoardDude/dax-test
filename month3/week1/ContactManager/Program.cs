using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ContactManager.Data;
using ContactManager.Services;
using ContactManager.Models;

namespace ContactManager
{
    public class ContactManagerApp
    {
        private readonly IContactRepository _contactRepository;

        public ContactManagerApp(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("=== Contact Manager with SQL Server ===");
            Console.WriteLine("Commands: list, add, edit, delete, search, category, birthdays, stats, quit");

            while (true)
            {
                Console.Write("\nEnter command: ");
                string command = Console.ReadLine()?.ToLower() ?? string.Empty;

                try
                {
                    switch (command)
                    {
                        case "list":
                            await ListContactsAsync();
                            break;
                        case "add":
                            await AddContactAsync();
                            break;
                        case "edit":
                            await EditContactAsync();
                            break;
                        case "delete":
                            await DeleteContactAsync();
                            break;
                        case "search":
                            await SearchContactsAsync();
                            break;
                        case "category":
                            await ListContactsByCategoryAsync();
                            break;
                        case "birthdays":
                            await ShowBirthdaysAsync();
                            break;
                        case "stats":
                            await ShowStatisticsAsync();
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

        private async Task ListContactsAsync()
        {
            var contacts = await _contactRepository.GetAllContactsAsync();
            Console.WriteLine($"\n=== All Contacts ({contacts.Count()}) ===");
            Console.WriteLine($"{"ID",-3} {"Name",-20} {"Email",-25} {"Phone",-12} {"City",-15} {"Category",-10}");
            Console.WriteLine(new string('-', 90));

            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.Id,-3} {contact.Name,-20} {contact.Email,-25} {contact.Phone ?? "N/A",-12} {contact.City ?? "N/A",-15} {contact.Category,-10}");
            }
        }

        private async Task AddContactAsync()
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine() ?? string.Empty;
            
            Console.Write("Enter email: ");
            string email = Console.ReadLine() ?? string.Empty;
            
            Console.Write("Enter phone (optional): ");
            string phone = Console.ReadLine() ?? string.Empty;
            
            Console.Write("Enter city (optional): ");
            string city = Console.ReadLine() ?? string.Empty;
            
            Console.Write("Enter category (Family/Work/Friends/Other): ");
            string categoryStr = Console.ReadLine() ?? string.Empty;
            
            Console.Write("Enter birthday (MM/DD/YYYY, optional): ");
            string birthdayStr = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Name and email are required.");
                return;
            }

            if (!Enum.TryParse<ContactCategory>(categoryStr, true, out var category))
            {
                category = ContactCategory.Other;
            }

            DateTime? birthday = null;
            if (!string.IsNullOrWhiteSpace(birthdayStr) && DateTime.TryParse(birthdayStr, out var parsedBirthday))
            {
                birthday = parsedBirthday;
            }

            var contact = new Contact
            {
                Name = name,
                Email = email,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                City = string.IsNullOrWhiteSpace(city) ? null : city,
                Category = category,
                Birthday = birthday
            };

            await _contactRepository.AddContactAsync(contact);
            Console.WriteLine($"Contact '{name}' has been added successfully.");
        }

        private async Task EditContactAsync()
        {
            Console.Write("Enter contact ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var contact = await _contactRepository.GetContactByIdAsync(id);
            if (contact == null)
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine($"Editing contact: {contact.Name}");
            Console.Write("Enter new name (or press Enter to keep current): ");
            string name = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(name))
                contact.Name = name;

            Console.Write("Enter new email (or press Enter to keep current): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(email))
                contact.Email = email;

            Console.Write("Enter new phone (or press Enter to keep current): ");
            string phone = Console.ReadLine() ?? string.Empty;
            contact.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone;

            Console.Write("Enter new city (or press Enter to keep current): ");
            string city = Console.ReadLine() ?? string.Empty;
            contact.City = string.IsNullOrWhiteSpace(city) ? null : city;

            await _contactRepository.UpdateContactAsync(contact);
            Console.WriteLine("Contact updated successfully.");
        }

        private async Task DeleteContactAsync()
        {
            Console.Write("Enter contact ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            await _contactRepository.DeleteContactAsync(id);
            Console.WriteLine("Contact deleted successfully.");
        }

        private async Task SearchContactsAsync()
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine() ?? string.Empty;
            
            var contacts = await _contactRepository.SearchContactsAsync(searchTerm);
            Console.WriteLine($"\n=== Search Results for '{searchTerm}' ({contacts.Count()}) ===");
            
            foreach (var contact in contacts)
            {
                Console.WriteLine($"ID: {contact.Id}, Name: {contact.Name}, Email: {contact.Email}, Category: {contact.Category}");
            }
        }

        private async Task ListContactsByCategoryAsync()
        {
            Console.Write("Enter category (Family/Work/Friends/Other): ");
            string categoryStr = Console.ReadLine() ?? string.Empty;
            
            if (!Enum.TryParse<ContactCategory>(categoryStr, true, out var category))
            {
                Console.WriteLine("Invalid category.");
                return;
            }

            var contacts = await _contactRepository.GetContactsByCategoryAsync(category);
            Console.WriteLine($"\n=== Contacts in {category} Category ({contacts.Count()}) ===");
            
            foreach (var contact in contacts)
            {
                Console.WriteLine($"ID: {contact.Id}, Name: {contact.Name}, Email: {contact.Email}");
            }
        }

        private async Task ShowBirthdaysAsync()
        {
            var contacts = await _contactRepository.GetContactsWithBirthdayThisMonthAsync();
            Console.WriteLine($"\n=== Birthdays This Month ({contacts.Count()}) ===");
            
            foreach (var contact in contacts)
            {
                Console.WriteLine($"Name: {contact.Name}, Birthday: {contact.Birthday:MM/dd/yyyy}");
            }
        }

        private async Task ShowStatisticsAsync()
        {
            var stats = await _contactRepository.GetContactStatisticsAsync();
            var totalCount = await _contactRepository.GetContactCountAsync();
            
            Console.WriteLine($"\n=== Contact Statistics (Total: {totalCount}) ===");
            
            foreach (var stat in stats)
            {
                Console.WriteLine($"{stat.Key}: {stat.Value} contacts");
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up dependency injection
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // Configure SQL Server connection
                    services.AddDbContext<ContactDbContext>(options =>
                        options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ContactManagerDb;Trusted_Connection=true;MultipleActiveResultSets=true"));
                    
                    // Register services
                    services.AddScoped<IContactRepository, ContactRepository>();
                    services.AddScoped<ContactManagerApp>();
                })
                .Build();

            // Ensure database is created
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
                await context.Database.EnsureCreatedAsync();
            }

            // Get the service and run the application
            var app = host.Services.GetRequiredService<ContactManagerApp>();
            await app.RunAsync();
        }
    }
} 