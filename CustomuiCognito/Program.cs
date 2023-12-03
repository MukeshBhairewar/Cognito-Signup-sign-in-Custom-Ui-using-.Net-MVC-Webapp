using Amazon;
using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CustomuiCognito;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure AWS options
builder.Services.AddAWSService<IAmazonCognitoIdentityProvider>();


// Retrieve AWS and Cognito configuration from app settings
var awsRegion = builder.Configuration["Cognito:Region"];
AWSConfigs.AWSRegion = awsRegion;

// Add CognitoService
builder.Services.AddScoped<ICognitoService, CognitoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

