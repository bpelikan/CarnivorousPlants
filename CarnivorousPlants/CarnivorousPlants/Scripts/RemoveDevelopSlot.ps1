[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $WebAppServiceName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $SlotName
)

"Create Slot:"
"Resource group name: " + $ResourceGroupName
"Webb App Service Plan name: " + $WebAppServiceName
"Slot name: " + $SlotName
Remove-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $WebAppServiceName -Slot $SlotName -Force