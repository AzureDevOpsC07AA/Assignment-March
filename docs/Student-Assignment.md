This is merge commit tester
# ShipPulse Student Assignment

Ram and Prasanth, welcome to ShipPulse.

You both joined a startup that already has a product skeleton, a cloud direction, and a deadline. Farees has handed you the starter repo. Your job is not only to write code. Your job is to prove you can own the full engineering flow: local development on Ubuntu, Git branching, CI visibility, Azure deployment, and production-style verification.

Ram owns frontend delivery. Prasanth owns backend delivery. Both of you must still understand the full pipeline end to end.

## Mission

### Goal

Complete the ShipPulse assignment from your own repo, your own Ubuntu VM, and your own Azure subscription.

### Success Criteria

You are done only when all of these are true:

1. You created your own repo in `AzureDevOpsC07AA`.
2. You created your own Ubuntu VM and used it as the main development machine.
3. You ran backend and frontend locally from Ubuntu.
4. You updated the infrastructure parameter files with your own suffix and names.
5. You configured GitHub Actions for dev and prod.
6. You deployed backend and frontend to Azure.
7. You verified monitoring, deployment results, and runtime configuration.

## Repo You Must Create

### Goal

Create your own working repo without overwriting the other engineer.

### Where to do this

GitHub web UI.

### Exact steps

1. Open the GitHub organization `AzureDevOpsC07AA`.
2. Create a new private repository.
3. Use your assigned repo name:
   - Ram: `shippulse-ram`
   - Prasanth: `shippulse-prasanth`
4. Start from this starter repo by either:
   - forking the starter into the org, or
   - creating a fresh repo and pushing this starter code into it
5. Confirm the default branch is `main`.
6. Create a second branch named `develop`.

### Expected result

You have an isolated repo with both `main` and `develop`.

### If blocked, check this

- Confirm you are working inside `AzureDevOpsC07AA`.
- Confirm the repo is private.

## Phase 1: Prepare the Ubuntu Development Machine

### Goal

Create the Ubuntu VM and make it ready for ShipPulse work.

### Where to do this

Azure Portal and your Ubuntu shell over SSH.

### Exact steps

1. Follow [UBUNTU_SETUP.md](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/UBUNTU_SETUP.md) completely.
2. Do not continue until these commands work from Ubuntu:

```bash
git --version
dotnet --info
az version
```

### Expected result

Ubuntu is your working machine and is ready for Git, .NET, and Azure commands.

### If blocked, check this

- Do not switch to your Windows laptop to continue coding.
- Fix the Ubuntu setup first.

## Phase 2: Clone the Repo into Ubuntu

### Goal

Get your own ShipPulse repo onto the VM.

### Where to do this

Ubuntu terminal.

### Exact steps

1. Create a working folder:

```bash
mkdir -p ~/work
cd ~/work
```

2. Clone your repo:

Ram:

```bash
git clone git@github.com:AzureDevOpsC07AA/shippulse-ram.git
```

Prasanth:

```bash
git clone git@github.com:AzureDevOpsC07AA/shippulse-prasanth.git
```

3. Enter the repo:

```bash
cd shippulse-ram
```

or

```bash
cd shippulse-prasanth
```

4. Fetch all branches:

```bash
git fetch --all
```

5. Switch to `develop`:

```bash
git checkout develop
```

### Expected result

The repo exists on Ubuntu and you are working on `develop`.

### If blocked, check this

- If clone fails, verify GitHub SSH setup or use HTTPS temporarily.
- If `develop` does not exist, create it from `main`.

## Phase 3: Understand the Product Layout

### Goal

Know which part of the repo controls which part of the system.

### Where to do this

Your repo in Ubuntu and GitHub.

### Exact steps

Review these locations:

- Backend API: `src/ShipPulse.Api`
- Frontend app: `src/ShipPulse.Web`
- Tests: `src/ShipPulse.Tests`
- Infrastructure: `infra`
- Backend workflow: `.github/workflows/deploy-backend.yml`
- Frontend workflow: `.github/workflows/deploy-frontend.yml`

### Expected result

You know where application code, tests, infrastructure, and workflows live.

### If blocked, check this

- Use the top-level [README.md](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/README.md) for the architecture overview.

## Phase 4: Run the Backend Locally

### Goal

Prove the API works on Ubuntu before deploying anything.

### Where to do this

Ubuntu terminal.

### Exact steps

1. From the repo root, restore packages:

```bash
dotnet restore
```

2. Run the tests:

```bash
dotnet test src/ShipPulse.Tests/ShipPulse.Tests.csproj
```

3. Start the API:

```bash
dotnet run --project src/ShipPulse.Api
```

4. In a second SSH tab, verify health:

```bash
curl -X GET "http://localhost:5080/health"
```

5. Submit sample feedback:

```bash
curl -X POST "http://localhost:5080/api/feedback" -H "Content-Type: application/json" -d "{\"message\":\"Message from local Ubuntu test\",\"createdBy\":\"student\"}"
```

6. Read all feedback:

```bash
curl -X GET "http://localhost:5080/api/feedback"
```

### Expected result

The health endpoint returns `200 OK`, feedback can be created, and the tests pass.

### If blocked, check this

- If restore fails, check internet access from Ubuntu.
- If the API does not start, read the console output carefully before changing code.

## Phase 5: Run the Frontend Locally

### Goal

Prove the frontend builds and serves from Ubuntu.

### Where to do this

Ubuntu terminal.

### Exact steps

1. Stop the API only if you need the terminal back. Otherwise keep it running.
2. Start the frontend:

```bash
dotnet run --project src/ShipPulse.Web
```

3. Note the local URL shown by the app. The repo README uses `http://localhost:5170` as the expected default.
4. If you need to test from the Windows laptop browser, create an SSH tunnel or use a terminal browser strategy that you already know. Do not change the assignment model to laptop-first.

### Expected result

The frontend starts successfully from Ubuntu.

### If blocked, check this

- If the build fails, inspect the first real error, not the last repeated one.
- If you cannot reach the UI visually from Windows, that does not invalidate the backend/frontend build success inside Ubuntu.

## Phase 6: Update Infrastructure Parameters

### Goal

Make the Azure resource names unique for your own subscription.

### Where to do this

Repo files on Ubuntu.

### Exact steps

1. Open these files:
   - [infra/env/dev.bicepparam](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/infra/env/dev.bicepparam)
   - [infra/env/prod.bicepparam](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/infra/env/prod.bicepparam)
2. Replace the sample suffix `demo01` with your own suffix.
3. Keep the suffix consistent across dev and prod.
4. Update the resource names so they remain unique and readable.
5. Pay special attention to Key Vault names:
   - must be globally unique
   - letters and numbers only
   - no hyphens
6. Keep `enableJumpbox = false` unless you intentionally want to test the optional Bicep-managed jumpbox path and you have real SSH values ready.
7. Keep `enableJumpbox = false` for prod.
8. Keep `location = 'centralindia'` unless Azure capacity forces you to change it.
9. Keep `staticWebAppLocation = 'eastasia'`. The validated deployment path uses `East Asia` for Static Web Apps.
10. If you are not using Bicep to create the VM because you already created it manually in the portal, document that choice in your notes so Farees can see your reasoning.

### Expected result

Both parameter files contain your own consistent, deployable naming pattern.

### If blocked, check this

- If a name collision happens, change only the suffix and keep the structure readable.
- If Azure rejects Key Vault naming, remove unsupported characters.

## Phase 7: Prepare Azure Authentication for GitHub Actions

### Goal

Create the Azure identity GitHub Actions will use to deploy backend infrastructure, backend code, and frontend code.

### Where to do this

Azure CLI in Ubuntu, Azure Portal, and GitHub repo settings.

### Exact steps

1. Sign in to Azure from Ubuntu:

```bash
az login
az account show
```

2. If you have more than one subscription, set the correct one:

```bash
az account set --subscription "<YOUR_SUBSCRIPTION_ID_OR_NAME>"
```

3. Get your subscription ID if you do not already know it:

```bash
az account show --query id -o tsv
```

4. Create a service principal with Contributor access. Run this from Ubuntu:

```bash
az ad sp create-for-rbac \
  --name "sp-shippulse-ram-github" \
  --role contributor \
  --scopes /subscriptions/<YOUR_SUBSCRIPTION_ID>
```

Prasanth should use a name such as `sp-shippulse-prasanth-github`.

5. The command returns JSON. Record these values:
   - `clientId`
   - `clientSecret`
   - `tenantId`
   - `subscriptionId`

6. Important terminology:
   - The app registration defines the identity.
   - The service principal, sometimes shown as an enterprise application, is the identity that actually receives Azure role assignments.

7. Create the GitHub secret payload file from Ubuntu:

```bash
cat > azure-credentials.json <<'EOF'
{
  "clientId": "<clientId>",
  "clientSecret": "<clientSecret>",
  "subscriptionId": "<subscriptionId>",
  "tenantId": "<tenantId>"
}
EOF
```

8. Verify the service principal can sign in:

```bash
az login --service-principal \
  --username "<clientId>" \
  --password "<clientSecret>" \
  --tenant "<tenantId>"
az account show
```

9. If your organization requires resource-group scoped access instead of subscription-wide access, assign `Contributor` only to the target resource group:

```bash
az role assignment create \
  --assignee <clientId> \
  --role Contributor \
  --scope /subscriptions/<subscriptionId>/resourceGroups/<YOUR_RESOURCE_GROUP_NAME>
```

10. Keep the full JSON content ready for the `AZURE_CREDENTIALS` GitHub secret in the next phase.

### Expected result

GitHub Actions can sign in to Azure by using a service principal stored in `AZURE_CREDENTIALS`.

### If blocked, check this

- If `az ad sp create-for-rbac` fails, confirm you have permission to create app registrations and role assignments.
- If service principal login fails, re-check the four JSON values before saving them into GitHub.

## Phase 8: Configure GitHub Environments and Secrets

### Goal

Provide every secret the workflows need for dev and prod.

### Where to do this

GitHub repo settings.

### Exact steps

1. Open your repo in GitHub.
2. Open `Settings > Environments`.
3. Create environments named `dev` and `prod`.
4. Decide whether you will store shared values as repository secrets or environment secrets.
   - If the value is the same for both environments, repository secrets are acceptable.
   - If the value is different for dev and prod, environment secrets are clearer.

5. Add the Azure login secret.

Required value:

- `AZURE_CREDENTIALS`
  - Value source: the full JSON content from `azure-credentials.json`

6. Add the backend deployment secrets.

Required values:

- `AZURE_RG_DEV`
  - Value source: the resource group name used for dev deployment
- `AZURE_RG_PROD`
  - Value source: the resource group name used for prod deployment
- `AZURE_WEBAPP_NAME_DEV`
  - Value source: the backend dev app name in `infra/env/dev.bicepparam`
- `AZURE_WEBAPP_NAME_PROD`
  - Value source: the backend prod app name in `infra/env/prod.bicepparam`

7. Create or confirm the backend Azure resources so you know the real names.
   - If you deploy infra first from the CLI, copy the actual created resource names from Azure Portal.
   - If you rely on workflow-created infra, confirm the names match the Bicep parameter files exactly.

8. Add the frontend deployment secrets.

- `AZURE_STATIC_WEBAPP_NAME_DEV`
- `AZURE_STATIC_WEBAPP_NAME_PROD`
- `API_BASE_URL_DEV`
- `API_BASE_URL_PROD`
- `ADDITIONAL_BICEP_PARAMS_DEV`
- `ADDITIONAL_BICEP_PARAMS_PROD`

9. Save the Static Web App names exactly as deployed by your Bicep parameters.
   - Save the dev name as `AZURE_STATIC_WEBAPP_NAME_DEV`
   - Save the prod name as `AZURE_STATIC_WEBAPP_NAME_PROD`
   - The workflow signs in to Azure and fetches the deployment token automatically. Do not create manual deployment token secrets unless you are debugging.

10. To get the correct API base URL values:
    - Open the dev App Service overview page
    - Copy the default hostname and build the URL as `https://<your-dev-app-service-hostname>`
    - Save it as `API_BASE_URL_DEV`
    - Repeat for prod and save it as `API_BASE_URL_PROD`

11. Add the optional Bicep override secrets if you want GitHub Actions to enforce your final names directly from repo settings.

Example for dev:

```text
uniqueSuffix=ram01 apiWebAppName=app-shippulse-api-dev-ram01 appServicePlanName=asp-shippulse-dev-ram01 keyVaultName=kvshippulsedevram01 logAnalyticsName=law-shippulse-dev-ram01 appInsightsName=appi-shippulse-dev-ram01 staticWebAppName=swa-shippulse-dev-ram01 staticWebAppLocation=eastasia location=centralindia enableJumpbox=false
```

Create one value for `ADDITIONAL_BICEP_PARAMS_DEV` and one for `ADDITIONAL_BICEP_PARAMS_PROD`.

12. Double-check that the values line up with the Bicep parameter files.
     - `AZURE_WEBAPP_NAME_DEV` must match `apiWebAppName` in `infra/env/dev.bicepparam`
     - `AZURE_WEBAPP_NAME_PROD` must match `apiWebAppName` in `infra/env/prod.bicepparam`
     - `AZURE_STATIC_WEBAPP_NAME_DEV` must match the dev `staticWebAppName`
     - `AZURE_STATIC_WEBAPP_NAME_PROD` must match the prod `staticWebAppName`
     - `staticWebAppLocation` should remain `eastasia`

13. If you want extra protection, add required reviewers to the `prod` environment before allowing production deployment.

### Expected result

Your repo has the required secrets for both workflows.

### If blocked, check this

- If a frontend deployment fails immediately, the Static Web App name or `AZURE_CREDENTIALS` value is a likely issue.
- If the frontend deploys but cannot call the API, the `API_BASE_URL_*` values are likely wrong.

## Phase 9: Commit and Push to `develop`

### Goal

Trigger dev CI/CD and observe the result.

### Where to do this

Ubuntu terminal and GitHub Actions tab.

### Exact steps

1. Review your changes:

```bash
git status
```

2. Add and commit:

```bash
git add .
git commit -m "Configure ShipPulse dev environment"
```

3. Push to `develop`:

```bash
git push origin develop
```

4. Open the GitHub Actions tab.
5. Watch both workflows.
6. Read each failed step completely if something breaks.

### Expected result

The backend workflow builds, tests, publishes, deploys infra for dev, and deploys the API. The frontend workflow builds and deploys the frontend to dev.

### If blocked, check this

- If backend login fails, re-check `AZURE_CREDENTIALS`.
- If backend infra deployment fails, re-check `infra/env/dev.bicepparam`.
- If frontend config fails, re-check `API_BASE_URL_DEV`.

## Phase 10: Verify the Dev Environment

### Goal

Confirm the deployed dev system actually works.

### Where to do this

Azure Portal, deployed app URLs, and GitHub Actions logs.

### Exact steps

1. Open the dev App Service.
2. Copy its URL and open `/health`.
3. Confirm the backend responds successfully.
4. Open the dev Static Web App URL.
5. Confirm the frontend loads.
6. Check Application Insights and Log Analytics for signs of the running application.
7. Open Key Vault and verify whether your expected secret exists.

### Expected result

Dev deployment is reachable and observable.

### If blocked, check this

- If the backend deployed but `/health` fails, inspect App Service logs and configuration.
- If monitoring is empty, make sure the app is actually receiving traffic.
- If the frontend URL opens but shows the wrong API endpoint in `appsettings.json`, re-check `API_BASE_URL_DEV` or `API_BASE_URL_PROD`.

## Phase 11: Promote to `main`

### Goal

Deploy the production environment using the same repo and pipeline model.

### Where to do this

Ubuntu terminal and GitHub.

### Exact steps

1. Make sure `develop` is stable.
2. Merge `develop` into `main` using your team process.
3. Push `main`.
4. Watch the same workflows execute for production.

### Expected result

The backend deploys using `infra/env/prod.bicepparam` and the frontend deploys to the prod Static Web App.

### If blocked, check this

- If prod fails but dev worked, compare prod secrets against dev carefully.

## What the Workflows Are Doing

### Backend

[deploy-backend.yml](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/.github/workflows/deploy-backend.yml) builds, tests, publishes, deploys Bicep, and deploys the API package.

### Frontend

[deploy-frontend.yml](/c:/Users/Mohamed Farees/Downloads/ShipPulse_StarterRepo_v3/shippulse-starter/.github/workflows/deploy-frontend.yml) injects the API base URL into app settings, publishes the Blazor app, and deploys to Static Web Apps.

## Important Runtime Note About Key Vault

The current repo sets a sample Key Vault reference in the backend App Service configuration. It does not fully complete the secret creation and permission model automatically in the repo you have today.

That means you must verify these manually:

1. The secret exists in Key Vault.
2. The web app identity can access it.
3. The app behaves correctly if the secret is required at runtime.

Do not assume success just because the infrastructure deployment succeeded.

## Validation Checklist

- Repo created with the correct name
- `develop` and `main` exist
- Ubuntu VM created and accessible
- Git, .NET, and Azure CLI installed on Ubuntu
- Backend runs locally
- Frontend runs locally
- Tests pass
- Dev backend deployed
- Dev frontend deployed
- Prod backend deployed
- Prod frontend deployed
- Application Insights visible
- Log Analytics visible
- Key Vault checked

## Common Problems

### Restore, build, or test fails

- Re-run from repo root.
- Check network connectivity from Ubuntu.
- Read the first failing project.

### GitHub Actions Azure auth fails

- Re-check `AZURE_CREDENTIALS`.
- Confirm the service principal has access to the target resources.

### Bicep deployment fails

- Re-check the resource group name secret.
- Re-check the parameter file names and values.
- Re-check Azure region availability.

### Static Web App deployment issue

- Re-check `AZURE_STATIC_WEBAPP_NAME_DEV` or `AZURE_STATIC_WEBAPP_NAME_PROD`.
- Confirm the Static Web App exists in Azure.
- Confirm the service principal in `AZURE_CREDENTIALS` can read Static Web App secrets.

### Wrong API base URL

- Check `API_BASE_URL_DEV` and `API_BASE_URL_PROD`.
- Confirm the URL points to the deployed App Service, not localhost.

### Azure region unavailable

- Use `South India` and document it clearly.
- Keep `staticWebAppLocation` as `eastasia` unless you intentionally validate another supported Static Web Apps region.

## What Farees Will Expect in the Review

Be ready to explain:

1. Why the VM-first workflow was used
2. How `develop` differs from `main`
3. What each GitHub workflow does
4. Which Azure services are used and why
5. How frontend learns the backend URL
6. What is manual versus fully automated in the current repo
7. Where you saw logs, telemetry, and deployment evidence


