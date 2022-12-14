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
    /* The defines a function in the Azure Function app with the name GetEmbedToken. It can be triggers via a web service
    call using the get or post methods. */
    [FunctionName("GetEmbedToken")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log) {
      //Capture the parameters passed through the web service call
      Guid WorkspaceId = new Guid(Environment.GetEnvironmentVariable("WorkspaceId"));
      Guid ReportId = new Guid(req.Query["report_id"]);
      string Username = req.Query["username"];
      string Roles = req.Query["roles"];

      // Make an call to get the Power BI Embed token from  Services/PowerBIManager.cs
      var embedToken = await PowerBiManager.GetEmbedToken(WorkspaceId, ReportId, Username, Roles);

      /* Return the output of getting our EmbedToken as a class. We created a class to return additional attributes
      for simplicity in the calling app instead of just returning the embed token. If bandwidth is a concern you could
      choose to just return the EmbedToken */ 
      var output = new ExecutionResult(WorkspaceId, ReportId, embedToken, 
        String.Format("https://app.powerbi.com/reportEmbed?reportId={0}&groupId={1}", ReportId, WorkspaceId));

      return new OkObjectResult(output);

    }
  }
  //Class definition for returning output to the callings app.
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
