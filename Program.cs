using APSStarterPackSwaggerUI.Services;
using APSStarterPackSwaggerUI.Models;
using APSStarterPackSwaggerUI.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure enum serialization to use string names instead of integers
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

// Configure Session (required for SSO authentication)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Change to Always in production with HTTPS
});

// Configure Swagger with better documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APS Starter Pack Workshop",
        Version = "v1",
        Description = "Simple API collection to demonstrate Autodesk Platform Services (APS) Data Management and ACC APIs. Perfect for learning how to interact with APS!"
    });
    
    // Use method names as operation IDs for better Swagger display
    c.CustomOperationIds(apiDesc =>
    {
        var actionDescriptor = apiDesc.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        return actionDescriptor?.MethodInfo?.Name;
    });
    
    // Configure enums to use string values in Swagger UI
    c.UseInlineDefinitionsForEnums();
    
    // Include XML comments for better documentation in Swagger UI
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
    // Note: We intentionally don't add Bearer authentication here because our endpoints
    // use query parameters for tokens (simpler for workshop/learning purposes)
});

// Register APS configuration
builder.Services.Configure<APSConfiguration>(
    builder.Configuration.GetSection("APS"));

// Register Authorized Users configuration (email whitelist)
builder.Services.Configure<AuthorizedUsersConfiguration>(
    builder.Configuration.GetSection("AuthorizedUsers"));

// Register services
builder.Services.AddHttpClient();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<UserInfoService>();
builder.Services.AddScoped<HubsService>();
builder.Services.AddScoped<ProjectsService>();
builder.Services.AddScoped<CompaniesService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<AccountUsersService>();
builder.Services.AddScoped<ProjectUsersService>();
builder.Services.AddScoped<FoldersService>();
builder.Services.AddScoped<PermissionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline

// Enable session BEFORE authentication middleware
app.UseSession();

// Add security headers (HSTS, CSP, X-Frame-Options, etc.)
// Important for production deployments, especially Azure
app.UseSecurityHeaders();

// Enable static files (for help.html, login.html, access-denied.html)
app.UseStaticFiles();

// Add custom authentication middleware (protects all routes except login)
app.UseMiddleware<APSAuthenticationMiddleware>();

// Add email whitelist middleware (checks if user's email is authorized)
// This runs AFTER authentication, so user is already logged in with Autodesk SSO
// If EnableWhitelist=true in config, only allowed emails can access the app
app.UseEmailWhitelist();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APS Starter Pack Workshop APIs Collection v1");
    c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    
    // Add custom HTML at the top of Swagger UI with link to help page
    // Clean, modern design using Autodesk brand colors (white, black, yellow)
    c.HeadContent = @"
        <style>
            .help-banner {
                position: relative;
                background: rgba(255, 255, 255, 0.95);
                backdrop-filter: blur(10px);
                -webkit-backdrop-filter: blur(10px);
                border: 2px solid #FFD100;
                color: #000000;
                padding: 30px 320px 30px 30px;
                text-align: center;
                margin-bottom: 20px;
                border-radius: 16px;
                box-shadow: 0 8px 32px rgba(0, 0, 0, 0.08), 0 0 0 1px rgba(255, 209, 0, 0.1);
            }
            @media (max-width: 768px) {
                .help-banner {
                    padding: 70px 30px 30px 30px;
                }
                .user-info {
                    position: static;
                    margin: 0 auto 15px auto;
                }
            }
            .help-banner h2 {
                margin: 0 0 12px 0;
                font-size: 1.7em;
                color: #000000;
                font-weight: 700;
                letter-spacing: -0.5px;
            }
            .help-banner p {
                margin: 0 0 20px 0;
                color: #333333;
                font-size: 1.05em;
                line-height: 1.6;
            }
            .help-link {
                display: inline-block;
                background: #FFD100;
                color: #000000;
                padding: 14px 36px;
                border-radius: 30px;
                text-decoration: none;
                font-weight: 700;
                transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                margin: 5px;
                border: 2px solid #FFD100;
                box-shadow: 0 4px 15px rgba(255, 209, 0, 0.3);
                font-size: 1.05em;
                letter-spacing: 0.3px;
            }
            .help-link:hover {
                transform: translateY(-2px) scale(1.02);
                box-shadow: 0 8px 25px rgba(255, 209, 0, 0.5);
                background: #FFDB33;
                border-color: #FFDB33;
            }
        </style>
        <style>
            .logout-link {
                display: inline-block;
                background: transparent;
                color: #DC2626;
                padding: 12px 30px;
                border-radius: 30px;
                text-decoration: none;
                font-weight: 700;
                transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
                margin: 5px;
                border: 2px solid #DC2626;
                font-size: 0.95em;
                letter-spacing: 0.3px;
            }
            .logout-link:hover {
                transform: translateY(-2px) scale(1.02);
                background: #DC2626;
                color: white;
            }
            .auth-status {
                display: inline-block;
                background: #F0FDF4;
                color: #166534;
                padding: 8px 16px;
                border-radius: 20px;
                font-size: 0.9em;
                font-weight: 600;
                margin-bottom: 10px;
                border: 1px solid #BBF7D0;
            }
            /* Hide Schemas section - too technical for workshop users */
            .swagger-ui .models,
            .swagger-ui section.models {
                display: none !important;
            }
            .user-info {
                position: absolute;
                top: 20px;
                right: 20px;
                display: flex;
                align-items: center;
                gap: 10px;
                padding: 8px 16px;
                background: rgba(255, 255, 255, 0.98);
                border-radius: 25px;
                border: 1.5px solid #E5E5E5;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
                max-width: 280px;
            }
            .user-avatar {
                width: 36px;
                height: 36px;
                border-radius: 50%;
                border: 2px solid #FFD100;
                background: #F5F5F5;
                display: flex;
                align-items: center;
                justify-content: center;
                font-size: 1.1em;
                flex-shrink: 0;
            }
            .user-details {
                text-align: left;
                overflow: hidden;
                flex: 1;
            }
            .user-name {
                font-weight: 700;
                color: #000000;
                font-size: 0.95em;
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }
            .user-email {
                font-size: 0.8em;
                color: #666666;
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }
        </style>
        <script>
            // Add help banner when Swagger UI loads
            window.addEventListener('DOMContentLoaded', (event) => {
                setTimeout(async () => {
                    const infoContainer = document.querySelector('.information-container');
                    if (infoContainer) {
                        // Fetch current user info
                        let userHtml = '';
                        try {
                            const userResponse = await fetch('/auth/current-user');
                            if (userResponse.ok) {
                                const user = await userResponse.json();
                                const initials = user.name ? user.name.split(' ').map(n => n[0]).join('').toUpperCase().substring(0, 2) : '👤';
                                userHtml = `
                                    <div class='user-info'>
                                        <div class='user-avatar'>${user.picture ? `<img src='${user.picture}' style='width:100%;height:100%;border-radius:50%;object-fit:cover;'/>` : initials}</div>
                                        <div class='user-details'>
                                            <div class='user-name'>${user.name || 'User'}</div>
                                            <div class='user-email'>${user.email || ''}</div>
                                        </div>
                                    </div>
                                `;
                            }
                        } catch (e) {
                            console.log('Could not fetch user info');
                        }

                        const helpBanner = document.createElement('div');
                        helpBanner.className = 'help-banner';
                        helpBanner.innerHTML = `
                            <div class='auth-status'>✅ Authenticated with Autodesk SSO</div>
                            ${userHtml}
                            <h2>🎓 APS Workshop</h2>
                            <p>New to APIs? Don't worry! We've created a step-by-step guide just for you.</p>
                            <a href='/help.html' target='_blank' class='help-link'>📖 User Manual</a>
                            <a href='/about.html' target='_blank' class='help-link'>👨‍💻 Developer Guide</a>
                            <a href='/auth/logout' class='logout-link'>🚪 Logout</a>
                        `;
                        infoContainer.parentNode.insertBefore(helpBanner, infoContainer);
                    }
                }, 500);
            });
        </script>
    ";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("🚀 APS Starter Pack Workshop is running!");
Console.WriteLine("🔐 SSO Authentication is ENABLED - Users must login with Autodesk");
Console.WriteLine("📖 Open your browser at: http://localhost:8080");
Console.WriteLine("💡 Update appsettings.json with your APS Client ID and Secret");

app.Run();

