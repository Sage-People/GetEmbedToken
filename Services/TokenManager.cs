using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetEmbedToken.Services {

  class TokenManager {

    public static string GetAadAccessToken() {

      string appId = Environment.GetEnvironmentVariable("AppId");
      string appSecret = Environment.GetEnvironmentVariable("AppSecret");
      string tenentId = Environment.GetEnvironmentVariable("TenantId");
      string tenantSpecificAuthority = "https://login.microsoftonline.com/" + tenentId;


      var appConfidential = ConfidentialClientApplicationBuilder.Create(appId)
                                .WithClientSecret(appSecret)
                                .WithAuthority(tenantSpecificAuthority)
                                .Build();

      string[] scopes = { "https://analysis.windows.net/powerbi/api/.default" };

      var authResult = appConfidential.AcquireTokenForClient(scopes).ExecuteAsync().Result;
      return authResult.AccessToken;
    } 


  }


}
