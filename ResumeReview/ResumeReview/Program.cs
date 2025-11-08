using ResumeReview.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Azure.Core;
using Azure.AI.Language.Conversations;

var builder = WebApplication.CreateBuilder(args);

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
}

var apiKey = "none";
builder.Services.AddHttpClient("CoinGeckoClient", client =>
{
    client.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
    client.DefaultRequestHeaders.Add("x-cg-pro-api-key", apiKey);
});

builder.Services.AddHttpClient();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
