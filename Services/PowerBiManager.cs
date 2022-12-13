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

    public static async Task<string> GetEmbedToken(Guid WorkspaceId, Guid ReportId, string username, string role) {

      var report = await pbiClient.Reports.GetReportInGroupAsync(WorkspaceId,ReportId);
      Console.WriteLine("Report: {0}", report.ToString());

      var datasetRequests = new List<GenerateTokenRequestV2Dataset>();
      datasetRequests.Add(new GenerateTokenRequestV2Dataset(report.DatasetId));
      Console.WriteLine("DatasetRequests: {0}", datasetRequests.ToString());

      var reportRequests = new List<GenerateTokenRequestV2Report>();
      reportRequests.Add(new GenerateTokenRequestV2Report(report.Id, allowEdit: false));
      Console.WriteLine("ReportRequests: {0}", reportRequests.ToString());

      var workspaceRequests = new List<GenerateTokenRequestV2TargetWorkspace>();
      workspaceRequests.Add(new GenerateTokenRequestV2TargetWorkspace(WorkspaceId));
      Console.WriteLine("WorkspaceRequests: {0}", workspaceRequests.ToString());

      var effectiveIdentities = new List<EffectiveIdentity>();
      var effectiveIdentitiiesRoles = new List<string>();
      effectiveIdentitiiesRoles.Add(role);
      effectiveIdentities.Add(new EffectiveIdentity(username: username, roles: effectiveIdentitiiesRoles));
      Console.WriteLine("effectiveIdentities: {0}", effectiveIdentities.ToString());


      GenerateTokenRequestV2 tokenRequest =
        new GenerateTokenRequestV2 {
          Datasets = datasetRequests,
          Identities = effectiveIdentities,
          Reports = reportRequests,
          TargetWorkspaces = workspaceRequests
        };
      Console.WriteLine("tokenRequest: {0}", tokenRequest.ToString());

      // call to Power BI Service API and pass GenerateTokenRequest object to generate embed token
      try {
        var EmbedTokenResult = await pbiClient.EmbedToken.GenerateTokenAsync(tokenRequest);
        return EmbedTokenResult.Token;
      }
      catch (Exception e) {
              Console.WriteLine("Exception Message: {0}", e.Message);
              Console.WriteLine("Exception Stacktrace: {0}", e.StackTrace);
      }

    }

  }

}
