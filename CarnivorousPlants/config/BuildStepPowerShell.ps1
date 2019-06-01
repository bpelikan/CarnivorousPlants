[CmdletBinding()]
param (
    $ApiManagementServiceName,
    $ApiManagementServiceResourceGroup
)

$apimServiceName = $ApiManagementServiceName
$resourceGroupName = $ApiManagementServiceResourceGroup

Write-Host "Api Management Service Name: $($apimServiceName)"
Write-Host "Api Management Resource Group Name: $($resourceGroupName)"