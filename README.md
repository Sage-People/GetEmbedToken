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

## Variables & Request Parameters
This function supports various environment variables and request parameters to configure the report being embedded. The table below shows all of the configuration items and where they can be used. In general if a configuration can be set in the environment
or the request parameter, the request parameter will take priority. 

|Name|Environment|Request|Description|
|---|---|---|---|
|app_id|X||When specififed the function will login to Power BI using the specified app registration instead of its default behavior which uses its managed identity.|
|app_secret|X||Used in conjunction with AppId to specify the app registration secret.|
|tenant_id|X||Used to specify the Azure Active Directory tenant to authenticate the App Registration with when AppId is specified|
|token_timeout|X|X|Specifies how long the Embed Token should live for in minutes.|
|workspace_id|X|X|Guid which identifies the Power BI workspace to use with this function.|
|report_id||X|GUID which identifies the Power BI report to embed.|
|username||X|The effective username to pass through to the PowerBI report. This can be any string, and will surface as Username() in the report.|
|roles||X|Comma separated list of roles to apply the Embed Token to. This must match the name of your roles in your report, and there must be at least one role defined in the report for this to work.|


## Known Issues
* In this model reports need to be embedded using Java Script so your deployment requires proper CORS headers
* Calling the Azure function needs to be done using server-side code, calling the function client size will lead to an insecure deployment.
* In order for Effective Identity to be available, your Power BI report must have at least one role and that role must be listed in the `roles` configuration item.

## Future Enhancements
This example is still fairly rough, and I'm working to add some additional enhancements so it's more useful for users wishing to repeat the pattern. Here are the items currently planned. 
* ~~Thorough commenting of code to make this example more meaningful.~~ (Complete)
* ~~Enhancement to the example page so you don't have to copy and paste the embed token into it to test your deployment~~ (Complete)
* Better error handling in the Azure function (checking to see if a report exists, checking if the right roles are being used, etc.)
* ~~Additional configuratbility using environment variables (e.g. Token TTL)~~ (Complete)
* ~~Utilization of the function's Service Principal instead of an App Registration for authenticating to Power BI.~~ (Complete)
* ~~Read WorkspaceId from the HTTP request for more flexibility.~~ (Complete)
* Add support to install the function in your environment with Azure Templates.

## Credit
This project was originally forked from [PowerBIDevCamp](https://github.com/PowerBiDevCamp/GetEmbedToken), and enhanced to enable the use of effective identities. When I started working on this project I had a rough idea on what I needed to accomplish and their version of the code really got me moving in the right direction. 