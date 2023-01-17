using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace StoreFunctions
{
  public static class UserFunctions
  {

    static UserService userService = new();

    [FunctionName("LogInUser")]
    public static async Task<IActionResult> LogIn(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req,
        ILogger log)
    {
      string email = req.Query["user"];
      string password = req.Query["pswd"];
      User wantedUser = await userService.LogInUser(email, password);
      bool successfull = wantedUser != null;
      return successfull ? new OkObjectResult(Utils.getJWTToken(wantedUser))
      : new BadRequestObjectResult("Credentials are not correct");
    }

    [FunctionName("CreateUser")]
    public static async Task<IActionResult> CreateUser(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/add/")] HttpRequest req,
        ILogger log)
    {
      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      User newUser = JsonConvert.DeserializeObject<User>(requestBody, Utils.deserializeSettings);
      bool succesfull = await userService.AddNewUser(newUser);
      return succesfull ? new OkObjectResult(Utils.getJWTToken(newUser))
        : new BadRequestObjectResult("Not a valid user");
    }

    [FunctionName("RefreshToken")]
    public static async Task<IActionResult> RefreshSession(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "refresh")] HttpRequest req,
        ILogger log)
    {
      User wantedUser = await userService.GetNewRefreshToken(req);
      bool successfull = wantedUser != null;
      return successfull ? new OkObjectResult(Utils.getJWTToken(wantedUser))
      : new BadRequestObjectResult("Credentials are not correct");
    }


    [FunctionName("usersTest")]
    public static async Task<IActionResult> TestAuth(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
      ValidateJWT auth = new ValidateJWT(req);
      if (!auth.IsValid)
      {
        return new UnauthorizedResult();
      }
      return new OkObjectResult("Merge!");
    }
  }
}
