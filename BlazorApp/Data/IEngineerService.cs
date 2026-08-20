namespace BlazorApp.Data
{
    public interface IEngineerService
    {
        Task AddEngineer(Engineer engineer);
        Task UpdateEngineer(Engineer engineer);
        Task DeleteEngineer(Guid? id);
        Task<List<Engineer>> GetEngineerDetails();
        Task<Engineer?> GetEngineerDetailsById(Guid? id);
    }
}