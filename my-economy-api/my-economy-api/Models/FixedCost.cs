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
        public string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public float Amount { get; set; }
        public int Frequency { get; set; }
        public DateTime AproxDatePayment { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
