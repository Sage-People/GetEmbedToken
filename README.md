# GetEmbedToken
Ever wonder how to securely embed a Power BI report into your application when you can't use one of the native Power BI Embeded server side SDKs? Well look no further, GetEmbedToken is an C# Azure Function that assists in retrieving a Power BI Embed token so you can use it elsewhere in your application flow.

**Please note this codebase is for illustrative purposes only and should not be deployed to production environment.**

## Components
This project is made up of three components.

|Component|Description|
|---|---|
|AzureFunction|This is the main component of this project, this is the function that does the heavy lifting for you.|
|ExamplePage|This is a simple HTML page with JavaScript code to help illustrate how this would be used in your application. If you've deployed the function and test report you can access a deployed version of the exmaple page at https://pbitest.pages.dev to give your deployment a try.|
|TestReport|This is a test Power BI file that helps you see the Username being passed when embedding.|

## Environment Variables
This function supports the following environment variables for configuration. 
|Variable|Description|Required|
|---|---|---|
|AppId|When specififed the function will login to Power BI using the specified app registration instead of its default behavior which uses its managed identity.|No|
|AppSecret|Used in conjunction with AppId to specify the app registration secret.|With AppId|
|TenantId|Used to specify the Azure Active Directory tenant to authenticate the App Registration with when AppId is specified|No|
|WorkspaceId|Guid which identifies the default Power BI workspace to use with this function, it can be overrode at execution time by specifying a 'workspacE_id'|Yes|

## Known Issues
* In this model reports need to be embedded using Java Script so your deployment requires proper CORS headers
* Calling the Azure function needs to be done using server-side code, calling the function client size will lead to an insecure deployment.

## Future Enhancements
This example is still fairly rough, and I'm working to add some additional enhancements so it's more useful for users wishing to repeat the pattern. Here are the items currently planned. 
* ~~Thorough commenting of code to make this example more meaningful.~~ (Complete)
* ~~Enhancement to the example page so you don't have to copy and paste the embed token into it to test your deployment~~ (Complete)
* Better error handling in the Azure function (checking to see if a report exists, checking if the right roles are being used, etc.)
* Additional configuratbility using environment variables (e.g. Token TTL)
* ~~Utilization of the function's Service Principal instead of an App Registration for authenticating to Power BI.~~ (Complete)
* Read WorkspaceId from the HTTP request for more flexibility.

## Credit
This project was originally forked from [PowerBIDevCamp](https://github.com/PowerBiDevCamp/GetEmbedToken), and enhanced to enable the use of effective identities. When I started working on this project I had a rough idea on what I needed to accomplish and their version of the code really got me moving in the right direction.