#!/bin/bash

################################################################################
# APS Starter Pack - Deep Clean Script
################################################################################
#
# WHAT THIS SCRIPT DOES:
# - Removes build artifacts (bin/, obj/, publish/)
# - Removes deployment packages (*.zip)
# - Removes IDE cache files
# - Removes temporary files
# - Makes the project GitHub-ready
#
# WHAT IT KEEPS:
# - All source code (.cs files)
# - Project files (.csproj)
# - Configuration files (appsettings.json)
# - Documentation (README.md, LICENSE)
# - Static files (wwwroot/)
# - Tools and scripts
#
# SAFE TO RUN:
# - This script only removes generated/temporary files
# - Your source code is never touched
#
# HOW TO USE:
# 1. chmod +x Tools/deep-clean.sh
# 2. ./Tools/deep-clean.sh
#
################################################################################

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Helper functions
print_header() {
    echo -e "\n${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}\n"
}

print_success() {
    echo -e "${GREEN}✅ $1${NC}"
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

print_header "🧹 Deep Clean - Make GitHub Ready"

echo "This script will remove all build artifacts and temporary files."
echo "Your source code and configuration will NOT be touched."
echo ""
read -p "Continue with cleanup? (y/n): " CONFIRM

# Accept both "y" and "yes"
if [[ ! "$CONFIRM" =~ ^[Yy](es)?$ ]]; then
    print_warning "Cleanup cancelled"
    exit 0
fi

# Go to project root (script is in /Tools folder)
cd "$(dirname "$0")/.."

print_info "Cleaning from: $(pwd)"

# Safety check: Make sure we're in the right directory
if [ ! -f "aps-starter-pack-swagger-ui.csproj" ]; then
    print_warning "ERROR: aps-starter-pack-swagger-ui.csproj not found!"
    print_warning "This script must be run from the project root or Tools/ folder."
    exit 1
fi

print_success "Project directory confirmed ✓"
echo ""

################################################################################
# CLEAN BUILD ARTIFACTS
################################################################################

print_header "Cleaning Build Artifacts"

# Remove bin folders (compiled output)
if [ -d "bin" ]; then
    print_info "Removing bin/ folder..."
    rm -rf bin/
    print_success "Removed bin/"
else
    print_info "bin/ not found (already clean)"
fi

# Remove obj folders (build intermediates)
if [ -d "obj" ]; then
    print_info "Removing obj/ folder..."
    rm -rf obj/
    print_success "Removed obj/"
else
    print_info "obj/ not found (already clean)"
fi

# Remove publish folders (deployment output)
if [ -d "publish" ]; then
    print_info "Removing publish/ folder..."
    rm -rf publish/
    print_success "Removed publish/"
else
    print_info "publish/ not found (already clean)"
fi

################################################################################
# CLEAN DEPLOYMENT PACKAGES
################################################################################

print_header "Cleaning Deployment Packages"

# Remove ZIP files (deployment packages)
ZIP_COUNT=$(find . -maxdepth 1 -name "*.zip" 2>/dev/null | wc -l | tr -d ' ')
if [ "$ZIP_COUNT" -gt 0 ]; then
    print_info "Removing *.zip files..."
    rm -f *.zip
    print_success "Removed $ZIP_COUNT ZIP file(s)"
else
    print_info "No ZIP files found"
fi

################################################################################
# CLEAN IDE CACHE FILES
################################################################################

print_header "Cleaning IDE Cache Files"

# Remove .vs folder (Visual Studio cache)
if [ -d ".vs" ]; then
    print_info "Removing .vs/ folder (Visual Studio cache)..."
    rm -rf .vs/
    print_success "Removed .vs/"
else
    print_info ".vs/ not found"
fi

# Remove .vscode folder (VS Code settings) - OPTIONAL
# Commented out because some people like to keep VS Code settings
# if [ -d ".vscode" ]; then
#     print_info "Removing .vscode/ folder..."
#     rm -rf .vscode/
#     print_success "Removed .vscode/"
# fi

# Remove .idea folder (JetBrains Rider cache)
if [ -d ".idea" ]; then
    print_info "Removing .idea/ folder (Rider cache)..."
    rm -rf .idea/
    print_success "Removed .idea/"
else
    print_info ".idea/ not found"
fi

# Remove *.user files (user-specific settings)
USER_COUNT=$(find . -name "*.user" 2>/dev/null | wc -l | tr -d ' ')
if [ "$USER_COUNT" -gt 0 ]; then
    print_info "Removing *.user files..."
    find . -name "*.user" -delete
    print_success "Removed $USER_COUNT *.user file(s)"
else
    print_info "No *.user files found"
fi

# Remove *.suo files (Visual Studio user options)
SUO_COUNT=$(find . -name "*.suo" 2>/dev/null | wc -l | tr -d ' ')
if [ "$SUO_COUNT" -gt 0 ]; then
    print_info "Removing *.suo files..."
    find . -name "*.suo" -delete
    print_success "Removed $SUO_COUNT *.suo file(s)"
else
    print_info "No *.suo files found"
fi

################################################################################
# CLEAN TEMPORARY FILES
################################################################################

print_header "Cleaning Temporary Files"

# Remove .DS_Store (macOS)
DS_COUNT=$(find . -name ".DS_Store" 2>/dev/null | wc -l | tr -d ' ')
if [ "$DS_COUNT" -gt 0 ]; then
    print_info "Removing .DS_Store files (macOS)..."
    find . -name ".DS_Store" -delete
    print_success "Removed $DS_COUNT .DS_Store file(s)"
else
    print_info "No .DS_Store files found"
fi

# Remove Thumbs.db (Windows)
THUMBS_COUNT=$(find . -name "Thumbs.db" 2>/dev/null | wc -l | tr -d ' ')
if [ "$THUMBS_COUNT" -gt 0 ]; then
    print_info "Removing Thumbs.db files (Windows)..."
    find . -name "Thumbs.db" -delete
    print_success "Removed $THUMBS_COUNT Thumbs.db file(s)"
else
    print_info "No Thumbs.db files found"
fi

# Remove *.log files
LOG_COUNT=$(find . -name "*.log" 2>/dev/null | wc -l | tr -d ' ')
if [ "$LOG_COUNT" -gt 0 ]; then
    print_info "Removing *.log files..."
    find . -name "*.log" -delete
    print_success "Removed $LOG_COUNT *.log file(s)"
else
    print_info "No *.log files found"
fi

# Remove *.tmp files
TMP_COUNT=$(find . -name "*.tmp" 2>/dev/null | wc -l | tr -d ' ')
if [ "$TMP_COUNT" -gt 0 ]; then
    print_info "Removing *.tmp files..."
    find . -name "*.tmp" -delete
    print_success "Removed $TMP_COUNT *.tmp file(s)"
else
    print_info "No *.tmp files found"
fi

################################################################################
# CLEAN PACKAGE CACHE (OPTIONAL)
################################################################################

print_header "Package Cache (Optional)"

echo "Do you want to clean NuGet package cache?"
echo "This will force re-download on next build (slower first build)."
echo ""
read -p "Clean NuGet cache? (y/n): " CLEAN_NUGET

# Accept both "y" and "yes"
if [[ "$CLEAN_NUGET" =~ ^[Yy](es)?$ ]]; then
    print_info "Cleaning NuGet cache..."
    dotnet nuget locals all --clear > /dev/null 2>&1
    print_success "NuGet cache cleared"
else
    print_info "Skipping NuGet cache cleanup"
fi

################################################################################
# VERIFY PROJECT INTEGRITY
################################################################################

print_header "Verifying Project Integrity"

print_info "Checking that essential files exist..."

ESSENTIAL_FILES=(
    "Program.cs"
    "appsettings.json"
    "aps-starter-pack-swagger-ui.csproj"
    "README.md"
    "LICENSE"
    "Tools/deploy.sh"
    "Tools/deep-clean.sh"
)

ALL_GOOD=true
for file in "${ESSENTIAL_FILES[@]}"; do
    if [ -f "$file" ]; then
        echo "  ✅ $file"
    else
        echo "  ❌ $file (MISSING!)"
        ALL_GOOD=false
    fi
done

if [ "$ALL_GOOD" = true ]; then
    print_success "All essential files present!"
else
    print_warning "Some essential files are missing!"
fi

################################################################################
# SUMMARY
################################################################################

print_header "✅ CLEANUP COMPLETE!"

echo ""
echo "📊 Summary of cleaned items:"
echo ""
echo "   BUILD ARTIFACTS:"
echo "   • bin/, obj/, publish/ folders"
echo ""
echo "   DEPLOYMENT PACKAGES:"
[ "$ZIP_COUNT" -gt 0 ] && echo "   • $ZIP_COUNT ZIP file(s) removed" || echo "   • No ZIP files found"
echo ""
echo "   IDE CACHE FILES:"
echo "   • .vs/, .idea/ folders"
[ "$USER_COUNT" -gt 0 ] && echo "   • $USER_COUNT *.user file(s) removed" || echo "   • No *.user files found"
[ "$SUO_COUNT" -gt 0 ] && echo "   • $SUO_COUNT *.suo file(s) removed" || echo "   • No *.suo files found"
echo ""
echo "   TEMPORARY FILES:"
[ "$DS_COUNT" -gt 0 ] && echo "   • $DS_COUNT .DS_Store file(s) removed" || echo "   • No .DS_Store files found"
[ "$THUMBS_COUNT" -gt 0 ] && echo "   • $THUMBS_COUNT Thumbs.db file(s) removed" || echo "   • No Thumbs.db files found"
[ "$LOG_COUNT" -gt 0 ] && echo "   • $LOG_COUNT *.log file(s) removed" || echo "   • No *.log files found"
[ "$TMP_COUNT" -gt 0 ] && echo "   • $TMP_COUNT *.tmp file(s) removed" || echo "   • No *.tmp files found"
echo ""
[[ "$CLEAN_NUGET" =~ ^[Yy](es)?$ ]] && echo "   PACKAGE CACHE:" && echo "   • NuGet cache cleared" && echo ""
echo "✅ What was preserved:"
echo "   • All source code (.cs files)"
echo "   • All project files (.csproj)"
echo "   • Configuration (appsettings.json)"
echo "   • Documentation (README.md, LICENSE)"
echo "   • Static files (wwwroot/)"
echo "   • All Tools and scripts"
echo ""
echo "🎯 Your project is now clean and GitHub-ready!"
echo ""
print_info "Next steps:"
echo "   1. Test build: dotnet build"
echo "   2. Review changes: git status"
echo "   3. Commit to Git: git add . && git commit -m 'Clean build artifacts'"
echo ""
print_success "Happy coding! 🚀"
echo ""

