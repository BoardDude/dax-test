using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactManager
{
    public class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        
        public Contact(string name, string phone, string email = "", string address = "")
        {
            Name = name;
            Phone = phone;
            Email = email;
            Address = address;
        }
        
        public override string ToString()
        {
            return $"Name: {Name}, Phone: {Phone}, Email: {Email}";
        }
    }
    
    public class ContactManager
    {
        private List<Contact> contacts;
        private Dictionary<string, Contact> contactsByName;
        
        public ContactManager()
        {
            contacts = new List<Contact>();
            contactsByName = new Dictionary<string, Contact>();
        }
        
        public void AddContact(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Name))
            {
                Console.WriteLine("Contact name cannot be empty.");
                return;
            }
            
            if (contactsByName.ContainsKey(contact.Name.ToLower()))
            {
                Console.WriteLine($"Contact '{contact.Name}' already exists.");
                return;
            }
            
            contacts.Add(contact);
            contactsByName[contact.Name.ToLower()] = contact;
            Console.WriteLine($"Contact '{contact.Name}' added successfully.");
        }
        
        public Contact FindContact(string name)
        {
            if (contactsByName.TryGetValue(name.ToLower(), out Contact contact))
            {
                return contact;
            }
            return null;
        }
        
        public void RemoveContact(string name)
        {
            Contact contact = FindContact(name);
            if (contact != null)
            {
                contacts.Remove(contact);
                contactsByName.Remove(name.ToLower());
                Console.WriteLine($"Contact '{name}' removed successfully.");
            }
            else
            {
                Console.WriteLine($"Contact '{name}' not found.");
            }
        }
        
        public void DisplayAllContacts()
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                return;
            }
            
            Console.WriteLine($"\n=== Contact List ({contacts.Count} contacts) ===");
            for (int i = 0; i < contacts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {contacts[i]}");
            }
        }
        
        public void SearchContacts(string searchTerm)
        {
            var results = contacts.Where(c => 
                c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(searchTerm)
            ).ToList();
            
            if (results.Count == 0)
            {
                Console.WriteLine($"No contacts found matching '{searchTerm}'.");
                return;
            }
            
            Console.WriteLine($"\n=== Search Results ({results.Count} matches) ===");
            foreach (var contact in results)
            {
                Console.WriteLine(contact);
            }
        }
        
        public void DisplayContactStats()
        {
            Console.WriteLine($"\n=== Contact Statistics ===");
            Console.WriteLine($"Total contacts: {contacts.Count}");
            
            if (contacts.Count > 0)
            {
                var contactsWithEmail = contacts.Count(c => !string.IsNullOrEmpty(c.Email));
                var contactsWithAddress = contacts.Count(c => !string.IsNullOrEmpty(c.Address));
                
                Console.WriteLine($"Contacts with email: {contactsWithEmail}");
                Console.WriteLine($"Contacts with address: {contactsWithAddress}");
                
                var longestName = contacts.OrderByDescending(c => c.Name.Length).First();
                Console.WriteLine($"Longest name: {longestName.Name} ({longestName.Name.Length} characters)");
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Contact Manager ===");
            
            ContactManager manager = new ContactManager();
            
            // Add some sample contacts
            manager.AddContact(new Contact("John Doe", "555-0101", "john@email.com", "123 Main St"));
            manager.AddContact(new Contact("Jane Smith", "555-0102", "jane@email.com"));
            manager.AddContact(new Contact("Bob Johnson", "555-0103", "", "456 Oak Ave"));
            
            while (true)
            {
                Console.WriteLine("\nChoose an operation:");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. Find Contact");
                Console.WriteLine("3. Remove Contact");
                Console.WriteLine("4. Display All Contacts");
                Console.WriteLine("5. Search Contacts");
                Console.WriteLine("6. Display Statistics");
                Console.WriteLine("7. Exit");
                
                Console.Write("Enter your choice (1-7): ");
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        AddNewContact(manager);
                        break;
                    case "2":
                        FindContactByName(manager);
                        break;
                    case "3":
                        RemoveContactByName(manager);
                        break;
                    case "4":
                        manager.DisplayAllContacts();
                        break;
                    case "5":
                        SearchContacts(manager);
                        break;
                    case "6":
                        manager.DisplayContactStats();
                        break;
                    case "7":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1-7.");
                        break;
                }
            }
        }
        
        static void AddNewContact(ContactManager manager)
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            
            Console.Write("Enter phone: ");
            string phone = Console.ReadLine();
            
            Console.Write("Enter email (optional): ");
            string email = Console.ReadLine();
            
            Console.Write("Enter address (optional): ");
            string address = Console.ReadLine();
            
            Contact newContact = new Contact(name, phone, email, address);
            manager.AddContact(newContact);
        }
        
        static void FindContactByName(ContactManager manager)
        {
            Console.Write("Enter contact name: ");
            string name = Console.ReadLine();
            
            Contact contact = manager.FindContact(name);
            if (contact != null)
            {
                Console.WriteLine($"\nFound contact: {contact}");
            }
            else
            {
                Console.WriteLine($"Contact '{name}' not found.");
            }
        }
        
        static void RemoveContactByName(ContactManager manager)
        {
            Console.Write("Enter contact name to remove: ");
            string name = Console.ReadLine();
            
            manager.RemoveContact(name);
        }
        
        static void SearchContacts(ContactManager manager)
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine();
            
            manager.SearchContacts(searchTerm);
        }
    }
} 