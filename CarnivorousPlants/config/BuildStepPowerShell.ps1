[CmdletBinding()]
param (
	# $AzureAplicationId,
	# $AzureTenantId,
	# $AzurePass,
    $ApiManagementServiceName,
    $ApiManagementServiceResourceGroup
)

# try {
#     $azurePassword = ConvertTo-SecureString $AzurePass -AsPlainText -Force
#     $psCred = New-Object System.Management.Automation.PSCredential($AzureAplicationId , $azurePassword)
#     Add-AzureRmAccount -Credential $psCred -TenantId $AzureTenantId  -ServicePrincipal
# }
# catch {
#     Write-Error -Message $_.Exception
#     throw $_.Exception
# }

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