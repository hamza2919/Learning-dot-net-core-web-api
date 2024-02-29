using Crud_Operation_Using_EF.DAL;
using Crud_Operation_Using_EF.General;
using Crud_Operation_Using_EF.General.CustomValidationAttibutesOnActionFilter;
using Crud_Operation_Using_EF.General.JWT;
using Crud_Operation_Using_EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using static Crud_Operation_Using_EF.General.SystemEnums;

namespace Crud_Operation_Using_EF.Controllers
{
    public interface IEmployeeServiceOnlyForLearnDI // Dependency Injection
    {
        bool IsCurrentEmployee();
    }

    public class EmployeeServiceForLearnDI: IEmployeeServiceOnlyForLearnDI
    {
        public EmployeeServiceForLearnDI() { }

        public bool IsCurrentEmployee()
        {
            return true;
        }
    }
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EmployeeAPIController : ControllerBase
    { 
        private readonly EmployeeDBContext _Context;
        private readonly IEmployeeServiceOnlyForLearnDI _empService;
        private readonly ILogger<EmployeeAPIController> _logger;

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5";
        public EmployeeAPIController(EmployeeDBContext context,IEmployeeServiceOnlyForLearnDI empService,ILogger<EmployeeAPIController> logger
            ,HttpClient httpClient,IConfiguration configuration

            )
        {
            this._Context = context;
            _empService = empService;
            this._logger = logger;
            
        }
        
        [HttpGet] 
        [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Manager) + "," + nameof(Roles.User))] 
        public async Task<ActionResult<List<Employees>>> GetEmployees(string tokken)
        {
            try
            { 
                var isEmployee = _empService.IsCurrentEmployee();

                #region SANARIOS TO GET CLAIM vALUES USING CLAIM PRINCIPAL
                //var claimsIdentity = User.Identity as ClaimsIdentity;
                //if (claimsIdentity != null)
                //{
                //    // Access specific claims
                //    var UiD = claimsIdentity.FindFirst(UsersDefinedClaimTypes.UserId.ToString())?.Value;
                //    var uNMAE = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;


                //}
                //var user = HttpContext.User; // Get the ClaimsPrincipal from the HttpContext

                //// Retrieve the claim value using FindFirst
                //var claimValue = user.FindFirst(ClaimTypes.Name)?.Value;
                #endregion

                #region SANARIOS TO GET CLAIM vALUES USING jwt tOKKEN dECODING 
                //var userId = JwtTokken.GetClaimValueFromTokkenDeodeSchemeByClaimType(tokken, UsersDefinedClaimTypes.UserId);
                //var userName = JwtTokken.GetClaimValueFromTokkenDeodeSchemeByClaimType(tokken, ClaimTypes.Name);
                //var userRole = JwtTokken.GetClaimValueFromTokkenDeodeSchemeByClaimType(tokken, ClaimTypes.Role);
                #endregion
                int companyId = Convert.ToInt32(JwtTokken.GetClaimValueFromClaimPrincipalByClaimType(HttpContext.User, UsersDefinedClaimTypes.CompanyId));


                var data = await _Context.Company.Where(c => c.Id == companyId).SelectMany(c => c.Employees).ToArrayAsync();
                //var data = await _Context.Employees.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception or perform additional error handling
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.xyz");

            }
        }
        
        [HttpGet("{EmployeeId}")]
        [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Manager))]
        [ValidateEmployeeCompanyAttribute]
        public async Task<ActionResult<Employees>> GetEmployeeById(int EmployeeId)
        {
            try
            {
                //var userId = JwtTokken.GetClaimValueFromTokkenDeodeSchemeByClaimType(tokken, UsersDefinedClaimTypes.UserId);
                //var userName = JwtTokken.GetClaimValueFromTokkenDeodeSchemeByClaimType(tokken, ClaimTypes.Name);
                //var userRole = JwtTokken.GetClaimValueFromTokkenDeodeSchemeByClaimType(tokken, ClaimTypes.Role);

                int companyId = Convert.ToInt32(JwtTokken.GetClaimValueFromClaimPrincipalByClaimType(HttpContext.User, UsersDefinedClaimTypes.CompanyId));
                var data = await _Context.Employees.Where(e => e.EmpId == EmployeeId && e.CompanyId == companyId).FirstOrDefaultAsync();
                //var data = await _Context.Employees.FindAsync(EmployeeId);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception or perform additional error handling
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.xyz");

            }
}
        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Manager))]
        public async Task<ActionResult<Employees>> SaveEmployee( Employees emp)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    var userId = JwtTokken.GetClaimValueFromClaimPrincipalByClaimType(HttpContext.User, UsersDefinedClaimTypes.UserId);

                    emp.CreatedById = userId.ToString();

                    await _Context.Employees.AddAsync(emp);
                    var rowsAffected = await _Context.SaveChangesAsync();

                    if (rowsAffected > 0)
                    {
                        return Ok("Employee Saved successfully...");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save Employee..");
                    }
                     
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                // Log the exception or perform additional error handling
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.\n" + ex.Message);

            }

        }
        [HttpPut("EmployeeId")]
        [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Manager))]
        public async Task<ActionResult<Employees>> UpdateEmployee(int EmployeeId , Employees emp)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    if (EmployeeId != emp.EmpId)
                    {
                        return BadRequest();
                    }
                    if (!EmployeeExists(EmployeeId))
                    {
                        return NotFound();
                    }
                    _Context.Entry(emp).State = EntityState.Modified;
                    await _Context.SaveChangesAsync();
                    return Ok(emp);
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                // Log the exception or perform additional error handling
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.xyz");

            }

        }
        private bool EmployeeExists(int id)
        {
            return _Context.Employees.Any(e => e.EmpId == id);
        } 
        [HttpDelete("EmployeeId")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<ActionResult<Employees>> DeleteEmployeeById(int EmployeeId)
        {
            try
            { 
                var employee = await _Context.Employees.FindAsync(EmployeeId);
                if (employee == null)
                {
                    return NotFound();
                }
                _Context.Employees.Remove(employee);
                await _Context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or perform additional error handling
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.xyz");

            }
        }

    }




}

