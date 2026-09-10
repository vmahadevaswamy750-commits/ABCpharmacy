using Microsoft.AspNetCore.Mvc;
using ABCpharmacy.DTOs;
using ABCpharmacy.Services;

namespace ABCpharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicinesController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        // GET: api/medicines
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicines = await _medicineService.GetAllMedicinesAsync();
            return Ok(medicines);
        }

        // GET: api/medicines/name/{name}
        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var medicine = await _medicineService.GetMedicineByNameAsync(name);
            if (medicine == null)
                return NotFound($"Medicine '{name}' not found.");

            return Ok(medicine);
        }

        // POST: api/medicines
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateMedicineDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _medicineService.AddMedicineAsync(dto);
                return CreatedAtAction(nameof(GetByName), new { name = created.FullName }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
