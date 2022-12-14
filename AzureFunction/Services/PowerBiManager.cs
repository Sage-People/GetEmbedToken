using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetEmbedToken.Services {

  class PowerBiManager {
    static string accessToken = TokenManager.GetAadAccessToken();
    private const string urlPowerBiServiceApiRoot = "https://api.powerbi.com/";
    static TokenCredentials tokenCredentials = new TokenCredentials(accessToken, "Bearer");
    static PowerBIClient pbiClient = new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);

    /* Method to get the EmbedToken from the Power BI service. The Embed Token an authentication token
        used to grant access to a set of reports and datasets when deploying a report in App Owns Data mode. 
        this method could be updated to use a collection of reports instead of a single report like we do with
        roles however that adds some complexity when mapping roles. To simplify this example we are only working 
        with single reports. */
    public static async Task<string> GetEmbedToken(Guid WorkspaceId, Guid ReportId, string username, string roles, int timeout) {
      //Get report metadata from the Power BI service using the service principal.
      var report = await pbiClient.Reports.GetReportInGroupAsync(WorkspaceId,ReportId);
      //Create list of associated datasets to request access to with the embed token.
      var datasetRequests = new List<GenerateTokenRequestV2Dataset>();
      datasetRequests.Add(new GenerateTokenRequestV2Dataset(report.DatasetId));
      //Create list of reports to request access to with the Embed Token
      var reportRequests = new List<GenerateTokenRequestV2Report>();
      reportRequests.Add(new GenerateTokenRequestV2Report(report.Id, allowEdit: false));
      //Create list of workspaces to request access to with the Embed Token.
      var workspaceRequests = new List<GenerateTokenRequestV2TargetWorkspace>();
      workspaceRequests.Add(new GenerateTokenRequestV2TargetWorkspace(WorkspaceId));
      //Create list of effective identities to associate the Embed Token with.
      var effectiveIdentities = new List<EffectiveIdentity>();
      var effectiveIdentity = new EffectiveIdentity(username: username, datasets: new List<string>(){report.DatasetId.ToString()});
      /* Create list of roles to request access to with the Embed token. These roles must exist in the report 
          and the report must have at least one role for Effective Identity to work */
      if (roles != null) {
        var effectiveIdentitiiesRoles = new List<string>();
          foreach(var r in roles.Split(",")) {
            effectiveIdentitiiesRoles.Add(r);
          }
        effectiveIdentity.Roles = effectiveIdentitiiesRoles;
      }
      effectiveIdentities.Add(effectiveIdentity);
      //Create the actual token requests with the data generated above
      GenerateTokenRequestV2 tokenRequest =
        new GenerateTokenRequestV2 {
          Datasets = datasetRequests,
          Identities = effectiveIdentities,
          Reports = reportRequests,
          TargetWorkspaces = workspaceRequests
        };
        /* Set the timeout for the Embed Token. After this period of time the function needs to be called again for a 
            user to continue accessing the reports */ 
        tokenRequest.LifetimeInMinutes = timeout;

      // call to Power BI Service API and pass GenerateTokenRequest object to generate embed token
      var EmbedTokenResult = await pbiClient.EmbedToken.GenerateTokenAsync(tokenRequest);
      return EmbedTokenResult.Token;
    }
  }
}
