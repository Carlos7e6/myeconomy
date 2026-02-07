using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_economy_api.Models
{
    [Table("FixedCosts")]
    public class FixedCost
    {
        [Key]
        public int Id { get; set; }
        public int CategoryID { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Name is mandatory")]
        public string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be > 0")]
        public float Amount { get; set; }

        [Range(1, 365, ErrorMessage = "Frequency must be at least 1 day")]
        public int Frequency { get; set; }
        public DateTime AproxDatePayment { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
