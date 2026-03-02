param appName string
param location string
param planId string
param appInsightsConnectionString string
param env string
param keyVaultUri string

resource web 'Microsoft.Web/sites@2023-12-01' = {
  name: appName
  location: location
  kind: 'app,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: planId
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      minTlsVersion: '1.2'
      healthCheckPath: '/health'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: env
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        // Example: runtime secret via Key Vault reference (set SecretUri later after creating secret)
        {
          name: 'ExternalApi__ApiKey'
          value: '@Microsoft.KeyVault(SecretUri=' + keyVaultUri + 'secrets/ExternalApi--ApiKey/)'
        }
      ]
    }
  }
}

output hostname string = web.properties.defaultHostName
output principalId string = web.identity.principalId
