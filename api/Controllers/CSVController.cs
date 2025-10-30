using System.IdentityModel.Tokens.Jwt;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/csv")]
    [ApiController]
    public class CSVController(ICSVService _csvService) : ControllerBase
    {

        /// <summary>
        /// Uploads a base64-encoded CSV file, validates its structure, 
        /// and inserts the records into the database.
        /// </summary>
        /// <param name="csvBase64">
        /// The base64-encoded CSV payload. The value may include a prefix such as 
        /// data:text/csv;base64,.
        /// </param>
        /// <returns>
        /// A standardized <see cref="ApiResponse"/> indicating success or failure.
        /// </returns>
        /// <response code="201">Created</response>
        /// <response code="422">Unprocessable Entity</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="415">Unsupported Media Type</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        public IActionResult UploadCSV([FromBody] UploadedBase64 csvBase64)
        {
            try
            {
                // Validation: empty content → 400
                if (string.IsNullOrWhiteSpace(csvBase64.Base64Content))
                {
                    return StatusCode(400, new ApiResponse
                    {
                        Title = "Bad Request",
                        StatusCode = 400
                    });
                }

                // Process CSV and insert into the database
                bool result = _csvService.InsertData(csvBase64);

                if (result)
                {
                    // If records were inserted successfully → 201
                    return StatusCode(201, new ApiResponse
                    {
                        Title = "Created",
                        StatusCode = 201
                    });
                }

                // If no records were inserted → 422
                return StatusCode(422, new ApiResponse
                {
                    Title = "Unprocessable Entity",
                    StatusCode = 422
                });
            }
            catch (FormatException)
            {
                // If the Base64 format is invalid → 400
                return StatusCode(400, new ApiResponse
                {
                    Title = "Bad Request",
                    StatusCode = 400
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                if (ex.Message == "415")
                {
                    // Unsupported media type → 415
                    return StatusCode(415, new ApiResponse
                    {
                        Title = "Unsupported Media Type",
                        StatusCode = 415
                    });
                }

                // Generic error → 500
                return StatusCode(500, new ApiResponse
                {
                    Title = "Internal Server Error",
                    StatusCode = 500
                });
            }
        }
    }
}