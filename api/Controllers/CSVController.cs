using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/csv")]
    [ApiController]
    public class CSVController(ICSVService _csvService) : ControllerBase
    {

        [HttpPost]
        public IActionResult UploadCSV([FromBody] UploadedBase64 csvBase64)
        {
            try
            {
                if (string.IsNullOrEmpty(csvBase64.Base64Content))
                {
                    var response = new ApiResponse
                    {
                        Title = "Bad Request",
                        StatusCode = "400"
                    };
                    return BadRequest(response);
                }

                if (!_csvService.UploadCSV(csvBase64))
                {
                    var response = new ApiResponse
                    {
                        Title = "Unsupported Media Type",
                        StatusCode = "415"
                    };
                    return StatusCode(415, response);
                }

                var okResponse = new ApiResponse
                {
                    Title = "Ok",
                    StatusCode = "200"
                };
                return Ok(okResponse);
            }
            catch (FormatException)
            {
                var response = new ApiResponse
                    {
                        Title = "Bad Request",
                        StatusCode = "400"
                    };
                    return BadRequest(response);
            }

            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex}");
                var response = new ApiResponse
                {
                    Title = "Internal Server Error",
                    StatusCode = "500"
                };
                return StatusCode(500, response);
            }
        }
    }
}