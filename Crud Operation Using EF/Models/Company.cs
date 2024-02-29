using System.ComponentModel.DataAnnotations;

namespace Crud_Operation_Using_EF.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Employees>? Employees { get; set; }
    }
}
