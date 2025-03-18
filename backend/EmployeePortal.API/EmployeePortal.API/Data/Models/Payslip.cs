using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.API.Data.Models
{
    public class Payslip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal GrossPay { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Deductions { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal NetPay { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
