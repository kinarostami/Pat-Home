using DataLayer;
using Eshop.Infrastructure;
using InventoryManagement.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using WebEssentials.AspNetCore.Pwa;

var builder = WebApplication.CreateBuilder(args);

//var configuration = builder.Configuration;
//var environment = builder.Environment;

// Add services to the container.
builder.Services.AddRazorPages();

DataBootstrapper.ConfigData(builder.Services);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
InventoryManagementBootstrapper.Init(builder.Services, connectionString);

DependencyRegister.Register(builder.Services);

#region LoginOption
//string keysFolder = Path.Combine(environment.ContentRootPath, "Keys");
//builder.Services.AddDataProtection()
//    .SetApplicationName("Eshop")
//    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
//    .SetDefaultKeyLifetime(TimeSpan.FromDays(30));

#endregion

//builder.Services.AddProgressiveWebApp(new PwaOptions()
//{
//    AllowHttp = false,
//    Strategy = ServiceWorkerStrategy.Minimal,
//    CacheId = "Worker 1.0",
//    OfflineRoute = "/offlinePage.html",
//});

#region Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(option =>
{
    option.LoginPath = "/Auth/Login";
    option.LogoutPath = "/Auth/Logout";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(43200);

});
#endregion
builder.Services.AddHttpContextAccessor();
builder.Services.AddCookieManager();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.Use(async (context, next) =>
//{
//    var token = context.Request.Cookies["Eshop"]?.ToString();
//    if (string.IsNullOrWhiteSpace(token) == false)
//    {
//        context.Request.Headers.Append("Authorization", $"Bearer {token}");
//    }
//    await next();
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
