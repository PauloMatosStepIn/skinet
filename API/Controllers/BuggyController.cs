using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class BuggyController : BaseApiController
  {
    private readonly StoreContext _context;
    public BuggyController(StoreContext context)
    {
      _context = context;
    }

    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> GetSecretText()
    {
      return ("secret stuff");
    }


    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
      return NotFound(new ApiResponse(404));
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
      //nonexistent record
      var thing = _context.Products.Find(42);

      //generates an exception => 500 error
      var thingToReturn = thing.ToString();

      return Ok();
    }
    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
      return BadRequest(new ApiResponse(400));
    }
    [HttpGet("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {


      return Ok();
    }
  }
}