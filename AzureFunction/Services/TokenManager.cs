using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Core;

namespace GetEmbedToken.Services {

  class TokenManager {

    public static string GetAadAccessToken() {

      string appId = Environment.GetEnvironmentVariable("AppId");
      string appSecret = Environment.GetEnvironmentVariable("AppSecret");
      string tenentId = Environment.GetEnvironmentVariable("TenantId");
      string tenantSpecificAuthority = "https://login.microsoftonline.com/" + tenentId;
 
      string token = null; 
      string[] scopes = { "https://analysis.windows.net/powerbi/api/.default" };

      if (appId != null) {
        var appConfidential = ConfidentialClientApplicationBuilder.Create(appId)
                                .WithClientSecret(appSecret)
                                .WithAuthority(tenantSpecificAuthority)
                                .Build();
        var authResult = appConfidential.AcquireTokenForClient(scopes).ExecuteAsync().Result;
        token = authResult.AccessToken;
      } else {
        var credential = new DefaultAzureCredential();
        var context = new TokenRequestContext(scopes);
        token = credential.GetTokenAsync(context).Result.Token;
      }

      return token;
    } 

  }

}
