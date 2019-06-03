[CmdletBinding()]
param (
	$azureAplicationId,
	$azureTenantId,
	$azurePass,
    $ApiManagementServiceName,
    $ApiManagementServiceResourceGroup
)

try {
    $azurePassword = ConvertTo-SecureString $azurePasswordString -AsPlainText -Force
    $psCred = New-Object System.Management.Automation.PSCredential($azureAplicationId , $azurePassword)
    Add-AzureRmAccount -Credential $psCred -TenantId $azureTenantId  -ServicePrincipal
}
catch {
    Write-Error -Message $_.Exception
    throw $_.Exception
}

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