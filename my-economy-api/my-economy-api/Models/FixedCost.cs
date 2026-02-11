using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_economy_api.Models
{
    [Table("FixedCosts")]
    public class FixedCost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty; // El ID que viene de Supabase Auth

        [Range(1, int.MaxValue, ErrorMessage = "CategoryID must be greater than 0")]
        public int CategoryID { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Name is mandatory")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be > 0")]
        public decimal Amount { get; set; }

        [Range(1, 365, ErrorMessage = "Frequency must be at least 1 day")]
        public int Frequency { get; set; }
        public DateTime AproxDatePayment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
