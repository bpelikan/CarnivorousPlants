[CmdletBinding()]
param (
	# $TenantId,
	# $ApplicationId,
	# $CertificateThumbprint,
    $ApiManagementServiceName,
    $ApiManagementServiceResourceGroup
)

# try
# {

    # "Logging in to Azure..."
    # Add-AzureRmAccount `
        # -ServicePrincipal `
        # -TenantId $servicePrincipalConnection.TenantId `
        # -ApplicationId $servicePrincipalConnection.ApplicationId `
        # -CertificateThumbprint $servicePrincipalConnection.CertificateThumbprint 
# }
# catch {
    # Write-Error -Message $_.Exception
	# throw $_.Exception
# }


$apimServiceName = $ApiManagementServiceName
$resourceGroupName = $ApiManagementServiceResourceGroup

# Get-AzureRmResourceGroup | ft

Write-Host "Api Management Service Name: $($apimServiceName)"
Write-Host "Api Management Resource Group Name: $($resourceGroupName)"