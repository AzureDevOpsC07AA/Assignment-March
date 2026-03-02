// ShipPulse Infrastructure (Bicep)
// Deploy from Azure Cloud Shell or GitHub Actions.
//
// NOTE: Key Vault and Static Web Apps names must be globally unique.
// Use the provided UNIQUE_SUFFIX in bicepparam files.

param env string
param location string = resourceGroup().location
param uniqueSuffix string

@description('Backend web app name')
param apiWebAppName string

@description('App Service plan name')
param appServicePlanName string

@description('Key Vault name (global unique)')
param keyVaultName string

@description('Log Analytics workspace name')
param logAnalyticsName string

@description('Application Insights name')
param appInsightsName string

@description('Static Web App name (global unique)')
param staticWebAppName string

@description('Static Web App location. Static Web Apps is not available in all Azure regions.')
param staticWebAppLocation string = 'eastasia'

@description('App Service SKU, e.g. B1 (dev) or P1v3 (prod-lite)')
param skuName string = 'B1'

@description('Enable jumpbox VM (recommended only for dev)')
param enableJumpbox bool = false

@description('Jumpbox VM name')
param jumpboxVmName string = 'vm-shippulse-jump'

@description('Admin username for VM')
param jumpboxAdminUsername string = 'azureuser'

@description('SSH public key for VM (only needed if enableJumpbox=true)')
param jumpboxSshPublicKey string = ''

@description('Allowed public IP CIDR for SSH (example: 1.2.3.4/32). Only used if enableJumpbox=true')
param sshAllowedCidr string = ''

module logAnalytics 'modules/logAnalytics.bicep' = {
  name: 'law-${env}-${uniqueSuffix}'
  params: {
    workspaceName: logAnalyticsName
    location: location
  }
}

module appInsights 'modules/appInsights.bicep' = {
  name: 'appi-${env}-${uniqueSuffix}'
  params: {
    appInsightsName: appInsightsName
    location: location
    workspaceId: logAnalytics.outputs.workspaceId
  }
}

module keyVault 'modules/keyVault.bicep' = {
  name: 'kv-${env}-${uniqueSuffix}'
  params: {
    vaultName: keyVaultName
    location: location
  }
}

module appPlan 'modules/appServicePlan.bicep' = {
  name: 'asp-${env}-${uniqueSuffix}'
  params: {
    planName: appServicePlanName
    location: location
    skuName: skuName
  }
}

module webApp 'modules/webApp.bicep' = {
  name: 'api-${env}-${uniqueSuffix}'
  params: {
    appName: apiWebAppName
    location: location
    planId: appPlan.outputs.planId
    appInsightsConnectionString: appInsights.outputs.connectionString
    env: env
  }
}

module staticWebApp 'modules/staticWebApp.bicep' = {
  name: 'swa-${env}-${uniqueSuffix}'
  params: {
    appName: staticWebAppName
    location: staticWebAppLocation
  }
}

// Optional: Jumpbox VM for learning (Nginx + self-hosted runner)
module jumpbox 'modules/jumpboxVm.bicep' = if (enableJumpbox) {
  name: 'jump-${env}-${uniqueSuffix}'
  params: {
    location: location
    vmName: jumpboxVmName
    adminUsername: jumpboxAdminUsername
    sshPublicKey: jumpboxSshPublicKey
    sshAllowedCidr: sshAllowedCidr
  }
}

output keyVaultUri string = keyVault.outputs.vaultUri
output webAppHostname string = webApp.outputs.hostname
output staticWebAppHostname string = staticWebApp.outputs.hostname
