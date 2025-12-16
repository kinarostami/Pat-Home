using DataLayer;
using Eshop.Infrastructure;
using InventoryManagement.Configuration;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

DataBootstrapper.ConfigData(builder.Services);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
InventoryManagementBootstrapper.Init(builder.Services, connectionString);
DependencyRegister.Register(builder.Services);
builder.Services.AddHttpContextAccessor();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
