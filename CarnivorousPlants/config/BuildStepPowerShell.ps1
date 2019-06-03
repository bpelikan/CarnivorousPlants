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

$ResourceGroupName
$ServicePlanName
$Tier
