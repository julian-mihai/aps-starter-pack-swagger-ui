# Tools Directory 🛠️

Deployment and utility scripts for the APS Starter Pack Workshop.

---

## 📋 Available Scripts

1. **`deploy.sh`** - Deploy to Azure
2. **`deep-clean.sh`** - Clean build artifacts (GitHub-ready)

---

## 🚀 `deploy.sh` - Simple Azure Deployment

**Super simple deployment - perfect for non-IT users!**

### Pre-configured Settings:
- **Resource Group:** `rg-aps-starting_pack`
- **App Name:** `aps-starter-pack`
- **URL:** `https://aps-starter-pack.azurewebsites.net`

**No complicated configuration needed!**

---

## 📋 Prerequisites

Install these three things:

1. **Azure CLI** - [Download here](https://docs.microsoft.com/cli/azure/install-azure-cli)
   ```bash
   # Verify it's installed
   az --version
   ```

2. **Azure Account** - [Sign up free](https://azure.microsoft.com/free/)

3. **.NET 9 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
   ```bash
   # Verify it's installed
   dotnet --version
   ```

---

## 🎯 How to Use

### Just 2 commands:

#### 1. Make executable (first time only):
```bash
chmod +x Tools/deploy.sh
```

#### 2. Run the script:
```bash
./Tools/deploy.sh
```

### The script will:

1. **Check if you're already logged in to Azure** ✨
   - ✅ If logged in: Shows your account and asks "Continue? (y/n)"
   - ❌ If not logged in: Prompts for device code login
   - 🔄 Type "n" to switch accounts (logs out and re-authenticates)

2. **Build and deploy automatically** (takes 3-5 minutes)

3. **Configure email whitelist (Optional)** 📧
   - Asks if you want to enable whitelist
   - **SUPER SIMPLE:** Enter comma-separated email list in ONE line
   - Example: `iulian@autodesk.com, admin1@autodesk.com, admin2@company.com`
   - Configures Azure App Settings automatically
   - Supports wildcard domains (`*@autodesk.com`)

**Smart Features:** 
- Skips device code login if already authenticated
- Optional email whitelist for production security
- Automatically opens your deployed app in browser

**Note:** APS credentials come from `appsettings.json` automatically.

---

## 📖 Example Session

```bash
$ ./Tools/deploy.sh

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🚀 APS Starter Pack - Simple Deployment
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

This script will deploy your app to Azure.

📍 Deploying to:
   Resource Group: rg-aps-starting_pack
   App Name: aps-starter-pack
   URL: https://aps-starter-pack.azurewebsites.net

⏱️  Time: ~5 minutes

ℹ️  Checking prerequisites...
✅ Prerequisites OK!

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
STEP 1: Azure Login Check
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

ℹ️  Checking Azure login status...
✅ Already logged in to Azure!

   Account: your.email@company.com
   Subscription: Visual Studio Enterprise

Continue with this account? (y/n): y
✅ Using existing Azure session!

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
STEP 2: Build and Deploy
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

ℹ️  Cleaning old builds...
ℹ️  Building application...
✅ Build complete!
ℹ️  Creating deployment package...
✅ Package created!
ℹ️  Uploading to Azure (this takes 2-3 minutes)...
✅ Deployed to Azure!
ℹ️  Cleaned up temporary files

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ DEPLOYMENT COMPLETE!
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🌐 Your app is live at:
   https://aps-starter-pack.azurewebsites.net

📖 Next steps:
   1. Open: https://aps-starter-pack.azurewebsites.net
   2. Login with Autodesk SSO
   3. Start testing APIs!

⚠️  REMINDER: Callback URL must be in APS portal
          https://aps-starter-pack.azurewebsites.net/auth/callback
          Visit: https://aps.autodesk.com/ → Your App → Callback URL

ℹ️  View logs: az webapp log tail --name aps-starter-pack --resource-group rg-aps-starting_pack

✅ Happy coding! 🚀
```

---

## 🎯 What Happens (Simple Overview)

| Step | What It Does | Time |
|------|-------------|------|
| **Prerequisites** | Checks Azure CLI and .NET SDK | 5s |
| **Login Check** | Checks if already logged in | 5s |
| **Login (if needed)** | Device code login (only if not logged in) | 0-30s |
| **Build** | Compiles your .NET 9 app | 30s |
| **Deploy** | Uploads to Azure | 2-3min |
| **Whitelist (Optional)** | Configure authorized emails | 0-30s |
| **Done!** | Opens app in browser | - |

**Total: ~3-5 minutes** (faster if already logged in!)

### Two Scenarios:

**Scenario A: Already Logged In** ✨
```
✅ Already logged in!
Continue? (y/n): y
→ Skips device code, goes straight to build
Total: ~3 minutes
```

**Scenario B: Not Logged In**
```
⚠️  Not logged in
Press ENTER to login...
→ Device code authentication
→ Then builds and deploys
Total: ~4 minutes
```

---

## ⚠️ Important: Callback URL

After first deployment, you **MUST** add the callback URL to your APS app:

1. Go to https://aps.autodesk.com/
2. Select your app
3. Go to **General Settings** → **Callback URL**
4. Add: `https://aps-starter-pack.azurewebsites.net/auth/callback`
5. Click **Save**

**Without this, SSO login won't work!**

---

## 🔄 Re-deploying Updates

To deploy code changes, just run the script again:

```bash
./Tools/deploy.sh
```

The script will:
- Login to Azure (device code)
- Build your updated code
- Deploy to Azure

**Same simple process every time!** (~3-5 minutes)

---

## 📊 View Logs

```bash
# Real-time logs
az webapp log tail --name aps-starter-pack --resource-group rg-aps-starting_pack

# Or in Azure Portal:
# App Service → Log stream
```

---

## 🐛 Troubleshooting

### "App name already exists"
- The app is already deployed. Just run the script again to update it.

### "403 Forbidden" from APS API
- Check callback URL is added to APS portal
- Verify credentials in Azure App Settings

### "502 Bad Gateway"
- Wait 1-2 minutes for app to fully start
- Check logs: `az webapp log tail ...`

### "Build failed"
- Make sure you're in the project root directory
- Run `dotnet build` to see detailed errors

---

## 💰 Cost Management

**Monthly Cost:** ~$13 (Azure B1 Basic tier)

### Stop the app (no charges while stopped):
```bash
az webapp stop --name aps-starter-pack --resource-group rg-aps-starting_pack
```

### Start it again:
```bash
az webapp start --name aps-starter-pack --resource-group rg-aps-starting_pack
```

### Delete everything:
```bash
az group delete --name rg-aps-starting_pack --yes
```

---

## 💡 Pro Tips

### Quickly Switch APS Apps

To use a different APS app, edit `appsettings.json`:

1. Update `ClientId` and `ClientSecret`
2. Save the file
3. Run `./Tools/deploy.sh`

Done! New credentials deploy automatically.

### Check Azure Resources

```bash
# List all resources in the group
az resource list --resource-group rg-aps-starting_pack --output table
```

### View App Settings

```bash
# See all app settings (credentials hidden)
az webapp config appsettings list \
  --name aps-starter-pack \
  --resource-group rg-aps-starting_pack
```

---

## 🎓 For Workshop Facilitators

### Teaching Tips:

1. **Run together** - Have everyone run the script at the same time
2. **Show Azure Portal** - Open portal to visualize resources
3. **Explain "yes" vs "no"** - Help participants understand when to update credentials
4. **Test together** - Everyone open the app URL together
5. **Show logs** - Demonstrate `az webapp log tail`

### Common Student Questions:

**Q: Why is it called `rg-aps-starting_pack`?**  
A: "rg" stands for Resource Group - it's an Azure naming convention.

**Q: Can I use my own app name?**  
A: Yes, but you'll need to edit the script (top of the file).

**Q: What if I make a code change?**  
A: Just run the script again - it deploys automatically.

**Q: How do I delete everything?**  
A: `az group delete --name rg-aps-starting_pack --yes`

---

## 🧹 `deep-clean.sh` - Clean Build Artifacts

**Make your project GitHub-ready by removing all temporary files!**

### What It Does:
- ✅ Removes build artifacts (`bin/`, `obj/`, `publish/`)
- ✅ Removes deployment packages (`*.zip`)
- ✅ Removes IDE cache (`.vs/`, `.idea/`, `*.user`)
- ✅ Removes temporary files (`.DS_Store`, `*.log`, `*.tmp`)
- ✅ Optionally cleans NuGet package cache
- ✅ Verifies project integrity after cleanup

### What It Keeps (Never Touches):
- ✅ All source code (`.cs` files)
- ✅ Project files (`.csproj`)
- ✅ Configuration (`appsettings.json`)
- ✅ Documentation (`README.md`, `LICENSE`)
- ✅ Static files (`wwwroot/`)
- ✅ Tools and scripts

### How to Use:

#### 1. Make executable (first time only):
```bash
chmod +x Tools/deep-clean.sh
```

#### 2. Run the cleanup:
```bash
./Tools/deep-clean.sh
```

### Interactive Prompts:

The script will ask you:

1. **Continue with cleanup? (y/n)**
   - Confirms you want to proceed
   - Type `y` or `yes` to continue
   - Type `n` or `no` to cancel

2. **Clean NuGet cache? (y/n)**
   - Optional: Forces re-download of packages
   - Type `y` or `yes` for complete clean
   - Type `n` or `no` to keep packages (faster next build)

### Example Session:

```bash
$ ./Tools/deep-clean.sh

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
🧹 Deep Clean - Make GitHub Ready
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

This script will remove all build artifacts and temporary files.
Your source code and configuration will NOT be touched.

Continue with cleanup? (y/n): y

ℹ️  Cleaning from: /path/to/project
✅ Project directory confirmed ✓

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Cleaning Build Artifacts
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

ℹ️  Removing bin/ folder...
✅ Removed bin/
ℹ️  Removing obj/ folder...
✅ Removed obj/

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Cleaning IDE Cache Files
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

ℹ️  Removing .vs/ folder (Visual Studio cache)...
✅ Removed .vs/
ℹ️  Removing *.user files...
✅ Removed 2 *.user file(s)

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ CLEANUP COMPLETE!
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📊 What was cleaned:
   • Build artifacts (bin/, obj/, publish/)
   • Deployment packages (*.zip)
   • IDE cache files (.vs/, .idea/, *.user)
   • Temporary files (.DS_Store, *.log, *.tmp)

✅ What was preserved:
   • Source code (.cs files)
   • Project files (.csproj)
   • Configuration (appsettings.json)
   • Documentation (README.md, LICENSE)
   • Static files (wwwroot/)
   • Tools and scripts

🎯 Your project is now GitHub-ready!

ℹ️  Next steps:
   1. Review changes: git status
   2. Test build: dotnet build
   3. Commit to Git: git add . && git commit -m 'Clean project'

✅ Happy coding! 🚀
```

### When to Use:

**Before committing to Git:**
```bash
./Tools/deep-clean.sh  # Clean everything
git status             # Review what's left
git add .              # Add clean files
git commit -m "Initial commit"
```

**Before sharing the project:**
```bash
./Tools/deep-clean.sh  # Remove temporary files
zip -r project.zip .   # Create clean archive
```

**After pulling updates:**
```bash
git pull               # Get latest changes
./Tools/deep-clean.sh  # Clean old build artifacts
dotnet build           # Fresh build
```

### Files Removed:

| Category | Files/Folders | Why Remove |
|----------|--------------|------------|
| **Build Output** | `bin/`, `obj/`, `publish/` | Regenerated on each build |
| **Deployment** | `*.zip` | Temporary deployment packages |
| **IDE Cache** | `.vs/`, `.idea/`, `*.user`, `*.suo` | User-specific, machine-specific |
| **macOS** | `.DS_Store` | macOS folder metadata |
| **Windows** | `Thumbs.db` | Windows thumbnail cache |
| **Logs** | `*.log`, `*.tmp` | Temporary log files |

### Safe and Verified:

The script includes an **integrity check** after cleanup:

```bash
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Verifying Project Integrity
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

  ✅ Program.cs
  ✅ appsettings.json
  ✅ aps-starter-pack-swagger-ui.csproj
  ✅ README.md
  ✅ LICENSE
  ✅ Tools/deploy.sh
  ✅ Tools/deep-clean.sh

✅ All essential files present!
```

### Troubleshooting:

**Script won't run:**
```bash
# Make it executable
chmod +x Tools/deep-clean.sh
```

**Want to keep .vscode folder:**
- The script already keeps `.vscode/` (commented out in code)
- Only removes Visual Studio (`.vs/`) and Rider (`.idea/`) caches

**Accidentally deleted something:**
```bash
# If source code was never touched:
git checkout .

# Or restore from your last commit:
git reset --hard HEAD
```

---

## 📚 Additional Resources

- **Azure CLI Reference:** https://docs.microsoft.com/cli/azure/
- **Azure App Service:** https://docs.microsoft.com/azure/app-service/
- **APS Documentation:** https://aps.autodesk.com/en/docs/
- **Main README:** See `../README.md` for complete documentation

---

**Happy Deploying! 🚀**
