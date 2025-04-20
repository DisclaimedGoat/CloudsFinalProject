using FinalProject.Components;
using FinalProject.Models;
using FinalProject.Services;
using AppContext = FinalProject.Services.AppContext;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

var config = builder.Configuration;

// Register services
services.AddScoped<AppContext>()
	.AddScoped<UserService>()
	.AddRazorComponents()
	.AddInteractiveServerComponents();

services.Configure<DatabaseConfig>(config.GetSection("Database"));

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var userService = scope.ServiceProvider.GetRequiredService<UserService>();
	var dbContext = scope.ServiceProvider.GetRequiredService<AppContext>();
	await userService.InitializeAsync();
	
	dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	//app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseAntiforgery();
app.UseStaticFiles();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();