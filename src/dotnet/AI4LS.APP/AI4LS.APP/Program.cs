using AI4LS.APP.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);


Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["syncfusionkey"]);


builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = true;
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
    options.MaxBufferedUnacknowledgedRenderBatches = 10;
});
builder.Services.AddSingleton<CountriesService>();
builder.Services.AddSingleton<LucasData2018Service>();
builder.Services.AddSingleton<ONNXService>();
builder.Services.AddServerSideBlazor().AddHubOptions(o => { o.MaximumReceiveMessageSize = 102400000; });

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
