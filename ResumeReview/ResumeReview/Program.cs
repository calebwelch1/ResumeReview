using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using ResumeReview.Components;
using ResumeReview.Models;
using ResumeReview.Services;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection("AzureOpenAI"));
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;

    if (string.IsNullOrWhiteSpace(options.Endpoint) || string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.Deployment))
    {
        throw new InvalidOperationException("AzureOpenAI configuration is missing. Please set AzureOpenAI:Endpoint, AzureOpenAI:ApiKey, and AzureOpenAI:Deployment in configuration.");
    }

    // Use ApiKeyCredential instead of AzureKeyCredential
    return new AzureOpenAIClient(new Uri(options.Endpoint), new ApiKeyCredential(options.ApiKey));
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

app.MapPost("/api/chat", async (ChatRequest request, ChatService chatService, ILogger<Program> logger, CancellationToken cancellationToken) =>
{
    try
    {
        logger.LogInformation("Received chat request with {MessageCount} messages", request.Messages.Count);
        var response = await chatService.GetResponseAsync(request.Messages, cancellationToken);
        logger.LogInformation("Successfully generated response");
        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing chat request");
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "Chat Service Error"
        );
    }
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();