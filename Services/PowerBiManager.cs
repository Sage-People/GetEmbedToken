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

      var datasetRequests = new List<GenerateTokenRequestV2Dataset>();
      datasetRequests.Add(new GenerateTokenRequestV2Dataset(report.DatasetId));

      var reportRequests = new List<GenerateTokenRequestV2Report>();
      reportRequests.Add(new GenerateTokenRequestV2Report(report.Id, allowEdit: false));

      var workspaceRequests = new List<GenerateTokenRequestV2TargetWorkspace>();
      workspaceRequests.Add(new GenerateTokenRequestV2TargetWorkspace(WorkspaceId));

      var effectiveIdentities = new List<EffectiveIdentity>();
      var effectiveIdentitiiesRoles = new List<string>();
      effectiveIdentitiiesRoles.Add(role);
      effectiveIdentities.Add(new EffectiveIdentity(username: username, roles: effectiveIdentitiiesRoles));


      GenerateTokenRequestV2 tokenRequest =
        new GenerateTokenRequestV2 {
          Datasets = datasetRequests,
          Identities = effectiveIdentities,
          Reports = reportRequests,
          TargetWorkspaces = workspaceRequests
        };

      // call to Power BI Service API and pass GenerateTokenRequest object to generate embed token
      var EmbedTokenResult = await pbiClient.EmbedToken.GenerateTokenAsync(tokenRequest);

      return EmbedTokenResult.Token;

    }

  }

}
