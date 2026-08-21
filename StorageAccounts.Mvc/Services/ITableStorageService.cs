using StorageAccounts.Mvc.Data;

namespace StorageAccounts.Mvc.Services
{
    public interface ITableStorageService
    {
        Task DeleteAttendee(string industry, string id);
        Task<Attendee> GetAttendee(string industry, string id);
        Task<List<Attendee>> GetAttendees();
        Task UpsertAttendee(Attendee attendeeEntity);
    }
}