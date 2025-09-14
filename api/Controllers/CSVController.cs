using api.Database.Connection;
using api.Enums;
using api.Models;
using api.Repository;
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
        /// <param name="csvBase64">
        /// The base64-encoded CSV payload. The value may include a prefix such as 
        /// data:text/csv;base64,.
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
                // Check if Base64 content is null or empty → return 400 Bad Request
                if (string.IsNullOrEmpty(csvBase64.Base64Content))
                {
                    var response = new ApiResponse
                    {
                        Title = "Bad Request",
                        StatusCode = "400"
                    };
                    return BadRequest(response);
                }

                // Call service to process CSV (decode, validate, convert to records)
                bool result = _csvService.InsertData(csvBase64);

                // If successful → return 200 OK 
                return Ok(result);
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe);
                // Handle invalid Base64 or CSV format → return 400 Bad Request
                var response = new ApiResponse
                {
                    Title = "Bad Request",
                    StatusCode = "400"
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {

                // If exception indicates wrong file type → return 415 Unsupported Media Type
                if (ex.Message == "415")
                {
                    ApiResponse unsupportedMediaTypeResponse = new()
                    {
                        Title = "Unsupported Media Type",
                        StatusCode = "415"
                    };
                    return StatusCode(415, unsupportedMediaTypeResponse);
                }
                else
                {
                    // Any other error → log exception and return 500 Internal Server Error
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

/*
        [HttpGet]
        public ActionResult<List<IssueType>> test()
        {
            IssueTypeRepository issueTypeRepository = new();

            IssueTypeStatus[]? enums = IssueTypeStatus.GetValues<IssueTypeStatus>();

            List<IssueType?> myList = [];

            for (int i = 0; i <= enums.Length - 1; i++)
            {
                IssueType issueType = new IssueType() { Status = enums[i], Priority = IssueTypePriority.Low };
                myList.Add(issueTypeRepository.Post(issueType));
            }

            return Ok(myList);
        }
  */      
        [HttpGet]
        public ActionResult<Issue> TestDos()
        {
            return Ok(new IssueRepository().Post(new Issue()));
        }
    }
}