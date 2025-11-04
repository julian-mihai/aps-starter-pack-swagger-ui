#!/bin/bash

################################################################################
# APS Starter Pack - Simple Azure Deployment Script
################################################################################
#
# WHAT THIS SCRIPT DOES:
# - Builds your .NET 9 application
# - Deploys it to Azure App Service
# - Optionally updates APS credentials in Azure
#
# PREREQUISITES:
# 1. Azure CLI installed: https://docs.microsoft.com/cli/azure/install-azure-cli
# 2. Azure account: https://azure.microsoft.com/free/
# 3. .NET 9 SDK installed: https://dotnet.microsoft.com/download
#
# HOW TO USE:
# 1. Make executable: chmod +x Tools/deploy.sh
# 2. Run: ./Tools/deploy.sh
# 3. Follow the simple prompts
#
################################################################################

set -e  # Exit on error

# FIXED VALUES (pre-configured for your workshop)
RESOURCE_GROUP="rg-aps-starting_pack"
APP_NAME="aps-starter-pack"
APP_URL="https://aps-starter-pack.azurewebsites.net"
APS_CALLBACK_URL="https://aps-starter-pack.azurewebsites.net/auth/callback"

# Colors for output (makes it easier to read)
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Helper functions for colored output
print_header() {
    echo -e "\n${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}\n"
}

print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

print_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

################################################################################
# WELCOME
################################################################################

print_header "🚀 APS Starter Pack - Simple Deployment"

echo "This script will deploy your app to Azure."
echo ""
echo "📍 Deploying to:"
echo "   Resource Group: $RESOURCE_GROUP"
echo "   App Name: $APP_NAME"
echo "   URL: $APP_URL"
echo ""
echo "⏱️  Time: ~5 minutes"
echo ""

# Check prerequisites
print_info "Checking prerequisites..."
if ! command -v az &> /dev/null; then
    print_error "Azure CLI is not installed!"
    echo "Install from: https://docs.microsoft.com/cli/azure/install-azure-cli"
    exit 1
fi

if ! command -v dotnet &> /dev/null; then
    print_error ".NET SDK is not installed!"
    echo "Install from: https://dotnet.microsoft.com/download/dotnet/9.0"
    exit 1
fi

print_success "Prerequisites OK!"

################################################################################
# STEP 1: Azure Login (Optional)
################################################################################

print_header "STEP 1: Azure Login"

read -p "Do you need to login to Azure? (y/n): " DO_LOGIN

if [[ "$DO_LOGIN" == "y" || "$DO_LOGIN" == "yes" ]]; then
    echo ""
    echo "Opening device code login..."
    echo "You'll get a code to enter in your browser."
    echo ""
    read -p "Press ENTER to start login..."
    echo ""
    
    az login --use-device-code
    
    if [ $? -ne 0 ]; then
        print_error "Azure login failed!"
        exit 1
    fi
    
    print_success "Logged in to Azure!"
else
    print_info "Skipping Azure login (using existing session)"
fi

################################################################################
# STEP 2: Build and Deploy
################################################################################

print_header "STEP 2: Build and Deploy"

# Go to project root
cd "$(dirname "$0")/.."

# Clean previous builds
print_info "Cleaning old builds..."
rm -rf ./publish ./app.zip 2>/dev/null

# Build
print_info "Building application..."
dotnet publish aps-starter-pack-swagger-ui.csproj -c Release -o ./publish --nologo -v q

if [ $? -ne 0 ]; then
    print_error "Build failed!"
    exit 1
fi

print_success "Build complete!"

# Create ZIP
print_info "Creating deployment package..."
cd publish && zip -r ../app.zip * > /dev/null 2>&1 && cd ..

print_success "Package created!"

# Deploy (using new Azure recommended command)
print_info "Uploading to Azure (this takes 2-3 minutes)..."
az webapp deploy \
    --name "$APP_NAME" \
    --resource-group "$RESOURCE_GROUP" \
    --src-path app.zip \
    --type zip \
    --async true

if [ $? -ne 0 ]; then
    print_error "Deployment failed!"
    exit 1
fi

print_success "Deployed to Azure!"

# Cleanup
rm -rf ./publish ./app.zip
print_info "Cleaned up temporary files"

################################################################################
# STEP 3: Optional - Configure Email Whitelist
################################################################################

print_header "STEP 3: Email Whitelist (Optional)"

echo ""
echo "📧 Configure email whitelist to restrict who can access your app."
echo ""
read -p "Do you want to enable email whitelist? (y/n): " ENABLE_WHITELIST

if [[ "$ENABLE_WHITELIST" == "y" || "$ENABLE_WHITELIST" == "yes" ]]; then
    echo ""
    echo "✨ NEW SIMPLE FORMAT: Just enter a comma-separated list of emails!"
    echo ""
    echo "Examples:"
    echo "  - iulian@autodesk.com, admin1@autodesk.com, admin2@company.com"
    echo "  - *@autodesk.com, partner@company.com"
    echo "  - john.doe@gmail.com"
    echo ""
    read -p "Enter emails (comma-separated): " EMAIL_LIST
    
    if [ -n "$EMAIL_LIST" ]; then
        echo ""
        print_info "Configuring whitelist on Azure..."
        
        az webapp config appsettings set \
            --name "$APP_NAME" \
            --resource-group "$RESOURCE_GROUP" \
            --settings AuthorizedUsers__EnableWhitelist="true" \
                       AuthorizedUsers__AllowedEmails="$EMAIL_LIST" \
            --output none
        
        print_success "Email whitelist configured!"
        echo ""
        echo "✅ Authorized emails: $EMAIL_LIST"
        echo ""
    else
        print_warning "No emails provided - whitelist not configured"
    fi
else
    print_info "Skipping email whitelist configuration"
    echo ""
    print_info "To enable later, run:"
    echo "   az webapp config appsettings set \\"
    echo "     --name $APP_NAME \\"
    echo "     --resource-group $RESOURCE_GROUP \\"
    echo "     --settings AuthorizedUsers__EnableWhitelist=\"true\" \\"
    echo "                AuthorizedUsers__AllowedEmails=\"your@email.com, admin@email.com\""
fi

echo ""

################################################################################
# STEP 4: Done!
################################################################################

print_header "✅ DEPLOYMENT COMPLETE!"

echo ""
echo "🌐 Your app is live at:"
echo "   $APP_URL"
echo ""
echo "📖 Next steps:"
echo "   1. Open: $APP_URL"
echo "   2. Login with Autodesk SSO"
echo "   3. Start testing APIs!"
echo ""
print_warning "REMINDER: Callback URL must be in APS portal"
echo "          $APS_CALLBACK_URL"
echo "          Visit: https://aps.autodesk.com/ → Your App → Callback URL"
echo ""
print_info "View logs: az webapp log tail --name $APP_NAME --resource-group $RESOURCE_GROUP"
echo ""
print_success "Happy coding! 🚀"
echo ""

# Open browser
if command -v open &> /dev/null; then
    open "$APP_URL"
elif command -v xdg-open &> /dev/null; then
    xdg-open "$APP_URL"
fi

