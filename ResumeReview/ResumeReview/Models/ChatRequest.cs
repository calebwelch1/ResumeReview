namespace ResumeReview.Models;

public class ChatRequest
{
    public List<ChatMessageDto> Messages { get; set; } = new();
}
