using ABCpharmacy.DTOs;
using ABCpharmacy.Models;
using ABCpharmacy.Repositories;

namespace ABCpharmacy.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _repository;

        public MedicineService(IMedicineRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Medicine>> GetAllMedicinesAsync() => _repository.GetAllAsync();

        public Task<Medicine?> GetMedicineByNameAsync(string name) => _repository.GetByNameAsync(name);

        public async Task<Medicine> AddMedicineAsync(CreateMedicineDto dto)
        {
            var existing = await _repository.GetByNameAsync(dto.FullName);
            if (existing != null)
                throw new InvalidOperationException($"Medicine '{dto.FullName}' already exists.");

            var medicine = new Medicine
            {
                FullName = dto.FullName,
                Notes = dto.Notes,
                ExpiryDate = dto.ExpiryDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Brand = dto.Brand
            };

            await _repository.AddAsync(medicine);
            return medicine;
        }
    }
}