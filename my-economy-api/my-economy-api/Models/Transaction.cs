using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_economy_api.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public int? CategoryID { get; set; }

        public bool IsIncome { get; set; } = false;

        public DateTime? CreatedAt { get; set; }
    }
}
