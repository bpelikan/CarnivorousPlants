[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePlanName,
    [Parameter(Mandatory=$true)][ValidateSet("Free","Shared","Basic","Standard")] 
    [String] 
    $Tier
)

"App Service Plan scale up:"
"Resource group name: " + $ResourceGroupName
"App Service Plan name: " + $ServicePlanName
"To App Service Plan: "  + $Tier
Set-AzureRmAppServicePlan -ResourceGroupName $ResourceGroupName -Name $ServicePlanName -Tier $Tier