# ShipPulse - Starter Repo (Backend + Frontend + IaC)

This repository is used for the **ShipPulse Startup DevOps Assignment (7 days)**.

# System Requirements
https://dotnet.microsoft.com/en-us/download

# Ubuntu Setup
Incase you are unable to install any of those, you can use Ubuntu VM from Azure and install there and use it

Install dotnet in Ubuntu
https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet10&pivots=os-linux-ubuntu-2404


## Training org and repo model
Use the shared GitHub org: **AzureDevOpsC07AA**

Each engineer must create a separate private repo in the same org:
- Engineer 1: `AzureDevOpsC07AA/shippulse-eng1`
- Engineer 2: `AzureDevOpsC07AA/shippulse-eng2`

Both engineers use the **same starter code**, but deploy into their own Azure resources using different suffixes:
- Engineer 1 suffix: `e1c07aa`
- Engineer 2 suffix: `e2c07aa`

## Tech
- Backend: .NET 8 Minimal API (`src/ShipPulse.Api`)
- Frontend: Blazor WebAssembly (`src/ShipPulse.Web`) - builds using **dotnet only** (no Node required)
- IaC: Bicep (`infra/`) - deploy from **Azure Cloud Shell** or GitHub Actions
- Azure Targets:
  - Ubuntu VM (Jumpbox + optional self-hosted runner)
  - Azure App Service (Backend)
  - Azure Static Web Apps (Frontend)
  - Key Vault (runtime secrets)
  - Application Insights + Log Analytics (monitoring)
  - Logic App (start a stopped VM on schedule)

## Region
Use **Central India**.
If Azure blocks creation due to temporary quota/capacity, use **South India** as the fallback and document it in your submission.

## Quick local run
Backend:
```bash
dotnet restore
dotnet run --project src/ShipPulse.Api
```

Frontend:
```bash
dotnet run --project src/ShipPulse.Web
```

Default local URLs:
- Backend: http://localhost:5080
- Frontend: http://localhost:5170

You should see Message can be seen below

Available endpoint to test in web for Backend
http://localhost:5080/health

Note for Backend you can use curl to get response, use below command
Health Check
This command checks the health of the API.
```bash
curl -X GET "http://localhost:5080/health"
```

Submit New Feedback
This command posts a new feedback message.
```bash
curl -X POST "http://localhost:5080/api/feedback" -H "Content-Type: application/json" -d "{\"message\":\"This is a test message from curl\",\"createdBy\":\"developer\"}"
```

Get All Feedback
This command retrieves a list of all submitted feedback items.
```bash
curl -X GET "http://localhost:5080/api/feedback"
```


## Important files you will edit
- `infra/env/dev.bicepparam`
- `infra/env/prod.bicepparam`
- `.github/workflows/deploy-backend.yml`
- `.github/workflows/deploy-frontend.yml`
- `src/ShipPulse.Web/Shared/MainLayout.razor`

## Docs
See `docs/printable/` for the final printable handouts.
