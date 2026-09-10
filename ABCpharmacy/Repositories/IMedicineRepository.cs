using ABCpharmacy.Models;

namespace ABCpharmacy.Repositories
{
    public interface IMedicineRepository
    {
        Task<List<Medicine>> GetAllAsync();
        Task<Medicine?> GetByNameAsync(string name);
        Task AddAsync(Medicine medicine);
    }
}