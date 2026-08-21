using System.Net.Mail;
using Azure.Storage.Queues;
using Newtonsoft.Json;

namespace StorageAccounts.Mvc.Services
{
    public class QueueService() : IQueueService
    {
        private readonly string queueName = "attendee-emails";
        private readonly QueueClient _queueClient;
        public async Task SendMessage(MailMessage emailMessage)
        {
            await _queueClient.CreateIfNotExistsAsync();

            var message = JsonConvert.SerializeObject(emailMessage);

            await _queueClient.SendMessageAsync(message, timeToLive: new TimeSpan(0,2,0));
        }
    }
}