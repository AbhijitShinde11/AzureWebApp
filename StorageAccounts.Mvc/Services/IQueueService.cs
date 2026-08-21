using System.Net.Mail;

namespace StorageAccounts.Mvc.Services
{
    public interface IQueueService
    {
        Task SendMessage(MailMessage emailMessage);
    }
}