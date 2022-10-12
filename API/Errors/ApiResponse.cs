namespace API.Errors
{
  public class ApiResponse
  {
    public ApiResponse(int statusCode, string message = null)
    {
      StatusCode = statusCode;
      Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
      return statusCode switch
      {
        400 => "from API : You have made a Bad Request",
        401 => "cYou are not Authorized",
        404 => "from API : Resource not Found",
        500 => "from API : Server Error",
        _ => null
      };
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
  }
}