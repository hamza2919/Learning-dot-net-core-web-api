using Crud_Operation_Using_EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Crud_Operation_Using_EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThirdPartyAPIController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5";
        public ThirdPartyAPIController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey =  _configuration["WeatherApi:ApiKey"];
        }
        [HttpGet]
        public async Task<ActionResult<List<WeatherResponse>>> GetEmployees(string city)
        {
            try
            {
                string url = $"{_baseUrl}/weather?q={city}&appid={_apiKey}&units=metric";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var weather = JsonConvert.DeserializeObject<WeatherResponse>(json);
                    return Ok(json);
                }
                else
                {
                    // Handle error
                    return BadRequest(response.Content);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or perform additional error handling
                //_logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.xyz");

            }
        }

    }
}
