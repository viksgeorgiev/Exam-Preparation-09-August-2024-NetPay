namespace NetPay.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Household
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string ContactPerson { get; set; } = null!;

        [MinLength(6)]
        [MaxLength(80)]
        public string? Email { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(15)")]
        [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]
        public string PhoneNumber { get; set; } = null!;

        public virtual HashSet<Expense> Expenses { get; set; } 
            = new HashSet<Expense>();
    }
}
