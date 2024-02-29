using Crud_Operation_Using_EF.DAL;
using Crud_Operation_Using_EF.General.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Security.Claims;
using static Crud_Operation_Using_EF.General.SystemEnums;

namespace Crud_Operation_Using_EF.General.CustomValidationAttibutesOnActionFilter
{
    public class ValidateEmployeeCompanyAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var employeeId = context.ActionArguments["EmployeeId"] as int?;
                
                if (employeeId.HasValue)
                {
                    // Get the logged-in user's company ID from your authentication mechanism
                    var loggedInCompanyId = JwtTokken.GetClaimValueFromClaimPrincipalByClaimType(context.HttpContext.User,UsersDefinedClaimTypes.CompanyId);

                    // Retrieve the employee from the database
                    var dbContext = context.HttpContext.RequestServices.GetRequiredService<EmployeeDBContext>(); // Using GetRequiredService for clarity
                    var employee = dbContext.Employees.FirstOrDefault(e => e.EmpId == employeeId);

                    // Check if the employee exists and belongs to the logged-in user's company
                     if (employee == null || employee.CompanyId != Convert.ToInt32(loggedInCompanyId))
                     {
                         context.Result = new BadRequestObjectResult("This employee does not exist in the selected company.");
                         return;
                     }
                }
                else
                {
                    context.Result = new BadRequestObjectResult("Invalid employee ID.");
                    return;
                }

            }
            catch (Exception ex)
            {
                context.Result = new BadRequestObjectResult(ex.Message);
                return;
            }
           
            base.OnActionExecuting(context);
        }
        
    }
} 