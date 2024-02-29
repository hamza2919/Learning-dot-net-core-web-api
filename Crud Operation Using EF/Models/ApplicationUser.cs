using Microsoft.AspNetCore.Identity;

namespace Crud_Operation_Using_EF.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
