using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using GetEmbedToken.Services;

namespace GetEmbedToken {
  public static class GetEmbedToken {
    [FunctionName("GetEmbedToken")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log) {

      Guid WorkspaceId = new Guid(Environment.GetEnvironmentVariable("WorkspaceId"));
      Guid ReportId = new Guid(Environment.GetEnvironmentVariable("ReportId"));
      string Username = Environment.GetEnvironmentVariable("Username");
      string Role = Environment.GetEnvironmentVariable("Role");

      var embedToken = await PowerBiManager.GetEmbedToken(WorkspaceId, ReportId, Username, Role);

      return new OkObjectResult(embedToken);

    }
  }
}
