using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Issue> Issue([FromBody] Issue issue)
        {
            return Ok(issue);
        }
        
    }
}