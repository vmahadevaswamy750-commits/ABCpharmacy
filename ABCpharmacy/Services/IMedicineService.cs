using ABCpharmacy.DTOs;
using ABCpharmacy.Models;

namespace ABCpharmacy.Services
{
    public interface IMedicineService
    {
        Task<List<Medicine>> GetAllMedicinesAsync();
        Task<Medicine?> GetMedicineByNameAsync(string name);
        Task<Medicine> AddMedicineAsync(CreateMedicineDto dto);
    }
}
