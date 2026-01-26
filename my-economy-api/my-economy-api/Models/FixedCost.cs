using System.ComponentModel.DataAnnotations;

namespace my_economy_api.Models
{
    public class FixedCost
    {
        [Key]
        public int Id { get; set; }

        public string? Expense { get; set; }

        public float Amount { get; set; }

        public int Frequency { get; set; }

    }
}
