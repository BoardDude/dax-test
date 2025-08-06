using ContactManager.Models;

namespace ContactManager.Services
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact?> GetContactByIdAsync(int id);
        Task<Contact?> GetContactByEmailAsync(string email);
        Task<IEnumerable<Contact>> GetContactsByCategoryAsync(ContactCategory category);
        Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm);
        Task<IEnumerable<Contact>> GetContactsWithBirthdayThisMonthAsync();
        Task<Contact> AddContactAsync(Contact contact);
        Task<Contact> UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
        Task<int> GetContactCountAsync();
        Task<IEnumerable<ContactCategory>> GetContactCategoriesAsync();
        Task<Dictionary<ContactCategory, int>> GetContactStatisticsAsync();
    }
} 