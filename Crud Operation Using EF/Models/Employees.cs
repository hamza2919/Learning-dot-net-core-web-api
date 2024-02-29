using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Crud_Operation_Using_EF.Models
{
    public class Employees
    {
        [Key]
        public int EmpId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        [ForeignKey("Area")]
        [Required(ErrorMessage = "Company is required.")]
        public int? AreaId { get; set; }
        public int Age { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public string? Gender { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("ApplicationUser")] 
        public string? CreatedById { get; set; } 
        public DateTime CreatedDate { get; set; }
        [ForeignKey("Company")]
        [Required(ErrorMessage = "Company is required.")]
        public int? CompanyId { get; set; }

        public AreaColony? Area { get; set; } // Navigation property 
        public Company? Company { get; set; } // Navigation property 
        public ApplicationUser? ApplicationUser { get; set; } // Navigation property 
    }
}
