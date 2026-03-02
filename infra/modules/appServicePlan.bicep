param planName string
param location string
param skuName string

resource plan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: planName
  location: location
  sku: {
    name: skuName
    tier: (skuName == 'B1' ? 'Basic' : 'PremiumV3')
  }
  properties: {
    reserved: true // Linux
  }
}

output planId string = plan.id
