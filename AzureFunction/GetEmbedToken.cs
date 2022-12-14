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
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log) {

      Guid WorkspaceId = new Guid(Environment.GetEnvironmentVariable("WorkspaceId"));
      Guid ReportId = new Guid(req.Query["report_id"]);
      string Username = req.Query["username"];
      string Roles = req.Query["roles"];

      var embedToken = await PowerBiManager.GetEmbedToken(WorkspaceId, ReportId, Username, Roles);

      return new OkObjectResult(embedToken);

    }
  }
}
