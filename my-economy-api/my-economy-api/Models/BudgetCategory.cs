using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace my_economy_api.Models
{
    [Table("BudgetCategories")]
    public class BudgetCategory
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Percentage { get; set; }
        public bool Active { get; set; } = true; 
    }
}
