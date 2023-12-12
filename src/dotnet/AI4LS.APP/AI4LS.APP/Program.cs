using AI4LS.APP.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);


Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["syncfusionkey"]);


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<CountriesService>();
builder.Services.AddSingleton<LucasData2018Service>();
builder.Services.AddSingleton<ONNXService>();
builder.Services.AddSyncfusionBlazor();

var app = builder.Build();
app.Services.GetService<ONNXService>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseExceptionHandler("/Error");
app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
