using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using ResumeReview.Models;

namespace ResumeReview.Services;

public class ChatService
{
    private readonly AzureOpenAIClient _client;
    private readonly AzureOpenAIOptions _options;

    public ChatService(AzureOpenAIClient client, IOptions<AzureOpenAIOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessageDto> messages, CancellationToken cancellationToken = default)
    {
        var chatClient = _client.GetChatClient(_options.Deployment);
        var chatMessages = BuildChatMessages(messages);
        var chatOptions = new ChatCompletionOptions
        {
            Temperature = 0.4f
        };

        var completionResult = await chatClient.CompleteChatAsync(chatMessages, chatOptions, cancellationToken);
        var completion = completionResult.Value.Content.FirstOrDefault()?.Text ?? "I wasn't able to generate a response.";

        return new ChatResponse(completion);
    }

    private static List<ChatMessage> BuildChatMessages(IEnumerable<ChatMessageDto> messages)
    {
        var chatMessages = new List<ChatMessage>
        {
            new SystemChatMessage("You are an AI assistant that helps with resume reviews and career guidance. Keep responses concise and actionable.")
        };

        foreach (var message in messages)
        {
            chatMessages.Add(RoleFromText(message.Role, message.Content));
        }

        return chatMessages;
    }

    private static ChatMessage RoleFromText(string role, string content) => role.ToLowerInvariant() switch
    {
        "assistant" => new AssistantChatMessage(content),
        "system" => new SystemChatMessage(content),
        _ => new UserChatMessage(content)
    };
}
