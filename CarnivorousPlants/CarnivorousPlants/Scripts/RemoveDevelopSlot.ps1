[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $WebAppServiceName
)

$slots = Get-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $WebAppServiceName
$slots.GetEnumerator() | Remove-AzureRmWebAppSlot -Force

#########
#[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    #[String]
    #$SlotName

#"Remove Slots:"
#"Resource group name: " + $ResourceGroupName
#"Webb App Service Plan name: " + $WebAppServiceName
#"Slot name: " + $SlotName

# Remove-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $WebAppServiceName -Slot $SlotName -Force