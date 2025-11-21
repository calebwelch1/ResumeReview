using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using ResumeReview.Models;

namespace ResumeReview.Services;

public class ChatService
{
    private readonly OpenAIClient _client;
    private readonly AzureOpenAIOptions _options;

    public ChatService(OpenAIClient client, IOptions<AzureOpenAIOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessageDto> messages, CancellationToken cancellationToken = default)
    {
        var chatMessages = BuildChatMessages(messages);
        var chatOptions = new ChatCompletionsOptions(_options.Deployment)
        {
            Temperature = 0.4f
        };

        foreach (var message in chatMessages)
        {
            chatOptions.Messages.Add(message);
        }

        var result = await _client.GetChatCompletionsAsync(chatOptions, cancellationToken);
        var completion = result.Value.Choices.FirstOrDefault()?.Message.Content ?? "I wasn't able to generate a response.";

        return new ChatResponse(completion);
    }

    private static List<ChatMessage> BuildChatMessages(IEnumerable<ChatMessageDto> messages)
    {
        var chatMessages = new List<ChatMessage>
        {
            new(ChatRole.System, "You are an AI assistant that helps with resume reviews and career guidance. Keep responses concise and actionable.")
        };

        foreach (var message in messages)
        {
            chatMessages.Add(new ChatMessage(RoleFromText(message.Role), message.Content));
        }

        return chatMessages;
    }

    private static ChatRole RoleFromText(string role) => role.ToLowerInvariant() switch
    {
        "assistant" => ChatRole.Assistant,
        "system" => ChatRole.System,
        _ => ChatRole.User
    };
}
