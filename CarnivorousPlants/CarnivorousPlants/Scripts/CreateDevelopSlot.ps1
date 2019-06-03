[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePlanName,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $WebAppServiceName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String] 
    $SlotName
)

"Create Slot:"
"Resource group name: " + $ResourceGroupName
"App Service Plan name: " + $ServicePlanName
"Webb App Service Plan name: " + $WebAppServiceName
"Slot name: "  + $SlotName
New-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $WebAppServiceName -AppServicePlan $ServicePlanName -Slot $SlotName