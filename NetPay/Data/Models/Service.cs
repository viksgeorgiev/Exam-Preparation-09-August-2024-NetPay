using System.ComponentModel.DataAnnotations;

namespace NetPay.Data.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string ServiceName { get; set; } = null!;

        public virtual HashSet<Expense> Expenses { get; set; } 
            = new HashSet<Expense>();

        public virtual HashSet<SupplierService> SuppliersServices { get; set; } 
            = new HashSet<SupplierService>();
    }
}