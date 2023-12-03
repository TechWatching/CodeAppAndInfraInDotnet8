using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;

var cancellationToken = new CancellationTokenSource();
var ct = cancellationToken.Token;
Console.CancelKeyPress += (sender, eventArgs) =>
{
    cancellationToken.Cancel();
};

Console.WriteLine("Infrastructure provisioning started");

try
{
    var client = new ArmClient(new DefaultAzureCredential());
    var subscription = await client.GetDefaultSubscriptionAsync(ct);

    var resourceGroupCollection = subscription.GetResourceGroups();
    var resourceGroupOperation = await resourceGroupCollection.CreateOrUpdateAsync(WaitUntil.Completed, "rg-dotnetconf-01",
        new(AzureLocation.WestEurope)
        {
            Tags = { ["Type"] = "Talk" }
        }, ct);
    var resourceGroup = resourceGroupOperation.Value;

    var appServicePlanCollection = resourceGroup.GetAppServicePlans();
    var servicePlanOperation = await appServicePlanCollection.CreateOrUpdateAsync(WaitUntil.Completed, "plan-dotnetconf-01",
        new(AzureLocation.WestEurope)
        {
            Kind = "app",
            Sku = new()
            {
                Name = "F1"
            }
        }, ct);
    var servicePlan = servicePlanOperation.Value;

    var webSiteCollection = resourceGroup.GetWebSites();
    var siteOperation = await webSiteCollection.CreateOrUpdateAsync(WaitUntil.Completed, "app-dotnetconf-01", 
        new(AzureLocation.WestEurope)
        {
            AppServicePlanId = servicePlan.Id
        }, ct);
    var appService = siteOperation.Value;

}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

Console.WriteLine("Infrastructure provisioning completed");