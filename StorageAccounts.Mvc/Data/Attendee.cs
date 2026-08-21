using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace StorageAccounts.Mvc.Data
{
    public class Attendee : ITableEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Industry { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string? PartitionKey { get ; set ; }
        public string RowKey { get ; set ; } = string.Empty;
        public DateTimeOffset? Timestamp { get ; set ; }
        public ETag ETag { get ; set ; }
    }
}