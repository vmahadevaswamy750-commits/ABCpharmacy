using System.ComponentModel.DataAnnotations;

namespace ABCpharmacy.DTOs
{
    public class CreateMedicineDto
    {
        [Required, MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        public string? Notes { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public string? Brand { get; set; }
    }
}
