using '../main.bicep'

param env = 'Development'
param location = 'centralindia'

// Pick one suffix and keep it for the whole project
param uniqueSuffix = 'demo01'

// Backend web app (must be unique within your subscription)
param apiWebAppName = 'app-shippulse-api-dev-demo01'
param appServicePlanName = 'asp-shippulse-dev-demo01'

// Key Vault must be globally unique and only letters/numbers (no hyphens)
param keyVaultName = 'kvshippulsedevDemo01'

// Monitoring
param logAnalyticsName = 'law-shippulse-dev-demo01'
param appInsightsName = 'appi-shippulse-dev-demo01'
param staticWebAppName = 'swa-shippulse-dev-demo01'
param staticWebAppLocation = 'eastasia'

param skuName = 'B1'

// Enable VM for dev learning only after setting SSH values below
param enableJumpbox = false
param jumpboxVmName = 'vm-shippulse-jump-demo01'
param jumpboxAdminUsername = 'azureuser'

// Set these before deploying:
// 1) sshPublicKey: contents of ~/.ssh/id_rsa.pub (Cloud Shell or local)
// 2) sshAllowedCidr: your public IP in CIDR, e.g. 1.2.3.4/32
param jumpboxSshPublicKey = ''
param sshAllowedCidr = ''
