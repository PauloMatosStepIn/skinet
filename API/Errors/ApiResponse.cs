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
        400 => "You have made a Bad Request",
        401 => "You are not Authorized",
        404 => "Resource not Found",
        500 => "Server Error",
        _ => null
      };
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
  }
}