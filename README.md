# APS Data Management Workshop API 🚀

A simple, educational .NET 9 Web API to demonstrate **Autodesk Platform Services (APS) Data Management and ACC APIs**. 
Perfect for learning how to interact with BIM 360 and Autodesk Construction Cloud!

## 📖 What is This?

This is a workshop application designed to help users learn how to code and interact with Autodesk APIs. It features:

- ✅ **Interactive Swagger UI** - Test APIs directly in your browser
- ✅ **Autodesk SSO Protection** - Secure login with your Autodesk account
- ✅ **User Profile Display** - See who's logged in with name and email
- ✅ **Built-in Help Page** - Step-by-step guide for non-technical users
- ✅ **Clean, readable code** - Each file has a single, clear purpose
- ✅ **Well-documented** - Comments and explanations everywhere
- ✅ **Easy configuration** - Just add your ClientId and ClientSecret
- ✅ **Production-ready patterns** - Learn proper architecture (Controllers, Services, Models, Middleware)

---

## 🛠️ Technologies & Frameworks

This workshop application is built with modern, production-ready technologies. Here's what powers this application:

### **Backend Technologies**

#### **.NET 9** 🟣
**What it is:** Microsoft's latest cross-platform framework for building modern applications.

**Why we use it:**
- ✅ **Cross-platform** - Runs on Windows, Mac, and Linux
- ✅ **High performance** - One of the fastest web frameworks available
- ✅ **Modern C#** - Latest language features and improvements
- ✅ **Built-in features** - Dependency injection, configuration, logging out of the box
- ✅ **Azure-optimized** - Seamless deployment to Azure App Service

**Our version:** .NET 9 SDK (latest stable release)

---

#### **ASP.NET Core Web API** 🌐
**What it is:** Framework for building RESTful HTTP services.

**Why we use it:**
- ✅ **RESTful APIs** - Industry-standard HTTP endpoints
- ✅ **Built-in middleware** - Authentication, CORS, session management
- ✅ **Model binding** - Automatic parameter parsing from URLs and JSON
- ✅ **Built-in validation** - Data annotations and custom validators
- ✅ **Production-ready** - Used by millions of applications worldwide

**What it handles:**
- HTTP routing (`/hubs`, `/projects`, `/auth/callback`)
- Request/Response serialization (JSON)
- Middleware pipeline (authentication, security headers)
- Dependency Injection container

---

### **API Documentation**

#### **Swashbuckle.AspNetCore 9.0.6** 📚
**What it is:** The tool that generates the interactive Swagger UI you see at `http://localhost:8080`.

**Why it's THE MOST IMPORTANT package:**
- ✅ **Auto-generates API documentation** - No manual documentation needed
- ✅ **Interactive testing** - "Try it out" buttons for every endpoint
- ✅ **OpenAPI 3.0 compliant** - Industry-standard API specification
- ✅ **Custom branding** - Autodesk-themed UI with user profile display
- ✅ **XML comments** - Documentation from code comments

**What it does:**
1. Scans all controllers and endpoints
2. Generates OpenAPI specification (`/swagger/v1/swagger.json`)
3. Serves interactive Swagger UI with "Try it out" functionality
4. Displays parameter descriptions, required fields, and examples

**Without Swashbuckle, this workshop wouldn't exist!** ⚠️

---

### **HTTP & JSON Libraries**

#### **System.Net.Http.HttpClient** 🔗
**What it is:** .NET's built-in HTTP client for making web requests.

**What we use it for:**
- Making HTTP calls to APS APIs (`developer.api.autodesk.com`)
- Setting authorization headers (Bearer tokens)
- Handling responses and errors

**Key features:**
- Connection pooling (reuses connections for performance)
- Automatic decompression (gzip)
- Timeout configuration
- Typed clients via Dependency Injection

---

#### **Newtonsoft.Json 13.0.4** 📦
**What it is:** High-performance JSON framework (also known as JSON.NET).

**Why we use it:**
- ✅ **JSON serialization/deserialization** - Convert C# objects to/from JSON
- ✅ **Flexible parsing** - Handle complex APS API responses
- ✅ **Industry standard** - Most widely used .NET JSON library
- ✅ **Advanced features** - Custom converters, LINQ queries on JSON

**What it does:**
- Parses APS API JSON responses
- Serializes request bodies for POST/PUT operations
- Dependency for Swashbuckle 9.0.6

---

#### **RestSharp 112.1.0** 🚀
**What it is:** Simple REST and HTTP API client library.

**Why we use it:**
- ✅ **Simplifies HTTP requests** - Less boilerplate code
- ✅ **Automatic serialization** - JSON handling built-in
- ✅ **Request/Response models** - Strongly-typed API calls
- ✅ **Dependency resolution** - Brings in required transitive packages

**What it does:**
- Provides dependency for Swashbuckle 9.0.6 (`Microsoft.Extensions.ApiDescription.Server 9.0.0`)
- Simplifies REST API calls with fluent syntax

---

### **Authentication & Security**

#### **OAuth 2.0** 🔐
**What it is:** Industry-standard protocol for authorization.

**What we implement:**
- ✅ **2-Legged (Client Credentials)** - App-only access token
- ✅ **3-Legged (Authorization Code Flow)** - User authentication with Autodesk SSO
- ✅ **Refresh tokens** - Automatic token renewal
- ✅ **Scopes** - Permission-based access (`data:read`, `data:write`, `account:read`)

**Flow:**
1. User clicks "Login with Autodesk"
2. Redirect to Autodesk login page
3. User authenticates
4. Autodesk returns authorization code
5. Exchange code for access token
6. Store token in session
7. Use token for all API calls

---

#### **Session Management** 🗃️
**What it is:** Server-side storage for user data.

**Why we use it:**
- ✅ **Secure token storage** - Tokens never exposed to client JavaScript
- ✅ **User profile caching** - Name, email, picture stored in session
- ✅ **HttpOnly cookies** - Protection against XSS attacks
- ✅ **Automatic expiration** - 60-minute timeout

**What we store:**
- APS access token
- APS refresh token
- User name
- User email
- User profile picture URL

---

### **Custom Middleware**

#### **APSAuthenticationMiddleware** 🛡️
**What it is:** Custom request interceptor that enforces authentication.

**What it does:**
- Intercepts ALL incoming requests
- Checks for valid session token
- Redirects unauthenticated users to `/login.html`
- Allows public routes (`/auth/*`, `/swagger/*`, `/login.html`)
- Protects Swagger UI and help pages

---

#### **SecurityHeadersMiddleware** 🔒
**What it is:** Custom middleware that adds HTTP security headers to all responses.

**What it implements:**
1. **HSTS** - Force HTTPS connections (production only)
2. **CSP** - Content Security Policy (prevent XSS attacks)
3. **X-Frame-Options** - Prevent clickjacking
4. **X-Content-Type-Options** - Prevent MIME sniffing
5. **Referrer-Policy** - Control referrer information
6. **Permissions-Policy** - Disable unnecessary browser features

**Why it matters:**
- ✅ **Production security** - Essential for Azure deployment
- ✅ **Compliance** - Meets modern web security standards
- ✅ **User protection** - Defends against common web attacks

---

### **Frontend Technologies**

#### **HTML5 + CSS3 + JavaScript** 🎨
**What it is:** Standard web technologies for user interfaces.

**What we built:**
- `login.html` - Autodesk SSO login page
- `help.html` - User manual with step-by-step guides
- `about.html` - Developer guide with architecture documentation
- Custom Swagger UI branding (injected via `c.HeadContent`)

**Features:**
- Responsive design (mobile-friendly)
- Autodesk brand colors (Yellow #FFD100, Black #000000)
- Collapsible sections
- Code syntax highlighting
- User profile display in Swagger UI banner

---

### **Deployment & DevOps**

#### **Azure App Service** ☁️
**What it is:** Microsoft's platform-as-a-service (PaaS) for hosting web applications.

**Why we use it:**
- ✅ **Easy deployment** - Single command deployment with `az webapp deploy`
- ✅ **Auto-scaling** - Handles traffic spikes automatically
- ✅ **HTTPS by default** - Free SSL certificates
- ✅ **Environment variables** - Secure credential management
- ✅ **Continuous deployment** - GitHub/Azure DevOps integration

**Deployment tools:**
- `Tools/deploy.sh` - Automated deployment script
- `Tools/deep-clean.sh` - GitHub-ready cleanup script
- Azure CLI - Command-line deployment tool

---

### **Autodesk Platform Services (APS) APIs** 🏗️

#### **Data Management API** 📁
**What it provides:**
- Get hubs (BIM 360/ACC accounts)
- List projects
- Browse folders and files
- Create folders
- Upload/download files

**Endpoints we use:**
- `GET /project/v1/hubs`
- `GET /project/v1/hubs/{hub_id}/projects`
- `GET /data/v1/projects/{project_id}/folders/{folder_urn}/contents`
- `POST /data/v1/projects/{project_id}/folders`

---

#### **ACC API (Account Admin)** 👥
**What it provides:**
- List companies
- Get account users
- Retrieve project users
- Manage industry roles
- Set permissions

**Endpoints we use:**
- `GET /hq/v1/accounts/{account_id}/companies`
- `GET /hq/v1/accounts/{account_id}/users`
- `GET /hq/v1/accounts/{account_id}/projects/{project_id}/users`
- `GET /construction/admin/v1/accounts/{account_id}/industry-roles`
- `GET /bim360/admin/v1/projects/{project_id}/folders/{folder_id}/permissions`

---

### **Package Management**

#### **NuGet** 📦
**What it is:** .NET's package manager (like npm for Node.js).

**How we use it:**
- `dotnet restore` - Downloads all dependencies
- `dotnet build` - Compiles project and packages
- `.csproj` file - Defines package versions

**Our packages:**
```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="9.0.6" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
<PackageReference Include="RestSharp" Version="112.1.0" />
```

---

### **Development Tools**

#### **Visual Studio Code** (Recommended) 💻
**What it is:** Lightweight, cross-platform code editor.

**Required extensions:**
- C# Dev Kit (by Microsoft)
- C# (by Microsoft)

---

#### **Azure CLI** 🔧
**What it is:** Command-line tool for managing Azure resources.

**What we use it for:**
- `az login` - Authenticate with Azure
- `az webapp deploy` - Deploy application
- `az webapp config appsettings set` - Configure environment variables

---

### **Why We Chose REST API Instead of Autodesk SDKs**

Autodesk provides official SDKs for various languages ([NuGet Profile](https://www.nuget.org/profiles/AutodeskPlatformServices.SDK)), but we deliberately chose to use the **REST API approach** for this workshop.

#### **Benefits of REST API Approach:**
- ✅ **Educational value** - Learn how HTTP APIs work under the hood
- ✅ **Universal knowledge** - REST API skills transfer to ANY API (not just APS)
- ✅ **Minimal dependencies** - Only Swashbuckle, no APS-specific packages
- ✅ **Transparency** - See exactly what requests are being made
- ✅ **Flexibility** - Easy to customize and extend
- ✅ **Cross-language** - Same concepts apply to Python, Java, JavaScript, etc.

#### **When to Use Autodesk SDKs:**
- ✅ **Production applications** - Strongly-typed models and error handling
- ✅ **Complex integrations** - Multi-API workflows
- ✅ **Type safety** - IntelliSense and compile-time checking
- ✅ **Reduced boilerplate** - Less code for common operations

#### **Available Autodesk SDKs:**
- `Autodesk.DataManagement` - Hubs, projects, folders, files
- `Autodesk.Authentication` - OAuth 2.0 flows
- `Autodesk.ACC.AccountAdmin` - Companies, users, roles
- `Autodesk.ModelDerivative` - 3D model conversion and viewing

**For learning and workshops → Use REST API (this project)**  
**For production applications → Consider Autodesk SDKs**

---

### **Architecture Pattern: Clean Separation of Concerns**

```
┌─────────────────────────────────────────────┐
│             Browser (User)                  │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│        Controllers (HTTP Handlers)          │
│  • Validate input parameters                │
│  • Call services                            │
│  • Return HTTP responses                    │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│      Services (Business Logic)              │
│  • Make HTTP calls to APS APIs              │
│  • Handle JSON serialization                │
│  • Error handling                           │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│         Autodesk APS APIs                   │
│  • Data Management API                      │
│  • ACC API                                  │
│  • Authentication API                       │
└─────────────────────────────────────────────┘
```

**Benefits:**
- ✅ **Testability** - Each layer can be tested independently
- ✅ **Maintainability** - Changes in one layer don't affect others
- ✅ **Reusability** - Services can be used by multiple controllers
- ✅ **Clarity** - Each file has a single, clear purpose

---

## 💻 Prerequisites & Required Software

Before you can open, compile, debug, and run this project, you need to install the following software on your PC or Mac:

### ✅ **Required Software**

#### 1. **.NET 9 SDK** (Required)
**What it is:** The software development kit needed to build and run .NET applications.

**Download:** [https://dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)

**Installation:**
- **Windows:** Download and run the installer (`.exe`)
- **Mac:** Download and run the installer (`.pkg`) or use Homebrew: `brew install dotnet@9`
- **Linux:** Follow instructions for your distribution at the link above

**Verify Installation:**
```bash
dotnet --version
```
Expected output: `9.0.x` or higher

---

#### 2. **Code Editor / IDE** (Choose ONE)

**Option A: Visual Studio Code (Recommended for beginners)**
- **What it is:** Free, lightweight code editor with excellent .NET support
- **Download:** [https://code.visualstudio.com/](https://code.visualstudio.com/)
- **Required Extensions:**
  - C# Dev Kit (by Microsoft) - Install from Extensions marketplace
  - C# (by Microsoft) - Usually auto-installed with C# Dev Kit
- **Why choose this:** Simple, fast, works on Windows/Mac/Linux, great for learning

**Option B: Visual Studio 2022 (Windows only)**
- **What it is:** Full-featured IDE from Microsoft with built-in .NET tools
- **Download:** [https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/)
- **Edition:** Community (free), Professional, or Enterprise
- **Workload needed:** ASP.NET and web development
- **Why choose this:** Most powerful, best debugging, Windows only

---

### 🔧 **Optional but Recommended Tools**

#### 3. **Git** (Version Control)
**What it is:** Version control system to manage code changes.

**Download:** [https://git-scm.com/downloads](https://git-scm.com/downloads)

**Why you need it:** To clone this repository and track changes.

**Verify Installation:**
```bash
git --version
```

---

#### 4. **Postman** (API Testing Tool)
**What it is:** Tool for testing APIs outside of Swagger UI.

**Download:** [https://www.postman.com/downloads/](https://www.postman.com/downloads/)

**Why you need it:** Test APS APIs directly, save API calls, share with team.

---

### 📋 **System Requirements**

**Minimum:**
- **OS:** Windows 10/11, macOS 11+, or Linux (Ubuntu 20.04+)
- **RAM:** 4 GB (8 GB recommended)
- **Disk Space:** 2 GB free space
- **Internet:** Required (to download packages and access APS APIs)

**Recommended for smooth development:**
- **RAM:** 8 GB or more
- **CPU:** Multi-core processor
- **Disk Space:** 5 GB+ (for SDK, IDE, and packages)

---

### 🚀 **Quick Setup Checklist**

Before starting the workshop, verify you have everything:

- [ ] ✅ .NET 9 SDK installed (`dotnet --version` shows 9.x)
- [ ] ✅ Code editor installed (VS Code + C# Dev Kit, Visual Studio, or Rider)
- [ ] ✅ Git installed (optional but recommended)
- [ ] ✅ Internet connection available
- [ ] ✅ Autodesk account created (free at [https://aps.autodesk.com/](https://aps.autodesk.com/))
- [ ] ✅ APS app created with ClientId and ClientSecret

**Test Your Setup:**
```bash
# 1. Check .NET version
dotnet --version

# 2. Clone or download this repository
git clone <repository-url>

# 3. Navigate to project folder
cd acc-project-template-sync

# 4. Restore packages
dotnet restore

# 5. Build the project
dotnet build

# 6. Run the project
dotnet run --urls=http://localhost:8080
```

If all commands succeed, you're ready to go! 🎉

---

### 🆘 **Troubleshooting Setup Issues**

**Problem: "dotnet: command not found"**
- **Solution:** .NET SDK not installed or not in PATH. Reinstall .NET SDK and restart terminal.

**Problem: "The current .NET SDK does not support targeting .NET 9.0"**
- **Solution:** You have an older .NET version. Download and install .NET 9 SDK.

**Problem: Visual Studio Code can't find .NET**
- **Solution:** Install C# Dev Kit extension, then reload VS Code.

**Problem: Build errors about missing packages**
- **Solution:** Run `dotnet restore` in the project folder.

**Problem: Port 8080 already in use**
- **Solution:** Change port in command: `dotnet run --urls=http://localhost:5000`

---

## 🆕 What's New in This Version?

This workshop now includes enterprise-grade features to make it production-ready and even more user-friendly!

### 🔐 **Autodesk SSO Authentication**
- **Secure Login**: Users must authenticate with their Autodesk account before accessing the app
- **Custom Middleware**: `APSAuthenticationMiddleware` protects all routes
- **Beautiful Login Page**: Professional UI with Autodesk branding (`login.html`)
- **Session Management**: Secure 60-minute sessions with server-side storage

### 👤 **User Profile Display**
- **Real-time User Info**: See your name and email in the top-right of Swagger UI
- **Profile Pictures**: Displays your Autodesk profile photo (or initials)
- **Personalized Experience**: Know who's logged in at all times
- **New Service**: `UserInfoService` fetches profile from Autodesk UserProfile API

### 📖 **Comprehensive Help System**
- **User Manual** (`help.html`): Step-by-step guidance for non-technical users
  - How to use each API section
  - What values to input and why
  - 2-legged vs 3-legged tokens explained
  - Inspiring Lego analogy about APS
  - Troubleshooting common errors
- **Developer Guide** (`about.html`): Technical documentation for developers
  - Architecture patterns (Controllers, Services, Middleware)
  - Swashbuckle deep dive and customization
  - OAuth 2.0 and SSO implementation
  - How to extend the app with new endpoints
  - Code examples and best practices
- **Accessible**: Links in Swagger banner for instant access

### 🎨 **Modern UI Design**
- **Autodesk Branding**: White, black, and yellow color scheme
- **Glassmorphic Effects**: Modern, translucent design elements
- **Responsive Layout**: Works on desktop, tablet, and mobile
- **Custom Swagger Banner**: Shows authentication status, user info, help link, and logout button
- **Professional Appearance**: Production-ready look and feel

### 📦 **Stable Packages**
- **Swashbuckle.AspNetCore**: Version `7.0.0` (most stable for .NET 9 + Azure deployments)
- **Session Support**: Built into .NET 9 (no separate package needed)
- **Minimal Dependencies**: Only ONE NuGet package required (Swashbuckle)
- **Production-Ready**: Version 7.0.0 proven stable in Azure App Service (versions 8.x/9.x have rendering issues)

> **📝 Note on Swashbuckle Version:**  
> We use Swashbuckle 7.0.0 (not the latest 9.x) because:
> - ✅ Fully compatible with .NET 9
> - ✅ Works perfectly in Azure App Service
> - ✅ No OpenAPI rendering issues
> - ❌ Versions 8.x and 9.x have known bugs with OpenAPI document generation in Azure
> - ❌ Conflict with `Microsoft.AspNetCore.OpenApi` (removed for stability)
> 
> **Tested and verified:** This version is production-ready and works reliably across all environments.

### 🏗️ **Architecture Improvements**
- **Middleware Pattern**: Learn how to implement custom request interceptors
- **Static File Serving**: Understand how to serve HTML, CSS, JS from `wwwroot`
- **Session State**: Master secure session management in ASP.NET Core
- **OAuth Integration**: Full 3-legged OAuth flow with Autodesk
- **Service Layer Expansion**: New `UserInfoService` for profile data

### 🔄 **Enhanced Workflow**
```
Before: Direct access to Swagger → Manually enter tokens
After:  Login with Autodesk → Auto token management → Personalized UI
```

**Result**: A more secure, professional, and user-friendly workshop experience! 🎉

---

## 🎯 What You'll Learn

This workshop covers essential APS APIs:

### Data Management API
- **Hubs** - Retrieve BIM 360/ACC accounts
- **Projects** - Get projects within hubs
- **Folders** - Navigate, create, and search folders
- **Files** - Access and manage project files

### ACC (Autodesk Construction Cloud) API
- **Companies** - Get organizations involved in projects
- **Industry Roles** - Retrieve available role templates
- **Account Users** - Manage account-level users
- **Project Users** - Manage project-specific users
- **Permissions** - Control folder access

### Authentication
- **2-Legged OAuth** - Server-to-server authentication
- **3-Legged OAuth** - User login and authorization

---

## 🔧 Why REST API Instead of Autodesk SDKs?

This workshop takes a **REST API-first approach** using direct HTTP calls (`HttpClient`) instead of the official [Autodesk Platform Services SDKs](https://www.nuget.org/profiles/AutodeskPlatformServices.SDK). Here's why:

### 📖 **Educational Value (Our Priority)**

**WHAT YOU LEARN:**
- ✅ **How OAuth 2.0 actually works** - See the token request/response flow
- ✅ **HTTP fundamentals** - Headers, methods (GET/POST), status codes, request bodies
- ✅ **JSON parsing** - Understand API responses at a low level
- ✅ **REST principles** - Resource URLs, query parameters, pagination
- ✅ **Debugging skills** - Inspect raw HTTP traffic, identify issues quickly
- ✅ **Transferable knowledge** - Apply these skills to ANY REST API (not just Autodesk)

**With SDKs, you'd learn:**
- ❌ How to call pre-built methods (limited transferability)
- ❌ Less visibility into what's happening "under the hood"

### 🎯 **When to Use Each Approach**

| **REST API (This Workshop)** | **Autodesk SDKs** |
|------------------------------|-------------------|
| 🎓 **Learning** how APIs work | 🚀 **Production** apps (speed matters) |
| 🔧 **Custom integrations** | 🏢 **Standard workflows** |
| 🔍 **Debugging** existing integrations | ✅ **Type-safe** code (IntelliSense) |
| 📚 **Teaching** others | ⚡ **Rapid development** |
| 🌐 **Multi-language** teams | 🛡️ **Built-in error handling** |

### 📦 **Available Autodesk SDKs (NuGet)**

If you prefer the SDK approach after this workshop, these are available:

```xml
<!-- Official Autodesk Platform Services SDKs -->
<PackageReference Include="Autodesk.Authentication" Version="2.0.1" />
<PackageReference Include="Autodesk.DataManagement" Version="2.1.2" />
<PackageReference Include="Autodesk.Oss" Version="2.2.3" />
<PackageReference Include="Autodesk.ModelDerivative" Version="2.2.0" />
<PackageReference Include="Autodesk.Webhooks" Version="3.3.0" />
<PackageReference Include="Autodesk.SDKManager" Version="1.1.2" />
<PackageReference Include="Autodesk.Construction.Issues" Version="4.0.0-beta" />
<PackageReference Include="Autodesk.Construction.AccountAdmin" Version="3.1.0-beta" />
```

**Find them here:** [NuGet: AutodeskPlatformServices.SDK](https://www.nuget.org/profiles/AutodeskPlatformServices.SDK)

### 💡 **Our Minimal Dependencies**

**This workshop uses ONLY ONE NuGet package:**

```xml
<ItemGroup>
  <PackageReference Include="Swashbuckle.AspNetCore" Version="7.0.0" />
</ItemGroup>
```

Everything else is built-in to .NET 9:
- `HttpClient` - For REST API calls
- `System.Text.Json` - For JSON parsing
- `Microsoft.AspNetCore.Session` - For session management
- `Microsoft.AspNetCore.Authentication` - For OAuth flows

**Benefits:**
- ✅ Fewer dependencies = fewer breaking changes
- ✅ Learn modern .NET features
- ✅ Understand what SDKs abstract away
- ✅ Full control over every request

### 🚀 **After This Workshop**

Once you've completed this workshop and understand the fundamentals, you can:

1. **✅ Use Autodesk SDKs** in production apps (now you'll know what they're doing!)
2. **✅ Debug SDK issues** (you understand the underlying HTTP calls)
3. **✅ Mix approaches** (SDK for common tasks, REST for custom needs)
4. **✅ Make informed decisions** about architecture

**The goal:** Make you a confident APS developer, regardless of which tools you choose! 🎓

---

## 🛠️ Setup Instructions

### Prerequisites

- **.NET 9 SDK** ([Download here](https://dotnet.microsoft.com/download/dotnet/9.0))
- **APS App** with credentials from [APS Portal](https://aps.autodesk.com/)

### Step 1: Get Your APS Credentials

1. Go to [APS Portal](https://aps.autodesk.com/)
2. Create a new app (or use an existing one)
3. **Enable these APIs** in your app:
   - ✅ Data Management API
   - ✅ BIM 360 API
   - ✅ ACC API (Autodesk Construction Cloud)
4. Copy your **Client ID** and **Client Secret**
5. Add callback URL: `http://localhost:8080/auth/callback` (no `/api/` prefix!)

### Step 2: Configure the App

Open `appsettings.json` and add your credentials:

```json
{
  "APS": {
    "ClientId": "YOUR_CLIENT_ID_HERE",
    "ClientSecret": "YOUR_CLIENT_SECRET_HERE",
    "CallbackUrl": "http://localhost:8080/auth/callback",
    "BaseUrl": "https://developer.api.autodesk.com",
    "OAuth2AuthorizeUrl": "https://developer.api.autodesk.com/authentication/v2/authorize",
    "OAuth2GetTokenUrl": "https://developer.api.autodesk.com/authentication/v2/token"
  }
}
```

### Step 3: Run the Application

```bash
# Navigate to the project directory
cd acc-project-template-sync

# Restore dependencies
dotnet restore

# Run the application
dotnet run --urls=http://localhost:8080
```

### Step 4: Login with Autodesk SSO

Open your browser and go to:
```
http://localhost:8080
```

**You'll see a secure login page:**
1. 🔐 Click "Login with Autodesk SSO"
2. 🔑 Enter your Autodesk credentials (the same ones you use for ACC/BIM 360)
3. ✅ Authorize the app to access your data
4. 🎉 You'll be redirected to the interactive Swagger UI

**After login, you'll see:**
- Your name and email displayed in the top-right corner
- Full access to all API endpoints
- Interactive "Try it out" buttons for each endpoint
- A link to the Help Guide for non-technical users

---

## 📚 How to Use the API

### Basic Workflow

1. **Get a Token** 🔑
   - Go to `1. Authentication` → `GET /auth/token` (2-Legged Token)
   - Click "Execute"
   - Copy the `access_token` from the response
   - **Note:** Token includes scopes: `data:read data:write data:create data:search account:read`

2. **Explore Hubs** 🏢
   - Go to `2. Hubs` → `GET /hubs`
   - Paste your token in the `token` field
   - Execute to see all your hubs
   - Copy a `hubId` (⚠️ includes "b." prefix, e.g., "b.3def0ac0-6276...")

3. **Get Projects** 📋
   - Go to `3. Projects` → `GET /projects`
   - Enter your token and hubId (⚠️ must include "b." prefix)
   - Execute to see all projects in that hub
   - Copy a `projectId` (also has "b." prefix)

4. **Continue Exploring!** 🚀
   - Follow the numbered sections in Swagger UI
   - Each endpoint has detailed documentation
   - Try different endpoints to see what data you can access

### ⚠️ Important: "b." Prefix Rules

**Data Management API endpoints** (Hubs, Projects, Folders):
- ✅ **MUST include "b." prefix**
- Example: `b.e66ece9f-5035-44da-b9ff-6e918509ca48`

**ACC API endpoints** (Companies, Roles, Users):
- ❌ **NO "b." prefix** (we auto-remove it for you!)
- Example: `3def0ac0-6276-4c72-89ec-81948b43496f`

---

## 📁 Project Structure Explained

### Overview

```
acc-project-template-sync/
├── bin/                          # Compiled output (auto-generated)
├── Controllers/                  # API Endpoints (HTTP routes)
├── Middleware/                   # Custom middleware (SSO authentication, security headers)
├── Models/                       # Data structures
├── Properties/                   # Project properties
├── Services/                     # Business logic
├── Tools/                        # Deployment scripts and utilities
│   ├── deploy.sh                 # Azure deployment script (automated)
│   └── README.md                 # Tools documentation
├── wwwroot/                      # Static files (login.html, help.html, about.html)
├── .gitignore                    # Git ignore rules
├── appsettings.json              # Configuration (⚠️ add your credentials here)
├── aps-starter-pack-swagger-ui.csproj  # Project file
├── LICENSE                       # MIT License
├── Program.cs                    # Application entry point
└── README.md                     # This file!
```

### 🎮 Controllers vs Services (Important Concept!)

**Controllers** = The "Front Desk" 🏨
- Handles HTTP requests from users
- Validates input parameters
- Returns HTTP responses
- **Does NOT** make API calls directly

**Services** = The "Worker" 👷
- Contains the actual business logic
- Makes HTTP calls to Autodesk APIs
- Processes and returns data
- Can be reused by multiple controllers

**Example:**
```
User → Controller → Service → Autodesk API
                ↓
            Response
```

---

### 🎨 What is Swashbuckle.AspNetCore? (The Magic Behind Swagger UI)

**Swashbuckle.AspNetCore** is the NuGet package that creates the **entire beautiful Swagger UI interface** you see when you open the app!

#### 🔧 What It Does:

**1. Auto-Generates API Documentation**
   - Scans your Controllers and Methods automatically
   - Reads XML comments (your `/// <summary>` tags)
   - Creates OpenAPI/Swagger JSON specification from your code

**2. Provides the Interactive Swagger UI**
   - The beautiful web interface at `http://localhost:8080`
   - The "Try it out" buttons
   - The expandable sections (Authentication, Hubs, Projects, etc.)
   - Parameter input fields
   - Response viewers with JSON formatting

**3. Enables Customization**
   - Custom operation IDs (2LeggedToken, 3LeggedToken)
   - Tags for grouping endpoints
   - Enum dropdowns (US, EMEA)
   - Custom HTML/CSS (our yellow Autodesk-branded banner)
   - User profile display

---

#### 📊 How Swashbuckle Works:

```
Your C# Code           Swashbuckle              What You See in Browser
─────────────────────────────────────────────────────────────────────
Controllers/           →  Reads & Analyzes  →   Swagger UI Interface
├── AuthController          |                    ┌──────────────────┐
├── HubsController          |                    │ GET /hubs        │
├── ProjectsController      ↓                    │ GET /projects    │
└── ...                Generates OpenAPI         │ GET /folders     │
                       Specification             │ [Try it out]     │
                            ↓                    └──────────────────┘
                       Serves Beautiful
                       Interactive UI
```

---

#### 💡 Without Swashbuckle:

If you removed `Swashbuckle.AspNetCore` from the project:

❌ **No Swagger UI** - Just raw, invisible API endpoints
❌ **No visual documentation** - Users would need Postman, curl, or code
❌ **No interactive testing** - Can't click "Try it out"
❌ **No user-friendly interface** - Just code and command line
❌ **Impossible for non-IT users** - No visual way to explore APIs

---

#### ✅ With Swashbuckle:

✅ **Beautiful Swagger UI** - Point-and-click interface
✅ **Auto documentation** - Generated from your code comments
✅ **Test endpoints instantly** - "Try it out" buttons for everything
✅ **Perfect for workshops** - Visual, intuitive, beginner-friendly
✅ **Non-technical friendly** - Anyone can explore and test APIs
✅ **Self-documenting** - Your XML comments appear as descriptions
✅ **Professional presentation** - Custom branding with Autodesk colors

---

#### 🎓 Why This Matters for Your Workshop:

The **entire workshop interface IS Swagger UI** powered by Swashbuckle:

- **Visual Learning**: Participants can **see** all available endpoints
- **Interactive Testing**: They can **try** APIs without writing code
- **Immediate Feedback**: Responses appear instantly with formatted JSON
- **Documentation**: Every endpoint shows what it does and what parameters it needs
- **Low Barrier to Entry**: Non-developers can learn APIs hands-on

**Without Swashbuckle, there would be no workshop!** It's the foundation that makes this educational experience possible.

---

#### 🔍 In Your Code (Program.cs):

```csharp
// Add Swagger generation with custom configuration
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(apiDesc => ...);      // Custom endpoint names
    c.UseInlineDefinitionsForEnums();          // US/EMEA instead of 0/1
    c.SwaggerDoc("v1", new OpenApiInfo {       // Document metadata
        Title = "APS Starter Pack Workshop",
        Version = "v1"
    });
});

// Enable Swagger middleware
app.UseSwagger();           // Serves the OpenAPI JSON at /swagger/v1/swagger.json
app.UseSwaggerUI(c =>       // Serves the interactive UI at /
{
    c.HeadContent = @"..."; // Custom banner with user info
});
```

This is what transforms your C# API code into the beautiful interface!

---

#### 📦 Current Package Version:

We're using **Swashbuckle.AspNetCore**: `7.0.0`

This version provides:
- ✅ Stable OpenAPI document generation
- ✅ Reliable Swagger UI rendering
- ✅ Full enum support (US/EMEA dropdowns)
- ✅ Custom operation IDs and branding

---

#### 🎨 Real Example:

When you write this simple code:

```csharp
/// <summary>
/// Get all hubs (BIM 360/ACC accounts)
/// </summary>
[HttpGet("hubs")]
public async Task<IActionResult> GetHubs(string token)
{
    var hubs = await _hubsService.GetHubsAsync(token);
    return Ok(hubs);
}
```

**Swashbuckle automatically creates this beautiful UI:**

```
┌─────────────────────────────────────────────┐
│ GET  /hubs  📝 Data Management API          │
│                                             │
│ Get all hubs (BIM 360/ACC accounts)         │
│                                             │
│ Parameters:                                 │
│   token * (string, query) [required]        │
│   ┌─────────────────────────────────────┐   │
│   │ Enter token here...                 │   │
│   └─────────────────────────────────────┘   │
│                                             │
│ [Try it out]  [Execute]                     │
│                                             │
│ Responses:                                  │
│   200  Success                              │
│   {                                         │
│     "data": [ ... ]                         │
│   }                                         │
└─────────────────────────────────────────────┘
```

**No manual HTML, no JavaScript, no extra work!** Just write good C# code with comments, and Swashbuckle does the rest!

---

#### 🚀 Bottom Line:

**Swashbuckle = The Workshop's Visual Interface**

It's the magic that turns invisible API endpoints into a beautiful, interactive learning environment where participants can:
- 👀 **See** all available endpoints organized by category
- 📖 **Read** clear documentation with examples
- 🧪 **Test** APIs in real-time with instant results
- 🎓 **Learn** API concepts without writing a single line of code
- 💪 **Gain confidence** before diving into actual development

**This is why every .NET API workshop uses Swagger/Swashbuckle!** ✨

---

## 📂 Detailed File Structure

### 📁 `/Controllers` - API Endpoints (What Users Call)

Controllers handle incoming HTTP requests and return responses.

#### `AuthenticationController.cs`
**What it does:**
- `GET /auth/token` - Get 2-Legged Token (server-to-server auth)
- `GET /auth/login` - Initiates 3-Legged OAuth flow (redirects to Autodesk login)
- `GET /auth/callback` - Handle OAuth callback and store user session
- `GET /auth/logout` - Clear session and return to login page
- `GET /auth/current-token` - Retrieve stored access token from session
- `GET /auth/current-user` - Get logged-in user's profile (name, email, picture)

**Learn:** 
- How OAuth 2.0 authentication works with APS
- How to implement Autodesk SSO (Single Sign-On)
- How to manage user sessions securely
- Difference between 2-legged (app) and 3-legged (user) authentication

---

#### `HubsController.cs`
**What it does:**
- `GET /hubs` - List all hubs (BIM 360/ACC accounts)
- `GET /hubs/{hubId}` - Get details of a specific hub

**Learn:** How to retrieve and navigate ACC accounts

**⚠️ Note:** hubId MUST include "b." prefix

---

#### `ProjectsController.cs`
**What it does:**
- `GET /projects` - List all projects in a hub
- `GET /projects/{projectId}` - Get project details
- `GET /projects/{projectId}/topFolders` - Get root folders

**Learn:** How to access project information and structure

**⚠️ Note:** Both hubId and projectId MUST include "b." prefix

---

#### `CompaniesController.cs`
**What it does:**
- `GET /companies` - List all companies in an account
- `GET /companies/project/{projectId}` - Get companies in a project

**Learn:** How to retrieve organizations involved in projects

**API:** ACC API (requires `account:read` scope)

---

#### `RolesController.cs`
**What it does:**
- `GET /roles/projects/{projectId}` - Get industry roles for a project

**Learn:** How to retrieve available role templates (Architect, Engineer, BIM Manager, etc.)

**API:** ACC API v2 (uses `/industry_roles` endpoint)

---

#### `AccountUsersController.cs`
**What it does:**
- `GET /users` - List all users in an account
- `GET /users/{userId}` - Get specific user details
- `GET /users/search` - Search for users by email

**Learn:** How to manage account-level users

**API:** ACC API (requires `account:read` scope)

---

#### `ProjectUsersController.cs`
**What it does:**
- `GET /projects/{projectId}/users` - List users in a project with their roles
- `GET /projects/{projectId}/users/{userId}` - Get specific user in project

**Learn:** How to see which users have access to a project and their assigned roles

**API:** ACC Admin API v1

---

#### `FoldersController.cs`
**What it does:**
- `GET /folders/contents` - List folder contents (subfolders and files)
- `GET /folders` - Get folder details
- `POST /folders` - Create a new folder
- `GET /folders/search` - Search for folders/files by name

**Learn:** How to navigate and manage folder structures

**⚠️ Note:** 
- projectId MUST include "b." prefix
- folderUrn format: `urn:adsk.wipprod:fs.folder:co.xxxxx`
- Search requires `data:search` scope

---

#### `PermissionsController.cs`
**What it does:**
- `GET /permissions/example` - Get example permission structure
- `GET /permissions` - View folder permissions
- `POST /permissions` - Set folder permissions

**Learn:** How to control who can access folders

**API:** ACC API (Docs permissions)

---

### 📁 `/Services` - Business Logic (Does the Work)

Services contain the actual logic for making API calls to Autodesk.

#### `AuthenticationService.cs`
**Responsibilities:**
- Generates OAuth URLs
- Exchanges credentials/codes for access tokens
- Handles token refresh logic

**Key Methods:**
- `GetTwoLeggedTokenAsync()` - Get app token
- `GetThreeLeggedAuthorizationUrl()` - Generate login URL
- `GetThreeLeggedTokenAsync()` - Exchange authorization code for user token

---

#### `UserInfoService.cs`
**Responsibilities:**
- Fetches user profile information from Autodesk
- Retrieves name, email, and profile picture
- Used after successful SSO login

**Key Methods:**
- `GetUserProfileAsync(accessToken)` - Get user profile from Autodesk UserProfile API

**API Endpoint:**
- `GET https://api.userprofile.autodesk.com/userinfo`

**Returns:**
- User's full name
- Email address
- Profile picture URL
- Autodesk user ID

**Learn:** How to retrieve authenticated user information after SSO login

---

#### `HubsService.cs`
**Responsibilities:**
- Makes HTTP calls to Data Management API `/project/v1/hubs` endpoints
- Retrieves hub data

**API Endpoint:** `https://developer.api.autodesk.com/project/v1/hubs`

---

#### `ProjectsService.cs`
**Responsibilities:**
- Makes HTTP calls to Data Management API `/project/v1/hubs/{hubId}/projects` endpoints
- Retrieves project and folder data

**API Endpoints:**
- `GET /project/v1/hubs/{hubId}/projects`
- `GET /project/v1/hubs/{hubId}/projects/{projectId}`
- `GET /project/v1/hubs/{hubId}/projects/{projectId}/topFolders`

---

#### `CompaniesService.cs`
**Responsibilities:**
- Makes HTTP calls to ACC Admin API `/hq/v1` companies endpoints
- Handles region parameter (US/EMEA)
- Auto-strips "b." prefix from accountId

**API Endpoints:**
- `GET /hq/v1/regions/{region}/accounts/{accountId}/companies`
- `GET /hq/v1/regions/{region}/accounts/{accountId}/projects/{projectId}/companies`

---

#### `RolesService.cs`
**Responsibilities:**
- Makes HTTP calls to ACC API v2 `/hq/v2` industry_roles endpoint
- Retrieves available role templates

**API Endpoint:**
- `GET /hq/v2/accounts/{accountId}/projects/{projectId}/industry_roles`

---

#### `AccountUsersService.cs`
**Responsibilities:**
- Makes HTTP calls to ACC Admin API `/hq/v1` users endpoints
- Manages account-level user operations

**API Endpoints:**
- `GET /hq/v1/regions/{region}/accounts/{accountId}/users`
- `GET /hq/v1/regions/{region}/accounts/{accountId}/users/{userId}`
- `GET /hq/v1/regions/{region}/accounts/{accountId}/users/search`

---

#### `ProjectUsersService.cs`
**Responsibilities:**
- Makes HTTP calls to ACC Admin API v1 `/construction/admin/v1` endpoints
- Retrieves project-specific user assignments

**API Endpoints:**
- `GET /construction/admin/v1/projects/{projectId}/users`
- `GET /construction/admin/v1/projects/{projectId}/users/{userId}`

---

#### `FoldersService.cs`
**Responsibilities:**
- Makes HTTP calls to Data Management API `/data/v1` folders endpoints
- URL-encodes folderUrns (they contain special characters)
- Handles folder CRUD operations

**API Endpoints:**
- `GET /data/v1/projects/{projectId}/folders/{folderUrn}/contents`
- `GET /data/v1/projects/{projectId}/folders/{folderUrn}`
- `POST /data/v1/projects/{projectId}/folders`
- `GET /data/v1/projects/{projectId}/folders/{folderUrn}/search`

---

#### `PermissionsService.cs`
**Responsibilities:**
- Makes HTTP calls to ACC Docs API permissions endpoints
- Manages folder-level access control

**API Endpoints:**
- `GET /bim360/docs/v1/projects/{projectId}/folders/{folderId}/permissions`
- `POST /bim360/docs/v1/projects/{projectId}/folders/{folderId}/permissions`

---

### 📁 `/Middleware` - Custom Middleware

Middleware intercepts HTTP requests before they reach controllers.

#### `APSAuthenticationMiddleware.cs`
**What it does:**
- Enforces Autodesk SSO login for all Swagger UI access
- Checks for valid access token in session
- Redirects unauthenticated users to login page
- Allows public access to login, authentication endpoints, and static files

**How it works:**
```
1. User tries to access Swagger UI (/)
2. Middleware checks session for APS_AccessToken
3. If token exists → Allow access to Swagger
4. If no token → Redirect to /login.html
```

**Protected Routes:**
- `/` (Swagger UI root)
- `/help.html` (User Manual)
- `/about.html` (Developer Guide)

**Public Routes (no auth required):**
- `/login.html` (Login page)
- `/auth/*` (Authentication endpoints)
- `/swagger/*` (Swagger JSON spec)
- Static files (CSS, JS, images)

**Learn:** 
- How to implement custom ASP.NET Core middleware
- How to protect routes with authentication
- How to work with ASP.NET Core sessions

---

#### `SecurityHeadersMiddleware.cs`
**What it does:**
- Adds 6 essential HTTP security headers to all responses
- Protects against common web vulnerabilities (XSS, clickjacking, MIME sniffing)
- Required for production deployments, especially on Azure
- Implements industry security best practices

**The 6 Security Headers:**
1. **Strict-Transport-Security (HSTS)** - Forces HTTPS only (prevents man-in-the-middle attacks)
2. **Content-Security-Policy (CSP)** - Controls which resources can be loaded (prevents XSS attacks)
3. **X-Frame-Options** - Prevents site from being embedded in iframes (prevents clickjacking)
4. **X-Content-Type-Options** - Prevents browser from MIME sniffing (stops content type attacks)
5. **Referrer-Policy** - Controls referrer information sent to other sites (protects privacy)
6. **Permissions-Policy** - Disables unnecessary browser features (reduces attack surface)

**How it works:**
```
1. Every HTTP response passes through this middleware
2. Middleware adds security headers to response
3. Browser receives response with security headers
4. Browser enforces security policies defined in headers
```

**Why this matters:**
- ✅ **Production Security** - Required for enterprise deployments
- ✅ **Compliance** - Helps pass security audits
- ✅ **User Protection** - Protects users from malicious attacks
- ✅ **Best Practice** - Industry-standard security pattern

**Usage in Program.cs:**
```csharp
app.UseSecurityHeaders(); // Automatically adds all 6 headers
```

**Special Configuration for Swagger:**
- CSP is relaxed to allow Swagger UI's inline scripts and styles
- `connect-src` allows connections to Autodesk APS APIs
- Still secure, but functional for development and workshops

**Azure Deployment:**
- Automatically enables HSTS when running on Azure (HTTPS enforced)
- In development mode, HSTS is disabled (HTTP localhost is OK)
- All other headers are enabled in both development and production

**Learn:** 
- HTTP security headers and their purpose
- How to implement security middleware in ASP.NET Core
- Production security best practices
- How to balance security with functionality (Swagger needs)

---

#### `EmailWhitelistMiddleware.cs`
**What it does:**
- **Email-based access control** - Only specific users can access the application
- Runs AFTER `APSAuthenticationMiddleware` (user is already authenticated via Autodesk SSO)
- Checks if user's email is on the "allowed list"
- Redirects unauthorized users to `/access-denied.html`
- Supports wildcard domains (e.g., `*@autodesk.com` allows all Autodesk emails)

**Why you need this:**
> **Problem:** By default, ANYONE with a valid Autodesk account can log in and access ALL hubs/projects your APS app has permission to.
> 
> **Solution:** Email whitelist adds an extra security layer - only trusted users can access your workshop application.

**How it works:**
```
1. User logs in with Autodesk SSO (authenticated ✅)
2. EmailWhitelistMiddleware checks user's email against whitelist
3. IF email is on the list → Allow access to Swagger UI
4. IF email is NOT on the list → Redirect to access-denied.html
```

**Configuration (appsettings.json):**
```json
"AuthorizedUsers": {
  "AllowedEmails": [
    "your.email@company.com",
    "colleague@company.com",
    "*@autodesk.com"  // Allow all Autodesk employees
  ],
  "EnableWhitelist": true  // Set to false to disable
}
```

**Features:**
- ✅ **Case-insensitive** - `John@Email.com` = `john@email.com`
- ✅ **Wildcard domains** - `*@autodesk.com` allows all Autodesk emails
- ✅ **Toggle on/off** - Set `EnableWhitelist: false` to allow everyone
- ✅ **Logging** - Logs all unauthorized access attempts
- ✅ **Empty list handling** - If whitelist is empty AND enabled, denies all (safest default)

**Local Development:**
```json
// In appsettings.json
"AuthorizedUsers": {
  "AllowedEmails": [
    "your.email@company.com"
  ],
  "EnableWhitelist": false  // Disable for easier local testing
}
```

**Azure Production (Simple Format):**
```bash
# Enable whitelist in Azure App Settings - SUPER SIMPLE! ✨
az webapp config appsettings set \
  --name aps-starter-pack \
  --resource-group rg-aps-starting_pack \
  --settings AuthorizedUsers__EnableWhitelist="true" \
             AuthorizedUsers__AllowedEmails="iulian@autodesk.com, admin1@autodesk.com, admin2@company.com"
```

**Why this is better:**
- ✅ **One setting** instead of multiple `__0`, `__1`, `__2` entries
- ✅ **Easy to copy/paste** your email list
- ✅ **Easy to edit** - just add or remove emails in one line
- ✅ **Perfect for non-IT users**
- ✅ **SMART AUTO-DETECTION** - Automatically parses comma-separated strings!

**In Azure Portal:**
1. Click **"+ Add"**
2. Name: `AuthorizedUsers__AllowedEmails` (or `AuthorizedUsers__AllowedEmailsString` - both work!)
3. Value: `iulian@autodesk.com, admin1@autodesk.com, admin2@company.com`
4. Click **"+ Add"** again
5. Name: `AuthorizedUsers__EnableWhitelist`
6. Value: `true`
7. Click **Save** and **Restart** the app

Done! Much simpler than the old indexed format! 🎉

**⚠️ IMPORTANT:** After saving, you MUST restart the app for changes to take effect:
- Go to **Overview** → Click **Restart** at the top

**Usage in Program.cs:**
```csharp
app.UseMiddleware<APSAuthenticationMiddleware>();
app.UseEmailWhitelist(); // Add this line
```

**Access Denied Page:**
- Beautiful Autodesk-branded page at `/access-denied.html`
- Shows user's current email address
- Provides instructions to contact administrator
- Includes "Logout" and "Try Different Account" buttons

**Example Scenarios:**

**Scenario 1: Workshop for Autodesk Employees Only**
```json
"AllowedEmails": ["*@autodesk.com"],
"EnableWhitelist": true
```
✅ `john.doe@autodesk.com` → Access granted  
❌ `partner@company.com` → Access denied

**Scenario 2: Specific Users + Partner Company**
```json
"AllowedEmails": [
  "admin@company.com",
  "john.doe@company.com",
  "*@partner-company.com"
],
"EnableWhitelist": true
```
✅ `admin@company.com` → Access granted  
✅ `anyone@partner-company.com` → Access granted  
❌ `random@gmail.com` → Access denied

**Scenario 3: Development Mode (Allow Everyone)**
```json
"AllowedEmails": [],
"EnableWhitelist": false
```
✅ Everyone with Autodesk SSO can access

**Security Best Practices:**
- ✅ Enable whitelist in production (`EnableWhitelist: true`)
- ✅ Use specific emails when possible (more secure than wildcards)
- ✅ Regularly review and update the allowed list
- ✅ Monitor logs for unauthorized access attempts
- ⚠️ Don't commit sensitive emails to version control (use Azure App Settings)

**Learn:**
- How to implement custom authorization middleware
- Email-based access control patterns
- Configuration management (local vs. Azure)
- Security layers beyond authentication
- Wildcard pattern matching in C#

---

### 📁 `/wwwroot` - Static Files

Contains HTML pages and static assets served directly to the browser.

#### `login.html`
**What it does:**
- Beautiful login page with Autodesk branding
- "Login with Autodesk SSO" button
- Redirects to `/auth/login` to start OAuth flow

**Features:**
- Clean, modern UI with glassmorphic design
- Autodesk brand colors (white, black, yellow)
- Responsive layout
- Security notice for users

**User Flow:**
```
1. User visits http://localhost:8080 (not logged in)
2. Middleware redirects to /login.html
3. User clicks "Login with Autodesk SSO"
4. Redirected to Autodesk login page
5. After successful login, returned to Swagger UI
```

---

#### `help.html`
**What it does:**
- Comprehensive help guide for non-technical users
- Step-by-step instructions for every API section
- Explains what values to input and why
- Includes inspiring story about APS using Lego analogy

**Content Includes:**
- **Getting Started**: How to navigate Swagger UI
- **Authentication Section**: Difference between 2-legged and 3-legged tokens
- **Each API Section**: Detailed guidance (Hubs, Projects, Folders, etc.)
- **Real-World Use Cases**: When to use each endpoint
- **Troubleshooting**: Common errors and solutions
- **APS Inspiration**: How APS empowers you to build solutions

**Design:**
- Autodesk brand theme (white, black, yellow)
- Glassmorphic cards with subtle shadows
- Color-coded sections (info, warning, success)
- Easy-to-scan format with emojis
- "Back to Swagger" button

**Accessible from:**
- Link in Swagger UI banner: "📖 User Manual"
- Direct URL: `http://localhost:8080/help.html`
- Cross-link to Developer Guide

**Learn:** 
- How to serve static HTML files in ASP.NET Core
- How to create user-friendly documentation
- Best practices for non-technical user guidance

---

#### `about.html`
**What it does:**
- Comprehensive developer documentation with technical architecture details
- Interactive collapsible sections for easy navigation
- Code examples with syntax highlighting
- Deep dive into Swashbuckle, OAuth, Middleware, and more

**Content Includes:**
- **Architecture Overview**: Request flow diagrams, project structure explained
- **Controllers vs Services**: Why separate them, dependency injection explained
- **Swashbuckle Deep Dive**: How Swagger UI works, customization, version compatibility
- **Authentication & SSO**: Complete OAuth 2.0 flow, custom middleware implementation
- **Adding New Endpoints**: Step-by-step guide with code examples
- **API Patterns**: HTTP client config, validation, URL encoding best practices
- **Debugging & Troubleshooting**: Common issues, logging, testing with Postman
- **Extension Ideas**: Beginner to advanced project suggestions
- **Resources**: Links to official documentation, community forums, tools

**Design:**
- Autodesk brand theme (white, black, yellow)
- Collapsible sections for progressive disclosure
- Syntax-highlighted code blocks
- Architecture diagrams in ASCII art
- Tables for quick reference
- Color-coded information cards

**Accessible from:**
- Link in Swagger UI banner: "👨‍💻 Developer Guide"
- Direct URL: `http://localhost:8080/about.html`
- Cross-link from User Manual

**Target Audience:**
- Developers learning the codebase
- Workshop facilitators teaching architecture patterns
- Anyone extending or customizing the app

**Learn:** 
- Advanced ASP.NET Core patterns and best practices
- How to architect a clean, maintainable Web API

---

#### `access-denied.html`
**What it does:**
- Displays friendly error message when user's email is not authorized
- Shows which email address was used for login
- Provides instructions for requesting access
- Offers logout and "Try Different Account" options

**When it appears:**
- User successfully authenticates with Autodesk SSO
- BUT their email is not on the `AllowedEmails` whitelist
- AND `EnableWhitelist` is set to `true`

**Features:**
- ✅ **Shows user's email** - Fetched from session via `/auth/current-user` endpoint
- ✅ **Clear instructions** - What user needs to do to get access
- ✅ **Autodesk branding** - Consistent yellow/black/white theme
- ✅ **Actionable buttons** - "Logout" and "Try Different Account"
- ✅ **Responsive design** - Works on mobile, tablet, desktop

**User Flow:**
```
1. User logs in with john.doe@company.com (authenticated ✅)
2. EmailWhitelistMiddleware checks whitelist
3. Email NOT found in AllowedEmails
4. User redirected to /access-denied.html
5. Page shows "You are logged in as: john.doe@company.com"
6. User sees instructions to contact admin
7. User can logout or try different account
```

**Page Content:**
- 🚫 Icon and "Access Denied" heading
- User's current email address (dynamically loaded)
- Instructions box with steps to request access
- "Logout" button (clears session)
- "Try Different Account" button (re-login)

**Learn:**
- How to create user-friendly error pages
- Dynamic content loading with JavaScript fetch
- Session management and user info retrieval
- Security UX best practices
- OAuth 2.0 and SSO implementation in production
- How Swashbuckle generates Swagger UI from code

---

### 📁 `/Tools` - Deployment Scripts and Utilities

Contains automation scripts for deploying and managing the application.

#### `deploy.sh`
**What it does:**
- Automated, interactive Azure deployment script
- Creates all Azure resources (Resource Group, App Service Plan, Web App)
- Configures APS credentials securely in Azure App Settings
- Builds and deploys the .NET 9 application
- Enables HTTPS and security settings
- Tests the deployment and provides detailed feedback

**Features:**
- ✅ **Interactive prompts** - Guides you step-by-step
- ✅ **Educational output** - Explains what's happening at each step
- ✅ **Color-coded messages** - Green for success, red for errors, blue for info
- ✅ **Prerequisites checks** - Verifies Azure CLI and .NET SDK are installed
- ✅ **Error handling** - Stops on errors with helpful messages
- ✅ **Reusable** - Run again to redeploy updates quickly

**How to use:**
```bash
# Make executable (first time only)
chmod +x Tools/deploy.sh

# Run the deployment
./Tools/deploy.sh
```

**What you'll be asked for:**
1. **App name** - Your unique Azure web app name (e.g., `aps-workshop-demo`)
2. **Resource group** - Name for Azure resource group (default: `rg-aps-workshop`)
3. **Region** - Azure region closest to your users (default: `eastus`)
4. **APS Client ID** - From your APS app at https://aps.autodesk.com/
5. **APS Client Secret** - From your APS app (input is hidden)

**Time:** 10-15 minutes for first deployment, 2-3 minutes for updates

**Cost:** ~$13/month (Azure App Service B1 Basic tier)

**Script Structure:**
```bash
STEP 0: Prerequisites check (Azure CLI, .NET SDK)
STEP 1: Azure login (opens browser)
STEP 2: Collect configuration (interactive prompts)
STEP 3: Confirmation (review settings)
STEP 4: Create Azure resources (Resource Group, App Service Plan, Web App)
STEP 5: Configure APS credentials (App Settings)
STEP 6: Enable security (HTTPS, TLS 1.2)
STEP 7: Build and deploy (dotnet publish → ZIP → upload)
STEP 8: Wait for app to start
STEP 9: Test deployment
STEP 10: Success summary with URLs and next steps
```

**Perfect for:**
- 🎓 Workshop participants (step-by-step learning)
- 🚀 Quick demos and POCs
- 👥 Teams new to Azure
- ⚡ Fast iterations during development

**Learn:**
- How to automate Azure deployments with bash scripts
- Azure CLI commands for resource creation
- Best practices for secure credential management
- How to build and package .NET applications for deployment
- Azure App Service configuration
- Educational scripting patterns (colored output, error handling, confirmations)

#### `README.md` (Tools Documentation)

Comprehensive guide for using the deployment tools:
- Prerequisites and installation instructions
- Step-by-step deployment walkthrough
- Example deployment session output
- Troubleshooting common issues
- Cost management tips (scaling, stopping, deleting)
- Security best practices
- Re-deployment instructions
- Monitoring and logging commands
- Pro tips for workshop facilitators

📖 **See:** [`Tools/README.md`](Tools/README.md) for full documentation

---

### 📁 `/Models` - Data Structures

#### `APSConfiguration.cs`
**What it does:**
- Defines the structure for `appsettings.json` configuration
- Strongly-typed access to ClientId, ClientSecret, etc.

**Properties:**
- `ClientId` - Your APS app client ID
- `ClientSecret` - Your APS app secret
- `CallbackUrl` - OAuth callback URL
- `BaseUrl`, `OAuth2AuthorizeUrl`, `OAuth2GetTokenUrl` - APS endpoints

---

#### `TokenResponse.cs`
**What it does:**
- Represents an OAuth token response from APS
- Used when deserializing authentication responses

**Properties:**
- `AccessToken` - The actual token string
- `TokenType` - Usually "Bearer"
- `ExpiresIn` - Token lifetime in seconds

---

#### `Region.cs`
**What it does:**
- Enum for ACC API regions
- Provides dropdown in Swagger UI

**Values:**
- `US` - United States
- `EMEA` - Europe, Middle East, Africa

---

### 📁 Root Files

#### `Program.cs`
**What it does:**
- Application entry point
- Configures Swagger UI with custom branding
- Registers services (dependency injection)
- Sets up HTTP pipeline and middleware
- Configures session management
- Enables static file serving

**Key Configuration:**
- **Swagger UI**: Custom operation IDs, enum conversion (US/EMEA), Autodesk branding
- **Session Management**: 60-minute timeout, secure cookies
- **Middleware Pipeline**: 
  1. Session middleware
  2. Static files (wwwroot)
  3. Custom authentication middleware (SSO)
  4. Swagger UI
  5. Controllers
- **Service Registration**: All services, controllers, and HTTP clients
- **Custom Swagger Banner**: Displays user info, help link, logout button

**Learn:**
- How to configure ASP.NET Core middleware pipeline
- How to customize Swagger UI with HTML/CSS/JavaScript
- How to set up session management
- Dependency injection best practices

---

#### `appsettings.json`
**What it does:**
- Stores application configuration
- **⚠️ This is where you add your APS credentials!**

**Structure:**
```json
{
  "Logging": { ... },
  "APS": {
    "ClientId": "YOUR_CLIENT_ID",
    "ClientSecret": "YOUR_CLIENT_SECRET",
    "CallbackUrl": "http://localhost:8080/auth/callback",
    ...
  }
}
```

---

#### `aps-starter-pack-swagger-ui.csproj`
**What it does:**
- .NET project file
- Defines target framework (.NET 9)
- Lists NuGet package dependencies

**Key Package:**
- `Swashbuckle.AspNetCore` **7.0.0** - Generates beautiful Swagger UI (see detailed explanation above!)

**Note:** Session management is built into .NET 9, so no additional package is needed! We also don't need Microsoft.AspNetCore.OpenApi because Swashbuckle 7.0.0 is self-contained.

**Why This Version:**
- ✅ **Stability**: Version 7.0.0 is proven and reliable
- ✅ **Compatibility**: Fully tested with .NET 9
- ✅ **Self-Contained**: No dependency on Microsoft.AspNetCore.OpenApi
- ✅ **No Breaking Changes**: Versions 8.x and 9.x have rendering issues
- ✅ **All Features Work**: Enums, custom IDs, branding all functional

**Keeping Updated:**
Run this command to check for newer versions:
```bash
dotnet list package --outdated
```

---

## 🔍 Key Concepts for Learning

### 1. Controllers vs Services Pattern

**Why separate them?**
- **Separation of Concerns**: Controllers handle HTTP, Services handle business logic
- **Reusability**: Multiple controllers can use the same service
- **Testability**: Services can be tested independently
- **Maintainability**: Changes to API logic don't affect HTTP handling

**Example Flow:**
```
1. User calls: GET /hubs?token=abc123
2. HubsController receives the request
3. HubsController validates the token parameter
4. HubsController calls HubsService.GetHubsAsync(token)
5. HubsService makes HTTP call to APS API
6. HubsService returns data to controller
7. HubsController returns HTTP 200 with data
```

---

### 2. Authentication Scopes

Different endpoints require different scopes:

| Scope           | Used For                            |
|-----------------|-------------------------------------|
| `data:read`     | Read hubs, projects, folders, files |
| `data:write`    | Modify files and folders            |
| `data:create`   | Create folders and upload files     |
| `data:search`   | Search within folders               |
| `account:read`  | Access companies, users, roles      |

**Get all scopes with:** `GET /auth/token` (includes all by default)

---

### 3. Autodesk SSO (Single Sign-On) Authentication

This workshop requires users to log in with their Autodesk account before accessing Swagger UI.

**How it works:**

```
1. User visits http://localhost:8080
   ↓
2. APSAuthenticationMiddleware checks for session
   ↓
3. No session found → Redirect to /login.html
   ↓
4. User clicks "Login with Autodesk SSO"
   ↓
5. Redirected to Autodesk OAuth page
   ↓
6. User enters Autodesk credentials
   ↓
7. Autodesk redirects back to /auth/callback with code
   ↓
8. App exchanges code for access token
   ↓
9. App fetches user profile (name, email, picture)
   ↓
10. User info & token stored in server session
   ↓
11. Redirected to Swagger UI with full access
```

**Session Management:**
- **Timeout**: 60 minutes of inactivity
- **Storage**: Server-side only (secure)
- **Contents**: Access token, refresh token, user name, email, picture
- **Security**: HttpOnly cookies, no client-side exposure

**Benefits:**
- ✅ **Secure**: Users must authenticate with Autodesk
- ✅ **Personalized**: Shows user's name and email
- ✅ **Seamless**: Token managed automatically
- ✅ **Real-world**: Same pattern used in production apps

**Learn:** How to implement enterprise-grade SSO authentication in .NET

---

### 4. API Versions

This workshop uses multiple APS APIs:

| API | Version | Base URL | Used For |
|-----------------------------|----|---------------------------|--------------------|
| Data Management             | v1 | `/data/v1`                | Folders, files     |
| Project                     | v1 | `/project/v1`             | Hubs, projects     |
| ACC Admin (HQ)              | v1 | `/hq/v1`                  | Companies, users   |
| ACC Admin                   | v2 | `/hq/v2`                  | Industry roles     |
| ACC Admin (Construction)    | v1 | `/construction/admin/v1`  | Project users      |
| BIM 360 Docs                | v1 | `/bim360/docs/v1`         | Permissions        |

---

### 5. IDs and URNs

| Type               | Format          | Example                                       | Used In API     |
|--------------------|-----------------|-----------------------------------------------|-----------------|
| Hub ID             | `b.{guid}`      | `b.3def0ac0-6276...`                          | Data Management |
| Account ID         | `{guid}`        | `3def0ac0-6276...`                            | ACC API         |
| Project ID         | `b.{guid}`      | `b.e66ece9f-5035...`                          | Data Management |
| Project ID (ACC)   | `{guid}`        | `e66ece9f-5035...`                            | ACC API         |
| Folder URN         | `urn:adsk...`   | `urn:adsk.wipprod:fs.folder:co.GdRoPl0LQxOl`  | Folders         |

**Note:** The app auto-removes "b." prefix where needed!

---

## 🚀 Workshop Exercises

### Exercise 1: Get Your First Token
1. Open Swagger UI
2. Navigate to `1. Authentication`
3. Execute `GET /auth/token`
4. Observe the response structure
5. Copy the `access_token`

**Learning Goal:** Understand OAuth 2.0 client credentials flow

---

### Exercise 2: Explore Your Data
1. Use your token to call `GET /hubs`
2. Pick a hub and call `GET /projects`
3. Pick a project and call `GET /projects/{projectId}/topFolders`
4. Use a folder URN to call `GET /folders/contents`

**Learning Goal:** Understand the data hierarchy (Hub → Project → Folder → File)

---

### Exercise 3: Create a Folder
1. Get a folder URN from `GET /projects/{projectId}/topFolders`
2. Call `POST /folders` with:
   - Your token
   - Project ID
   - Parent folder URN
   - Folder name: "Workshop Test Folder"
3. Verify it was created in ACC/BIM 360 web interface

**Learning Goal:** Understand how to make POST requests with JSON body

---

### Exercise 4: Search for Content
1. Get a new token (to ensure you have `data:search` scope)
2. Call `GET /folders/search` with:
   - Token
   - Project ID
   - Folder URN
   - Search filter: try "Folder" or specific file name
3. Observe the recursive search results

**Learning Goal:** Understand API filtering and search patterns

---

### Exercise 5: Manage Users
1. Call `GET /users` to see all account users
2. Call `GET /projects/{projectId}/users` to see project-specific users
3. Compare the results
4. Call `GET /roles/projects/{projectId}` to see available roles

**Learning Goal:** Understand account vs. project-level user management

---

## 💡 Common Issues and Solutions

### Issue: "Unauthorized" (401)
**Causes:**
- Invalid ClientId or ClientSecret in `appsettings.json`
- Token expired (tokens expire after ~3600 seconds)

**Solution:**
- Double-check your credentials
- Get a fresh token

---

### Issue: "Forbidden" (403)
**Causes:**
- Missing API enablement in APS Portal
- Missing scope in token
- User doesn't have access to resource

**Solution:**
- Enable "Data Management API", "BIM 360 API", and "ACC API" in APS Portal
- Get a new token with all scopes
- Use a 3-legged token if resource requires user access

---

### Issue: "Bad Request" (400) on Projects/Folders
**Causes:**
- Missing "b." prefix on hubId or projectId
- Incorrect folderUrn format

**Solution:**
- Ensure hubId looks like: `b.3def0ac0-6276...`
- Copy folderUrn exactly from the API response

---

### Issue: "Not Found" (404)
**Causes:**
- Incorrect ID format
- Resource doesn't exist
- Wrong API endpoint

**Solution:**
- Verify IDs are correct
- Check if endpoint is available for your account type
- Some ACC API endpoints require specific account setup

---

### Issue: Search Returns Empty Results
**Causes:**
- Search filter doesn't match any items
- Case-sensitive search
- `filter[attributes.displayName]` might not be the right property

**Solution:**
- Try exact folder/file names
- Use `GET /folders/contents` to see what actually exists
- Experiment with different search terms

---

## 📖 Additional Resources

### Official Documentation
- [APS Documentation](https://aps.autodesk.com/en/docs) - Complete API reference
- [Data Management API](https://aps.autodesk.com/en/docs/data/v2) - Hubs, projects, folders
- [ACC API](https://aps.autodesk.com/en/docs/acc/v1) - Companies, users, roles
- [OAuth Guide](https://aps.autodesk.com/en/docs/oauth/v2) - Authentication details

### Community
- [APS Portal](https://aps.autodesk.com/) - Manage apps and credentials
- [APS Blog](https://aps.autodesk.com/blog) - Tutorials and updates
- [APS Community Forums](https://forums.autodesk.com/t5/forge/ct-p/5226) - Ask questions

### Learning Resources
- [Getting Started Guide](https://aps.autodesk.com/developer/start-now/overview) - Beginner tutorials
- [Code Samples](https://github.com/autodesk-platform-services) - Official sample code
- [Postman Collections](https://www.postman.com/autodesk-forge) - Test APIs in Postman

---

## 🤝 Workshop Facilitation Tips

When presenting this workshop to your team:

### 1. **Start with the "Why"**
- Explain why APIs matter
- Show real business use cases
- Demo the Swagger UI first

### 2. **Live Coding Session**
- Open Visual Studio Code
- Walk through one service file step-by-step
- Explain HTTP requests, headers, authentication

### 3. **Hands-On Practice**
- Let attendees run the app on their machines
- Have them call endpoints with their own data
- Encourage experimentation

### 4. **Build Something Together**
- Create a folder as a group
- Search for it
- Set permissions
- Show the result in ACC web interface

### 5. **Code Review**
- Pick 2-3 service files
- Explain line-by-line what each does
- Discuss error handling patterns
- Show how to add new endpoints

### 6. **Next Steps**
- Encourage attendees to extend the app
- Suggest adding new endpoints
- Challenge them to build a simple UI
- Share additional resources

---

## 🎓 Learning Path

### Beginner Level
1. ✅ Run the app and explore Swagger UI
2. ✅ Get a token and call `GET /hubs`
3. ✅ Understand request/response structure
4. ✅ Read through `AuthenticationService.cs`

### Intermediate Level
1. ✅ Create a folder via the API
2. ✅ Understand Controllers vs Services pattern
3. ✅ Add a new endpoint (e.g., GET folder parent)
4. ✅ Handle errors gracefully

### Advanced Level
1. ✅ Implement file upload
2. ✅ Add pagination support
3. ✅ Build a simple frontend
4. ✅ Implement webhooks

---

## ☁️ Azure Deployment Guide

Ready to deploy your workshop app to Azure? You have **two options**:

### 🚀 **Option A: Automated Deployment Script (Recommended for Workshops)**

We've created a comprehensive, interactive deployment script that automates the entire process!

**Perfect for:**
- ✅ Workshop participants learning Azure deployment
- ✅ Quick demos and POCs
- ✅ First-time Azure users
- ✅ Step-by-step guided deployment

**What it does:**
- Creates all Azure resources automatically
- Configures security settings (HTTPS, TLS, security headers)
- Builds and deploys your application
- Tests the deployment
- Provides clear, educational output at each step

**How to use:**

```bash
# Make the script executable (first time only)
chmod +x Tools/deploy.sh

# Run the deployment script
./Tools/deploy.sh
```

The script will guide you through:
1. Azure login
2. Configuration (app name, region, APS credentials)
3. Resource creation
4. Deployment
5. Testing

**Time required:** 10-15 minutes  
**Cost:** ~$13/month (B1 Basic tier)

📖 **Full documentation:** See [`Tools/README.md`](Tools/README.md) for detailed instructions, troubleshooting, and pro tips.

---

### 📝 **Option B: Manual Deployment (Step-by-Step)**

If you prefer to understand each step in detail or need more control, follow the manual deployment guide below.

**Perfect for:**
- ✅ Learning Azure CLI commands
- ✅ Custom configurations
- ✅ CI/CD pipeline setup
- ✅ Advanced users

---

### Prerequisites

1. ✅ **Azure Account** - [Sign up for free](https://azure.microsoft.com/free/)
2. ✅ **Azure CLI** (optional) - [Install Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli)
3. ✅ **APS App configured** - Your app must be set up in the [APS portal](https://aps.autodesk.com/)

---

### Step 1: Create Azure App Service

#### Option A: Using Azure Portal (Recommended for beginners)

1. Go to [Azure Portal](https://portal.azure.com)
2. Click **"Create a resource"**
3. Search for **"Web App"** and click **Create**
4. Fill in the details:
   - **Resource Group:** Create new or use existing
   - **Name:** `aps-workshop-demo` (must be globally unique)
   - **Publish:** Code
   - **Runtime stack:** .NET 9
   - **Operating System:** Linux (cheaper) or Windows
   - **Region:** Choose closest to your users
   - **App Service Plan:** Choose your pricing tier (B1 Basic for workshops)
5. Click **Review + Create**, then **Create**

#### Option B: Using Azure CLI

```bash
# Login to Azure
az login

# Create resource group
az group create --name aps-workshop-rg --location eastus

# Create App Service Plan
az appservice plan create \
  --name aps-workshop-plan \
  --resource-group aps-workshop-rg \
  --sku B1 \
  --is-linux

# Create Web App
az webapp create \
  --name aps-workshop-demo \
  --resource-group aps-workshop-rg \
  --plan aps-workshop-plan \
  --runtime "DOTNETCORE:9.0"
```

---

### Step 2: Configure APS Credentials in Azure

**Important:** Never commit credentials to source control! Use Azure App Settings instead.

#### Using Azure Portal:

1. Go to your **App Service** in Azure Portal
2. Navigate to **Configuration** → **Application settings**
3. Click **+ New application setting** and add these:

| Name | Value | Example |
|------|-------|---------|
| `APS__ClientId` | Your production APS Client ID | `7UySSyPFKh38...` |
| `APS__ClientSecret` | Your production APS Secret | `xQVv3fIX9ycL...` |
| `APS__CallbackUrl` | Your Azure callback URL | `https://aps-workshop-demo.azurewebsites.net/auth/callback` |

4. Click **Save** (app will restart)

#### Using Azure CLI:

```bash
az webapp config appsettings set \
  --name aps-workshop-demo \
  --resource-group aps-workshop-rg \
  --settings \
    APS__ClientId="YOUR_CLIENT_ID" \
    APS__ClientSecret="YOUR_CLIENT_SECRET" \
    APS__CallbackUrl="https://aps-workshop-demo.azurewebsites.net/auth/callback"
```

**Note:** The double underscore `__` in Azure translates to `:` in JSON configuration. This is how Azure overrides `appsettings.json` values.

---

### Step 3: Update APS App Callback URL

1. Go to [APS Portal](https://aps.autodesk.com/)
2. Select your app
3. Navigate to **General Settings**
4. Add your Azure callback URL to **Callback URL** list:
   ```
   https://aps-workshop-demo.azurewebsites.net/auth/callback
   ```
5. Click **Save**

**Important:** The callback URL must match EXACTLY (including https, no trailing slash).

---

### Step 4: Deploy the Application

#### Option A: Deploy from Visual Studio Code

1. Install **Azure App Service** extension for VS Code
2. Right-click on your project folder
3. Select **Deploy to Web App...**
4. Choose your Azure subscription and App Service
5. Confirm deployment

#### Option B: Deploy from Visual Studio 2022

1. Right-click on your project in Solution Explorer
2. Select **Publish...**
3. Choose **Azure** → **Azure App Service (Linux or Windows)**
4. Sign in to your Azure account
5. Select your App Service
6. Click **Publish**

#### Option C: Deploy using Azure CLI

```bash
# Build and publish the app
dotnet publish -c Release -o ./publish

# Create a ZIP of the published files
cd publish
zip -r ../app.zip *
cd ..

# Deploy to Azure
az webapp deployment source config-zip \
  --name aps-workshop-demo \
  --resource-group aps-workshop-rg \
  --src app.zip
```

#### Option D: Deploy from GitHub (CI/CD)

1. In Azure Portal, go to your App Service
2. Navigate to **Deployment Center**
3. Select **GitHub** as source
4. Authenticate and select your repository and branch
5. Azure will automatically create a GitHub Actions workflow
6. Push code to GitHub → Automatic deployment!

---

### Step 5: Enable HTTPS and Security

Azure automatically provides HTTPS, but let's ensure it's enforced:

1. In Azure Portal, go to your App Service
2. Navigate to **TLS/SSL settings**
3. Set **HTTPS Only** to **On**
4. Set **Minimum TLS Version** to **1.2**

**Our Security Headers Middleware will automatically:**
- ✅ Enable HSTS (HTTP Strict Transport Security)
- ✅ Add Content-Security-Policy
- ✅ Protect against XSS, clickjacking, and MIME sniffing
- ✅ Implement all 6 essential security headers

---

### Step 6: Configure Session State (Important!)

ASP.NET Core sessions need special configuration for Azure:

#### For Production Scale (Recommended):

Use Azure Redis Cache for distributed sessions:

```bash
# Create Redis Cache
az redis create \
  --name aps-workshop-cache \
  --resource-group aps-workshop-rg \
  --location eastus \
  --sku Basic \
  --vm-size c0
```

Then add to `Program.cs` (or use App Settings):
```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Configuration["Redis:ConnectionString"];
});
```

#### For Small Workshops (Current Setup):

Our current in-memory cache works for single-instance deployments. For multi-instance scaling, use Redis.

---

### Step 7: Monitor and Troubleshoot

#### View Logs:

**Azure Portal:**
1. Go to **App Service** → **Log stream**
2. See real-time logs

**Azure CLI:**
```bash
az webapp log tail \
  --name aps-workshop-demo \
  --resource-group aps-workshop-rg
```

#### Common Issues:

**Issue: "502 Bad Gateway"**
- Solution: Check if .NET 9 runtime is installed. Verify in Configuration → General settings.

**Issue: "403 Forbidden" from APS API**
- Solution: Verify APS credentials in App Settings (check for typos).
- Verify callback URL matches exactly in APS portal.

**Issue: Session not persisting**
- Solution: Use Redis Cache for distributed sessions (see Step 6).

**Issue: Security headers not working**
- Solution: Verify `app.UseSecurityHeaders()` is called in `Program.cs` before `app.Run()`.

---

### Step 8: Test Your Deployment

1. Open your Azure URL: `https://aps-workshop-demo.azurewebsites.net`
2. You should see the **login page**
3. Click **"Login with Autodesk SSO"**
4. Authenticate with your Autodesk account
5. You should be redirected to **Swagger UI**
6. Test an endpoint (e.g., **GET /hubs**)

**Security Verification:**
- Open browser DevTools (F12) → Network tab
- Check response headers for:
  - `strict-transport-security`
  - `content-security-policy`
  - `x-frame-options`
  - `x-content-type-options`

---

### 💡 Pro Tips for Workshop Demos

#### Quickly Switch Between APS Apps

Since your credentials are in Azure App Settings (not code), you can easily switch between different APS apps for different demos:

1. Go to **App Service** → **Configuration**
2. Update `APS__ClientId` and `APS__ClientSecret`
3. Click **Save** (app restarts)
4. New credentials active immediately!

This is perfect for:
- ✅ Different workshop scenarios
- ✅ Testing with different APS scopes
- ✅ Switching between dev/staging/production APS apps

#### Use Deployment Slots

For advanced users:
- Create **staging slot** for testing
- Swap to production when ready
- Zero-downtime deployments!

```bash
az webapp deployment slot create \
  --name aps-workshop-demo \
  --resource-group aps-workshop-rg \
  --slot staging
```

---

### 🔒 Security Checklist for Production

Before going live with a production deployment:

- ✅ **HTTPS Only** - Enabled in Azure TLS/SSL settings
- ✅ **Security Headers** - `SecurityHeadersMiddleware.cs` active
- ✅ **Credentials in App Settings** - Never in `appsettings.json` for production
- ✅ **Authentication Required** - `APSAuthenticationMiddleware.cs` active
- ✅ **Logging Enabled** - Application Insights configured (optional but recommended)
- ✅ **Minimum TLS 1.2** - Enabled in Azure
- ✅ **CORS Configured** - If needed for your frontend
- ✅ **API Rate Limiting** - Consider adding if exposed publicly

---

### 📊 Optional: Add Application Insights

Monitor your workshop app's performance and usage:

1. In Azure Portal, create an **Application Insights** resource
2. Copy the **Instrumentation Key**
3. Add to App Settings:
   - `APPLICATIONINSIGHTS_CONNECTION_STRING` = Your connection string
4. Add NuGet package:
   ```bash
   dotnet add package Microsoft.ApplicationInsights.AspNetCore
   ```
5. Add to `Program.cs`:
   ```csharp
   builder.Services.AddApplicationInsightsTelemetry();
   ```

Now you can see:
- 📈 Request metrics
- 🐛 Exception tracking
- 📊 User analytics
- ⚡ Performance insights

---

## 📝 License

**Copyright (c) 2025 Autodesk, Inc.**

This workshop application was **created by Autodesk** as an educational resource for the APS developer community. 

While this is a workshop/demo/POC (Proof of Concept), the intellectual property belongs to **Autodesk, Inc.**


**What this means for you:**

✅ **Free to Use** - Use this Autodesk workshop for learning and development
✅ **Modify for Learning** - Adapt the code for educational purposes
✅ **Share with Attribution** - Redistribute with proper Autodesk copyright notice
✅ **Build Upon** - Create your own APS projects based on this foundation
✅ **Educational Purpose** - Perfect for team training and skill development

**Key Points:**

- 🏢 **Autodesk Property**: Created and owned by Autodesk, Inc.
- 📖 **Open Source**: Released under MIT License for community benefit
- 🎓 **Educational Focus**: Designed specifically for APS workshop training
- 🤝 **Community Driven**: Autodesk supports the developer community
- ⚠️ **As-Is**: No warranty, provided for educational purposes

**Important:** While this software is open source under MIT License, using **Autodesk Platform Services (APS) APIs** requires:
- Valid APS credentials (free to obtain at [aps.autodesk.com](https://aps.autodesk.com/))
- Compliance with [APS Terms of Service](https://aps.autodesk.com/terms)
- Adherence to APS rate limits and quotas

**Third-Party Components:**

All dependencies are also open source:
- `Swashbuckle.AspNetCore` (v7.0.0) - MIT License
- `Microsoft.AspNetCore.OpenApi` (v9.0.9) - MIT License
- `.NET 9.0 Runtime` - MIT License

See the [LICENSE](./LICENSE) file for complete details.

---

### 🔓 What You Can Do:

✅ Use this workshop for internal team training
✅ Modify the code for your specific needs
✅ Create your own APS applications based on this
✅ Share with colleagues and the developer community
✅ Contribute improvements back to the project
✅ Use as a foundation for commercial projects

### ⚠️ What to Know:

- This is an **Autodesk-created workshop application** for educational purposes
- While it's a demo/POC, the **copyright belongs to Autodesk, Inc.**
- You need your own **APS credentials** (free at [aps.autodesk.com](https://aps.autodesk.com/))
- **APS API usage** is subject to Autodesk's terms (separate from this code license)
- Software provided **"as-is"** without warranty
- **Maintain Autodesk copyright notice** when sharing or modifying

### 📧 Questions About Licensing?

- **Software License**: See [LICENSE](./LICENSE) file
- **APS API Terms**: Visit [aps.autodesk.com/terms](https://aps.autodesk.com/terms)
- **APS Developer Support**: [aps.autodesk.com/support](https://aps.autodesk.com/support)

---

**Happy Coding!** 🎉

If you have questions:
1. Check the [APS Documentation](https://aps.autodesk.com/en/docs)
2. Visit [APS Community Forums](https://forums.autodesk.com/t5/forge/ct-p/5226)
3. Review the code comments in this project - they're detailed!

**Remember:** The best way to learn is by doing. Don't be afraid to experiment! 💪
