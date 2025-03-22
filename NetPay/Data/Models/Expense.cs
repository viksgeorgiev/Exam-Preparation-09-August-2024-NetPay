using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetPay.Data.Models.Enums;

namespace NetPay.Data.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string ExpenseName { get; set; } = null!;

        [Required]
        [Range(typeof(decimal),"0.01", "100000")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        [ForeignKey(nameof(Household))]
        public int HouseholdId { get; set; }
        public virtual Household Household { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Service))]
        public int ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}