using WebApp.Components;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Services.Repositories;
using WebApp.Services.External;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<WebAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebAppContext") ?? throw new InvalidOperationException("Connection string 'WebAppContext' not found.")));

builder.Services.AddHttpClient<WebScraper>();

builder.Services.AddScoped<JobListingRepository>();
builder.Services.AddScoped<JobSourceRepository>();
builder.Services.AddScoped<JobRepository>();

builder.Services.AddScoped<JobService>();

builder.Services.AddSingleton<DropdownService>();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
