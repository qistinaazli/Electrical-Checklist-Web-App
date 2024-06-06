using FirstWebApp.Components;
using FirstWebApp.Service;
using MudBlazor.Services;
using Blazored.Toast;
using Microsoft.EntityFrameworkCore;
using FirstWebApp.Components.Pages;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using Append.Blazor.Printing;

var builder = WebApplication.CreateBuilder(args);

// Load the configuration file
builder.Configuration.AddJsonFile("appsettings.json");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<IDbHelper, DbHelper>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = "domain.com", // NOTE: ENTER DOMAIN HERE
        ValidateIssuer = true,
        ValidIssuer = "domain.com", // NOTE: ENTER DOMAIN HERE
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("524C1F28-6115-4E16-9B6A-3FBF185308F2")) // NOTE: THIS SHOULD BE A SECRET KEY NOT TO BE SHARED; A GUID IS RECOMMENDED, DO NOT REUSE THIS GUID
    };
});

builder.Services.AddMudServices();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<HttpClient>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<IPrintingService, PrintingService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Register the IDbConnection service with NpgsqlConnection
builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));

builder.Services.AddAuthorizationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure the DbContext with the Npgsql connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
