using System.ComponentModel.DataAnnotations;

namespace Crud_Operation_Using_EF.Models
{
    public class AreaColony
    {
        [Key]
        public int AreaId { get; set; }
        public string? Name { get; set; }
        public ICollection<Employees>? Employees { get; set; }
    }
}
