using Azure;
using Azure.Data.Tables;
using StorageAccounts.Mvc.Data;

namespace StorageAccounts.Mvc.Services
{
    public class TableStorageService() : ITableStorageService
    {
        private const string tableName = "Attendees";
        private readonly TableServiceClient _tableServiceClient;

        public async Task<Attendee> GetAttendee(string industry, string id)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName); 
            return await tableClient.GetEntityAsync<Attendee>(industry, id);
        }

        public async Task<List<Attendee>> GetAttendees()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName); 
            Pageable<Attendee> attendeeEntities = tableClient.Query<Attendee>();
            return [.. attendeeEntities];
        }

        public async Task UpsertAttendee(Attendee Attendee)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName); 
            await tableClient.UpsertEntityAsync(Attendee);
        }

        public async Task DeleteAttendee(string industry, string id)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName); 
            await tableClient.DeleteEntityAsync(industry, id);
        }
    }
}