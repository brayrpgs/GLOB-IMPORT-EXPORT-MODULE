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
        /// and processes it into JSON data.
        /// </summary>
        /// <remarks>
        /// **Request Example**
        ///
        ///     POST /api/csv/upload
        ///
        ///     {
        ///         "base64Content": "data:text/csv;base64,UmVzdW1lbixJc3N1ZSBrZXks..."
        ///     }
        ///
        /// **Behavior**
        /// - Validates that the request body is not empty.
        /// - Ensures the content is a valid base64-encoded CSV.
        /// - Checks that the CSV has a header row and consistent columns.
        /// - On success, processes the file and returns a 200 OK response.
        /// - Returns error codes for invalid or unsupported requests.
        /// </remarks>
        /// <param name="csvBase64">
        /// The base64-encoded CSV payload. The value may include a prefix such as 
        /// `data:text/csv;base64,`.
        /// </param>
        /// <returns>
        /// A standardized <see cref="ApiResponse"/> indicating success or failure.
        /// </returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="415">Unsupported Media Type</response>
        /// <response code="500">Internal Server Error</response>
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