using System.Text.Json;
using ABCpharmacy.Models;

namespace ABCpharmacy.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _fileLock = new(1, 1);
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public MedicineRepository(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data", "SeedData", "medicines.json");
        }

        public async Task<List<Medicine>> GetAllAsync()
        {
            await _fileLock.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                    return new List<Medicine>();

                var json = await File.ReadAllTextAsync(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<Medicine>();

                return JsonSerializer.Deserialize<List<Medicine>>(json, _jsonOptions)
                       ?? new List<Medicine>();
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task<Medicine?> GetByNameAsync(string name)
        {
            var searchText = name.Replace(" ", "").ToLowerInvariant();

            var medicines = await GetAllAsync();

            return medicines.FirstOrDefault(m => m.FullName.Replace(" ", "").ToLowerInvariant().Contains(searchText));
        }

        public async Task AddAsync(Medicine medicine)
        {
            await _fileLock.WaitAsync();
            try
            {
                var medicines = new List<Medicine>();

                if (File.Exists(_filePath))
                {
                    var existingJson = await File.ReadAllTextAsync(_filePath);
                    if (!string.IsNullOrWhiteSpace(existingJson))
                    {
                        medicines = JsonSerializer.Deserialize<List<Medicine>>(existingJson, _jsonOptions)
                                    ?? new List<Medicine>();
                    }
                }

                medicines.Add(medicine);

                var updatedJson = JsonSerializer.Serialize(medicines, _jsonOptions);
                await File.WriteAllTextAsync(_filePath, updatedJson);
            }
            finally
            {
                _fileLock.Release();
            }
        }
    }
}
