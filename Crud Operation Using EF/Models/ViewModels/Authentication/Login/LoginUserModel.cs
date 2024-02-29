using System.ComponentModel.DataAnnotations;

namespace Crud_Operation_Using_EF.Models.ViewModels.Authentication.Login
{
    public class LoginUserModel
    {
        [Required (ErrorMessage ="User Name is Required")] 
        public string UserName  { get; set; } 
        [Required (ErrorMessage ="Password is Required")] 
        public string Password  { get; set; }

        [Required (ErrorMessage ="CompanyId is required")] 
        public int CompanyId  { get; set; }
    }
}
