using '../main.bicep'

param env = 'Production'
param location = 'centralindia'
param uniqueSuffix = 'demo01'

// Backend web app
param apiWebAppName = 'app-shippulse-api-prod-demo01'
param appServicePlanName = 'asp-shippulse-prod-demo01'

// Key Vault must be globally unique and only letters/numbers
param keyVaultName = 'kvshippulseprodDemo01'

// Monitoring
param logAnalyticsName = 'law-shippulse-prod-demo01'
param appInsightsName = 'appi-shippulse-prod-demo01'
param staticWebAppName = 'swa-shippulse-prod-demo01'
param staticWebAppLocation = 'eastasia'

// Slightly stronger SKU if budget allows; keep B1 if needed
param skuName = 'B1'

// No VM in prod for this assignment
param enableJumpbox = false
