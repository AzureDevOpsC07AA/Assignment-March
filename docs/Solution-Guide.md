# ShipPulse Solution Guide

This guide is for your reference. It is the answer key, unblock guide, and review reference for Ram and Prasanth.

## Expected Final Architecture

Each student should end with:

- one private repo in `AzureDevOpsC07AA`,
- one Ubuntu VM in their own Azure subscription,
- one dev deployment path from `develop`,
- one prod deployment path from `main`,
- backend hosted in App Service,
- frontend hosted in Static Web Apps,
- Key Vault, Application Insights, and Log Analytics present in the solution,
- evidence that GitHub Actions is being used for CI and CD.

## Expected Student Repos

- Ram: `AzureDevOpsC07AA/shippulse-ram`
- Prasanth: `AzureDevOpsC07AA/shippulse-prasanth`

Both repos should be private and isolated.

## Expected Branch Model

- `develop` for dev deployment
- `main` for prod deployment

This must match the current workflow logic in:

- [deploy-backend.yml](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/.github/workflows/deploy-backend.yml)
- [deploy-frontend.yml](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/.github/workflows/deploy-frontend.yml)

## Expected Resource Naming Pattern

Students should replace the sample `demo01` suffix with their own suffix and use it consistently in:

- [infra/env/dev.bicepparam](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/infra/env/dev.bicepparam)
- [infra/env/prod.bicepparam](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/infra/env/prod.bicepparam)

### Acceptable examples

Ram:

- `ram01`
- `ramc07`

Prasanth:

- `pra01`
- `prac07`

### Rules

- Keep the same suffix across dev and prod
- Keep names readable
- Keep Key Vault names alphanumeric only
- Keep Key Vault globally unique

## Recommended Execution Sequence

1. Create repo in `AzureDevOpsC07AA`
2. Create Ubuntu VM manually in Azure Portal
3. SSH from Windows into Ubuntu
4. Install Git, .NET SDK, and Azure CLI
5. Clone repo inside Ubuntu
6. Create and use `develop`
7. Run `dotnet test`
8. Run backend locally and verify `/health`
9. Run frontend locally
10. Update `dev.bicepparam` and `prod.bicepparam`
11. Configure Azure identity and GitHub secrets
12. Push to `develop`
13. Verify dev deployment
14. Promote to `main`
15. Verify prod deployment

## Expected Azure Auth Model

The preferred classroom path is:`r`n`r`nImportant terminology:`r`n- `App registration` defines the application identity.`r`n- `Service principal` or `Enterprise application` is the Azure-side security principal that receives IAM role assignments.`r`n

- Azure App Registration
- GitHub OIDC federated credentials
- No stored client secret in GitHub
- Resource-group level `Contributor` role assignment for the app registration

### What we should expect 

1. One app registration per student
2. Two federated credentials per app registration
   - one for `develop`
   - one for `main`
3. Role assignment on the dev resource group
4. Role assignment on the prod resource group if prod is separate

### Values that must be collected

- `Application (client) ID` -> `AZURE_CLIENT_ID`
- `Directory (tenant) ID` -> `AZURE_TENANT_ID`
- Azure subscription ID -> `AZURE_SUBSCRIPTION_ID`

### Correct GitHub federated credential settings

- GitHub organization: `AzureDevOpsC07AA`
- Repository:
  - Ram: `shippulse-ram`
  - Prasanth: `shippulse-prasanth`
- Branch entries required:
  - `develop`
  - `main`

### Review warning

If the student creates the app registration correctly but forgets the federated credential for one branch, one environment will work and the other will fail. This is a common issue and should be checked early.
## Expected GitHub Secrets

Ensure to provide values for these names.

### Backend-related

- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`
- `AZURE_RG_DEV`
- `AZURE_RG_PROD`
- `AZURE_WEBAPP_NAME_DEV`
- `AZURE_WEBAPP_NAME_PROD`

### Frontend-related

- `AZURE_STATIC_WEB_APPS_API_TOKEN_DEV`
- `AZURE_STATIC_WEB_APPS_API_TOKEN_PROD`
- `API_BASE_URL_DEV`
- `API_BASE_URL_PROD`

### Review note

The current repo expects backend Azure login values as plain secret names, not separate `_DEV` and `_PROD` variants. Students may still organize them at repo or environment level, but the workflow names must match the YAML.

### How you should obtain the values

- `AZURE_CLIENT_ID`
  - From the Azure app registration overview page
- `AZURE_TENANT_ID`
  - From the Azure app registration overview page
- `AZURE_SUBSCRIPTION_ID`
  - From `Subscriptions` in Azure Portal or `az account show --query id -o tsv`
- `AZURE_STATIC_WEB_APPS_API_TOKEN_DEV` and `AZURE_STATIC_WEB_APPS_API_TOKEN_PROD`
  - From each Static Web App under `Manage deployment token`
- `API_BASE_URL_DEV` and `API_BASE_URL_PROD`
  - From each App Service default hostname, prefixed with `https://`

## What Should Happen on `develop`

### Backend workflow expected behavior

1. Checkout
2. Setup .NET
3. Restore
4. Build
5. Test
6. Publish backend
7. Upload artifact
8. Download artifact in deploy job
9. Azure login
10. `az deployment group create` using `infra/env/dev.bicepparam`
11. Deploy published package to dev App Service

### Frontend workflow expected behavior

1. Checkout
2. Copy `appsettings.template.json` to `appsettings.json`
3. Replace `__API_BASE_URL__` with `API_BASE_URL_DEV`
4. Publish Blazor app
5. Deploy to dev Static Web App using `AZURE_STATIC_WEB_APPS_API_TOKEN_DEV`

## What Should Happen on `main`

### Backend workflow expected behavior

Uses `infra/env/prod.bicepparam` and deploys the API to the prod App Service.

### Frontend workflow expected behavior

Uses `API_BASE_URL_PROD` and `AZURE_STATIC_WEB_APPS_API_TOKEN_PROD` to deploy the production frontend.

## Review Checklist by Capability

### Repo setup

- Correct repo name
- Private visibility
- `develop` and `main` both exist

### VM setup

- Ubuntu VM exists in the student subscription
- Student can explain SSH access
- Student can show Git, .NET, and Azure CLI on Ubuntu

### Backend local run

- `dotnet test` passes
- `/health` works locally
- Feedback endpoints respond locally

### Frontend local run

- Blazor app builds and starts from Ubuntu
- Student understands how the API base URL is injected in deployment

### CI

- Student can locate Actions runs
- Student can explain where build and test happen

### CD

- Student can explain dev deployment from `develop`
- Student can explain prod deployment from `main`

### Monitoring

- Student can identify Application Insights
- Student can identify Log Analytics

### Secrets

- Student knows where secrets live
- Student can describe the difference between GitHub secrets and Key Vault

## Common Student Mistakes and Fixes

### Mistake: working on the Windows laptop instead of the Ubuntu VM

Fix:

- Stop them early and redirect them to the VM-first model.
- Ask them to show the repo path and shell prompt from Ubuntu.

### Mistake: repo created under personal GitHub account

Fix:

- Move them back to `AzureDevOpsC07AA`.
- Have them recreate the repo if necessary.

### Mistake: no `develop` branch

Fix:

```bash
git checkout -b develop
git push -u origin develop
```

### Mistake: backend workflow fails at Azure login

Fix:

- Re-check `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`
- Re-check federated credential or service principal access
- Confirm the federated credential repository is exactly `AzureDevOpsC07AA/<student-repo-name>`
- Confirm the federated credential branch matches the branch that triggered the workflow
- Confirm the app registration has `Contributor` access on the correct resource group

### Mistake: Bicep deploy fails because of bad names

Fix:

- Re-check unique suffix
- Re-check Key Vault naming rules
- Re-check resource group names in secrets

### Mistake: frontend deploy succeeds but app cannot reach backend

Fix:

- Re-check `API_BASE_URL_DEV` or `API_BASE_URL_PROD`
- Confirm it points to the deployed App Service URL, not `localhost`

### Mistake: student assumes Key Vault is fully automated

Fix:

- Show them the app setting reference in [webApp.bicep](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/infra/modules/webApp.bicep)
- Ask them whether the secret itself exists
- Ask them whether the web app identity has access

## Known Repo Nuance to Explain Clearly

The repo currently provisions Key Vault and writes a sample Key Vault reference into App Service:

- app setting name: `ExternalApi__ApiKey`
- expected secret path shape: `secrets/ExternalApi--ApiKey/`

What is not fully automated here:

1. creating the actual secret value in Key Vault,
2. ensuring the app identity has the required access end to end,
3. proving the app consumes that secret in a visible functional path.

Students should not be told this is already complete. They should be told to verify it.

## Quick Verification Commands

Run from Ubuntu:

```bash
dotnet test
dotnet run --project src/ShipPulse.Api
curl -X GET "http://localhost:5080/health"
curl -X POST "http://localhost:5080/api/feedback" -H "Content-Type: application/json" -d "{\"message\":\"verification message\",\"createdBy\":\"review\"}"
curl -X GET "http://localhost:5080/api/feedback"
dotnet run --project src/ShipPulse.Web
```

## Review Talking Points for Farees

Use these questions in the week-later discussion:

1. Why did we force a VM-first workflow?
2. What does `develop` deploy and what does `main` deploy?
3. Why are frontend and backend deployed by separate workflows?
4. Where is the backend URL injected into the frontend deployment?
5. Why is Key Vault different from GitHub Actions secrets?
6. What did Application Insights and Log Analytics each contribute?
7. Which parts are automated and which parts still require manual verification?

## Recovery Paths

### If the VM is broken

1. Create a new VM in the same subscription.
2. Reconnect with SSH.
3. Reinstall tools.
4. Re-clone the repo.

### If Azure deployment partially succeeds

1. Read the failing workflow step.
2. Compare the actual Azure resources with the parameter file names.
3. Fix naming or permission issues first.
4. Re-run the workflow.

### If GitHub OIDC is misconfigured

1. Verify the federated credential subject and audience.
2. Verify the app registration has the right subscription or resource group access.
3. Re-run only after the login issue is fixed.

### If secrets are missing

1. Identify which exact secret name the workflow expects.
2. Add the missing secret in the correct repo or environment scope.
3. Re-run the workflow.

## Rubric

### Mandatory

- Correct repo
- Correct VM-first workflow
- Local backend run
- Local frontend run
- Working `develop` pipeline
- Working `main` pipeline
- Clear explanation of Azure services used

### Good submission

- Student can explain failures they hit and how they fixed them
- Student can show monitoring evidence
- Student can explain the difference between infrastructure deployment and application deployment

### Stretch

- Student documents a Logic App based VM start routine
- Student improves naming consistency
- Student identifies the Key Vault automation gap without prompting


