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
    //This function gets the Azure Active Directory access token needed to communicate with the PowerBI service.
    public static string GetAadAccessToken() {
      
      //Read the AppId envirnoment variable.
      string appId = null;
      try {
        appId = Environment.GetEnvironmentVariable("app_id");
      } catch (ArgumentNullException) {
        // Continue and keep appId as null we'll try Serviice Principal instead;
      }
 
      //Set cope for OAuth token to allow it to authenticate to Power BI
      string token = null; 
      string[] scopes = { "https://analysis.windows.net/powerbi/api/.default" };

      /* If we successfully read the appId environment variable we'll get the OAuth token using an app registration,
          otherwise we'll use the system managed identity on the Azure Functions application */
      if (appId != null) {
        string appSecret = Environment.GetEnvironmentVariable("app_secret");
        string tenentId = Environment.GetEnvironmentVariable("tenant_id");
        string tenantSpecificAuthority = "https://login.microsoftonline.com/" + tenentId;
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
