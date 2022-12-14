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

      var output = new ExecutionResult(WorkspaceId, ReportId, embedToken, 
        String.Format("https://app.powerbi.com/reportEmbed?reportId={0}&groupId={1}", ReportId, WorkspaceId));

      return new OkObjectResult(output);

    }
  }

  public class ExecutionResult {
       public ExecutionResult(Guid WorkspaceId, Guid ReportId, string EmbedToken, string ReportUri) {
        this.WorkspaceId = WorkspaceId;
        this.ReportId = ReportId;
        this.EmbedToken = EmbedToken;
        this.ReportUri = ReportUri;
       }
       public Guid WorkspaceId { get; set; }
       public Guid ReportId { get; set; }
       public string EmbedToken { get; set; }
       public string ReportUri {get; set;}
  }

}
