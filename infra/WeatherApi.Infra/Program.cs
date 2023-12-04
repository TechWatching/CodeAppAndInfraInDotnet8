using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using System.Collections.Generic;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using Deployment = Pulumi.Deployment;

return await Pulumi.Deployment.RunAsync(() =>
{
    var stackName = Deployment.Instance.StackName;
    
    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup($"rg-dotnetconf-{stackName}", new()
    {
        Tags =
        {
            { "Type", "Talk" }
        }
    });
    
    var appServicePlan = new AppServicePlan($"sp-dotnetconf-{stackName}", new()
    {
        ResourceGroupName = resourceGroup.Name,
        Sku = new SkuDescriptionArgs()
        {
            Name = "F1",
        },
    });
    
    var appService = new WebApp($"app-dotnetconf-{stackName}", new ()
    {
        ResourceGroupName = resourceGroup.Name,
        ServerFarmId = appServicePlan.Id,
    });

    // Export the primary key of the Storage Account
    return new Dictionary<string, object?>
    {
        ["AppServiceUrl"] = Output.Format($"https://{appService.DefaultHostName}")
    };
});