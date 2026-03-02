# Ubuntu VM Setup Guide

This guide is for Ram and Prasanth. Your Ubuntu VM is your ShipPulse startup dev box. Do all assignment work from this VM. Your Windows laptop is only used to connect to it.

## Before You Start

### Goal

Make sure you have everything required before creating the VM.

### Where to do this

Azure Portal, GitHub, and your Windows laptop.

### Exact steps

1. Confirm you can sign in to your own Azure subscription.
2. Confirm you can sign in to GitHub and access the `AzureDevOpsC07AA` organization.
3. Confirm you know which repo you must create:
   - Ram: `AzureDevOpsC07AA/shippulse-ram`
   - Prasanth: `AzureDevOpsC07AA/shippulse-prasanth`
4. On your Windows laptop, open PowerShell.
5. Check whether SSH is available:

```powershell
ssh -V
```

### Expected result

You can access Azure, GitHub, and SSH from Windows.

### If blocked, check this

- If `ssh` is not found, install OpenSSH Client in Windows optional features.
- If GitHub org access is missing, get org access before continuing.

## Step 1: Create a Resource Group

### Goal

Create a resource group that will hold your VM and related learning resources.

### Where to do this

Azure Portal.

### Exact steps

1. Sign in to `https://portal.azure.com`.
2. Search for `Resource groups`.
3. Select `Create`.
4. Choose your own Azure subscription.
5. Enter a resource group name.
   - Ram example: `rg-shippulse-ram-dev`
   - Prasanth example: `rg-shippulse-prasanth-dev`
6. Select region `Central India`.
7. If `Central India` is blocked by capacity, use `South India`.
8. This VM region guidance is for the Ubuntu development machine. In the ShipPulse deployment, Static Web Apps should still use `East Asia`.
9. Select `Review + create`.
10. Select `Create`.

### Expected result

Your resource group is created successfully.

### If blocked, check this

- Ensure the selected region is allowed in your subscription.
- If policy blocks naming, keep the same pattern and shorten only if required.

## Step 2: Create the Ubuntu VM

### Goal

Create the Linux machine where you will run the full assignment.

### Where to do this

Azure Portal.

### Exact steps

1. Search for `Virtual machines`.
2. Select `Create`, then `Azure virtual machine`.
3. Choose your own subscription.
4. Select the resource group you created in Step 1.
5. Enter a virtual machine name.
   - Ram example: `vm-shippulse-ram-dev`
   - Prasanth example: `vm-shippulse-prasanth-dev`
6. For region, choose the same region as your resource group.
7. For image, choose `Ubuntu Server 22.04 LTS`.
8. For size, start with `Standard_B1ms` unless your subscription blocks it.
9. For authentication type, choose `SSH public key`.
10. For username, enter `azureuser`.
11. For SSH key source:
    - Use an existing key if you already have one.
    - Otherwise generate a new key pair from the portal.
12. Under inbound port rules, allow only `SSH (22)`.
13. Move to disks and keep the default OS disk unless your subscription has a restriction.
14. Move to networking and keep the default public IP enabled.
15. Review the configuration carefully.
16. Select `Review + create`.
17. Select `Create`.
18. If Azure asks you to download a private key, store it safely on your Windows laptop.

### Expected result

Your Ubuntu VM is created and shows a running state.

### If blocked, check this

- If the selected size is unavailable, pick the smallest Ubuntu-supported size allowed by your subscription.
- If the image is not available, verify you chose Ubuntu 22.04 LTS and not a marketplace image with extra billing conditions.

## Step 3: Get the VM Public IP

### Goal

Find the address you need for SSH.

### Where to do this

Azure Portal.

### Exact steps

1. Open your virtual machine.
2. On the overview page, find `Public IP address`.
3. Copy it.

### Expected result

You have the public IP address for your VM.

### If blocked, check this

- If no public IP exists, verify the VM was created with a public IP.
- If the VM is stopped, start it and wait for the overview page to refresh.

## Step 4: Connect from Windows with SSH

### Goal

Open a terminal session into Ubuntu from your laptop.

### Where to do this

Windows PowerShell on your laptop.

### Exact steps

1. If you generated a new private key, save it in a known folder.
2. Open PowerShell.
3. Run one of these commands:

If you already have the key in the default SSH path:

```powershell
ssh azureuser@<PUBLIC_IP>
```

If you must specify the private key file:

```powershell
ssh -i C:\Path\To\Your\PrivateKey azureuser@<PUBLIC_IP>
```

4. When asked to trust the host, type `yes`.

### Expected result

You reach an Ubuntu shell prompt as `azureuser`.

### If blocked, check this

- If you get `Permission denied (publickey)`, verify you used the matching private key.
- If you get a timeout, confirm NSG allows inbound port 22.
- If you get connection refused, confirm the VM is running.

## Step 5: First Login Checks

### Goal

Confirm the VM is healthy before installing tools.

### Where to do this

Ubuntu terminal over SSH.

### Exact steps

Run:

```bash
whoami
pwd
uname -a
lsb_release -a
```

### Expected result

- Username shows `azureuser`
- Ubuntu version is displayed
- The shell is working normally

### If blocked, check this

- If `lsb_release` is missing, install `lsb-release` later. This is not a blocker.

## Step 6: Update Ubuntu

### Goal

Refresh package lists and install base tools.

### Where to do this

Ubuntu terminal over SSH.

### Exact steps

Run:

```bash
sudo apt update
sudo apt upgrade -y
sudo apt install -y git curl unzip ca-certificates apt-transport-https software-properties-common lsb-release
```

### Expected result

Ubuntu packages are updated and the base tools are installed.

### If blocked, check this

- If package update fails, confirm the VM has outbound internet access.
- If you see a dpkg lock, wait for background package tasks to finish and retry.

## Step 7: Install .NET SDK

### Goal

Install the .NET SDK required to build ShipPulse.

### Where to do this

Ubuntu terminal over SSH.

### Exact steps

1. Add the Microsoft package feed:

```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

2. Refresh package lists:

```bash
sudo apt update
```

3. Install the SDK:

```bash
sudo apt install -y dotnet-sdk-10.0
```

4. Verify the install:

```bash
dotnet --info
```

### Expected result

The .NET SDK is installed and `dotnet --info` returns successfully.

### If blocked, check this

- If the package is not found, confirm you added the Microsoft package feed first.
- If a different Ubuntu version is detected, use the matching Microsoft instructions for that version.

## Step 8: Install Azure CLI

### Goal

Install the Azure CLI for login, resource inspection, and deployment checks.

### Where to do this

Ubuntu terminal over SSH.

### Exact steps

Run:

```bash
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
az version
```

### Expected result

Azure CLI installs successfully and prints version information.

### If blocked, check this

- If the install script fails, run `sudo apt update` and retry.
- If `az` is not found, open a new shell session and run `az version` again.

## Step 9: Sign in to Azure from Ubuntu

### Goal

Make sure your VM can access your Azure subscription.

### Where to do this

Ubuntu terminal over SSH.

### Exact steps

Run:

```bash
az login
az account show
```

If you have access to multiple subscriptions, set the correct one:

```bash
az account set --subscription "<YOUR_SUBSCRIPTION_NAME_OR_ID>"
```

### Expected result

Azure CLI is signed in and points at your intended subscription.

### If blocked, check this

- If browser login is inconvenient, use the device code path:

```bash
az login --use-device-code
```

## Step 10: Configure Git

### Goal

Set your Git identity before cloning or pushing code.

### Where to do this

Ubuntu terminal over SSH.

### Exact steps

Run:

```bash
git config --global user.name "Your Name"
git config --global user.email "your-email@example.com"
git config --global init.defaultBranch main
```

Verify:

```bash
git config --global --list
```

### Expected result

Git is configured with your identity.

### If blocked, check this

- If you mistyped a value, rerun the command with the correct value.

## Step 11: Optional GitHub SSH Setup

### Goal

Set up GitHub authentication from Ubuntu without typing credentials repeatedly.

### Where to do this

Ubuntu terminal and GitHub web UI.

### Exact steps

1. Generate a key:

```bash
ssh-keygen -t ed25519 -C "your-email@example.com"
```

2. Accept the default file path.
3. Display the public key:

```bash
cat ~/.ssh/id_ed25519.pub
```

4. Copy the output.
5. In GitHub, open `Settings > SSH and GPG keys`.
6. Add a new SSH key and paste the copied public key.
7. Test it:

```bash
ssh -T git@github.com
```

### Expected result

GitHub accepts the key and SSH access works from the VM.

### If blocked, check this

- If GitHub says permission denied, verify you copied the `.pub` key and not the private key.

## Step 12: Definition of Done

You are ready for the ShipPulse assignment when all of these work from Ubuntu:

```bash
git --version
dotnet --info
az version
```

And you can also:

1. SSH into the VM from Windows.
2. Sign in to Azure from the VM.
3. Push to GitHub from the VM.

## Common Mistakes

- Creating the VM with password authentication instead of SSH key authentication
- Opening unnecessary inbound ports instead of only port 22
- Losing the downloaded private key after VM creation
- Installing the wrong .NET SDK version
- Forgetting to sign in to the correct Azure subscription
- Doing the assignment on the Windows laptop instead of inside Ubuntu

## Troubleshooting Quick Reference

### Cannot SSH

- Confirm the VM is running.
- Confirm the public IP is correct.
- Confirm NSG allows inbound SSH on port 22.
- Confirm you are using the matching private key.

### `dotnet` not found

- Re-run the Microsoft package feed steps.
- Re-run `sudo apt update`.
- Reinstall `dotnet-sdk-10.0`.

### `az` not found

- Re-run the Azure CLI install command.
- Open a fresh shell.
- Run `az version`.

### Permission denied

- If it is an SSH problem, check the private key path and file.
- If it is a package install problem, confirm the command starts with `sudo`.
