param workspaceName string
param location string

resource law 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: workspaceName
  location: location
  properties: {
    retentionInDays: 30
  }
}

output workspaceId string = law.id
