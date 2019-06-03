[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePlanName,
    [Parameter(Mandatory=$true)][ValidateSet("Free","Shared")] 
    [String] 
    $Tier
)

"Scale up: " + $ResourceGroupName + $ServicePlanName + " to: " + $Tier + " Plan"
Set-AzureRmAppServicePlan -ResourceGroupName $ResourceGroupName -Name $ServicePlanName -Tier $Tier