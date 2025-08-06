using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Services
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _context;

        public ContactRepository(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _context.Contacts
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            return await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Contact?> GetContactByEmailAsync(string email)
        {
            return await _context.Contacts
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Contact>> GetContactsByCategoryAsync(ContactCategory category)
        {
            return await _context.Contacts
                .Where(c => c.Category == category)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm)
        {
            return await _context.Contacts
                .Where(c => c.Name.Contains(searchTerm) || 
                           c.Email.Contains(searchTerm) || 
                           c.Phone!.Contains(searchTerm) || 
                           c.City!.Contains(searchTerm))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsWithBirthdayThisMonthAsync()
        {
            var currentMonth = DateTime.Now.Month;
            return await _context.Contacts
                .Where(c => c.Birthday.HasValue && c.Birthday.Value.Month == currentMonth)
                .OrderBy(c => c.Birthday!.Value.Day)
                .ToListAsync();
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            contact.CreatedAt = DateTime.UtcNow;
            contact.UpdatedAt = DateTime.UtcNow;
            
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            
            return contact;
        }

        public async Task<Contact> UpdateContactAsync(Contact contact)
        {
            var existingContact = await GetContactByIdAsync(contact.Id);
            if (existingContact == null)
            {
                throw new ArgumentException("Contact not found.");
            }

            existingContact.Name = contact.Name;
            existingContact.Email = contact.Email;
            existingContact.Phone = contact.Phone;
            existingContact.City = contact.City;
            existingContact.Category = contact.Category;
            existingContact.Birthday = contact.Birthday;
            existingContact.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingContact;
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await GetContactByIdAsync(id);
            if (contact == null)
            {
                throw new ArgumentException("Contact not found.");
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetContactCountAsync()
        {
            return await _context.Contacts.CountAsync();
        }

        public async Task<IEnumerable<ContactCategory>> GetContactCategoriesAsync()
        {
            return await _context.Contacts
                .Select(c => c.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Dictionary<ContactCategory, int>> GetContactStatisticsAsync()
        {
            return await _context.Contacts
                .GroupBy(c => c.Category)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
} 