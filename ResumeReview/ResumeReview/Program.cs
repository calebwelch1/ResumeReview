using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using ResumeReview.Components;
using ResumeReview.Models;
using ResumeReview.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection("AzureOpenAI"));
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;

    if (string.IsNullOrWhiteSpace(options.Endpoint) || string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.Deployment))
    {
        throw new InvalidOperationException("AzureOpenAI configuration is missing. Please set AzureOpenAI:Endpoint, AzureOpenAI:ApiKey, and AzureOpenAI:Deployment in configuration.");
    }

    return new OpenAIClient(new Uri(options.Endpoint), new AzureKeyCredential(options.ApiKey));
});

builder.Services.AddScoped<ChatService>();
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

app.MapPost("/api/chat", async (ChatRequest request, ChatService chatService, CancellationToken cancellationToken) =>
{
    var response = await chatService.GetResponseAsync(request.Messages, cancellationToken);
    return Results.Ok(response);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
