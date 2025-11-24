using DiarioMagna;
using DiarioMagna.Authorization;
using DiarioMagna.Components;
using DiarioMagna.Components.Account;
using DiarioMagna.Data;
using DiarioMagna.Services.Email;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<LayoutHeaderState>();

// Authentication / Identity State
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// Email Services
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IIdentityEmailService, IdentityEmailService>();
builder.Services.AddTransient<IEmailSender<ApplicationUser>, IdentityEmailSender>();
builder.Services.AddTransient<INoticieroEmailService, NoticieroEmailService>();

// Authentication cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddAuthorization();

// Database
// Si hay DefaultConnection -> SQL Server (tu PC).
// Si no hay -> SQLite (Render u otros entornos sin SQL Server).
// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment() && !string.IsNullOrWhiteSpace(connectionString))
    {
        // En tu PC (Development) usas SQL Server
        options.UseSqlServer(connectionString);
    }
    else
    {
        // En Render (Production dentro de Docker) usas SQLite
        options.UseSqlite("Data Source=app.db");
    }
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity Core
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

var app = builder.Build();

// Crear BD (SQL Server o SQLite) y sembrar roles al arrancar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Asegura que la BD exista (y esquema básico)
    var db = services.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();

    // Role seeding
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames =
    {
        AppRoles.Secretaria,
        AppRoles.RelacionesPublicas,
        AppRoles.EncargadoSistema,
        AppRoles.Administrador
    };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Identity Endpoints (login, register, etc)
app.MapAdditionalIdentityEndpoints();

// Custom Logout
app.MapPost("/app/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/");
});

app.Run();
