using StorageAccounts.Mvc.Data;

namespace StorageAccounts.Mvc.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadBlob(IFormFile formFile, string imageName, string? originalBlobName = null);
        Task<string> GetBlobUrl(string imageName);
        Task RemoveBlob(string imageName);
    }
}