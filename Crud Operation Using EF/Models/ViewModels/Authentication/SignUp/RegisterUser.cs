using System.ComponentModel.DataAnnotations;

namespace Crud_Operation_Using_EF.Models.ViewModels.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required] 
        [StringLength(255, ErrorMessage = "First Name must be of 255 characters")]
        public string FirstName { get; set; }

        [Required] 
        [StringLength(255, ErrorMessage = "First Name must be of 255 characters")]
        public string LastName { get; set; }

        [Required] 
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)] 
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

    }
}
