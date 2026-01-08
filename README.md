# AI Resume Strategist

A production-ready AI-powered career coaching application built with Azure OpenAI and Blazor Server, designed to provide personalized resume feedback, interview preparation, and career guidance through intelligent conversational AI.

## 🌐 Live Demo

**🚀 Deployed Application:** [https://resume-strategist-app.azurewebsites.net](https://rg-resume-strategist-hmepdehzgud9bdb5.canadaeast-01.azurewebsites.net/)

Hosted on **Azure App Service** with continuous deployment via GitHub Actions.

## 🎯 Project Overview

This application demonstrates enterprise-level Azure AI implementation, showcasing skills in cloud AI architecture, secure API integration, and modern web development practices. Built as part of my Azure AI Engineer certification journey, this project implements best practices for deploying and managing Azure Cognitive Services in a real-world application.

## 🏗️ Architecture

### Technology Stack

**Frontend**
- **Blazor Server (.NET 8)** - Interactive server-side rendering with real-time SignalR communication
- **Bootstrap 5** - Responsive UI framework
- **Custom CSS** - Modern, professional design system

**Backend & AI**
- **Azure OpenAI Service** - GPT model deployment for natural language understanding
- **Azure Cognitive Services** - Secure, scalable AI infrastructure
- **ASP.NET Core Minimal APIs** - Lightweight, performant API endpoints

**Infrastructure**
- **Azure Cloud Platform** - Enterprise-grade hosting and compute
- **Azure Key Vault** (production) - Secure credential management
- **GitHub Actions** - CI/CD pipeline for automated deployments
- **Application Insights** (production) - Monitoring and diagnostics

### System Architecture

```
┌─────────────────┐
│   User Client   │
│  (Browser UI)   │
└────────┬────────┘
         │ SignalR/HTTP
         ▼
┌─────────────────┐
│  Blazor Server  │
│   Application   │
└────────┬────────┘
         │ Internal API
         ▼
┌─────────────────┐
│   Chat Service  │
│  (C# Business   │
│     Logic)      │
└────────┬────────┘
         │ Azure SDK
         ▼
┌─────────────────┐
│  Azure OpenAI   │
│    Service      │
│ (GPT Model API) │
└─────────────────┘
```

## 🧠 Azure AI Implementation

### Custom Model Configuration

This project utilizes a custom-configured Azure OpenAI deployment with:

- **Model**: GPT-4 / GPT-3.5-Turbo (configurable)
- **Deployment Name**: `ResumeReviewBot`
- **Fine-tuned Parameters**:
  - Temperature: 0.4 (balanced creativity and consistency)
  - Max Tokens: 800 (optimized for detailed responses)
  - System Prompt Engineering: Career coaching expertise injection

### Azure OpenAI Studio Configuration

The model was deployed and configured through Azure OpenAI Studio with:

1. **Resource Provisioning**: Created dedicated Azure OpenAI resource in supported region
2. **Model Deployment**: Deployed GPT model with custom deployment configuration
3. **Prompt Engineering**: Implemented system-level prompts for domain expertise
4. **Rate Limiting**: Configured Tokens Per Minute (TPM) allocation
5. **Content Filtering**: Applied Azure content safety policies

### Security & Compliance

**API Key Management**
- Secrets stored in `appsettings.json` (development)
- User Secrets integration for local development
- Azure Key Vault integration ready for production deployment
- Never committed to version control (`.gitignore` configured)

**Request Authentication**
- Azure Key Credential-based authentication
- HTTPS enforcement for all API communications
- CORS policies configured for production domains

**Responsible AI**
- Content moderation through Azure's built-in filters
- User privacy protection (no data stored in Azure OpenAI)
- Transparent AI interaction disclosure

## 💡 Key Features

### Intelligent Career Guidance
- Real-time conversational AI interface
- Context-aware responses based on conversation history
- Actionable, specific advice tailored to user needs

### Resume Analysis
- Bullet point optimization suggestions
- Quantification recommendations for achievements
- Industry-specific terminology alignment
- ATS (Applicant Tracking System) optimization tips

### Interview Preparation
- Custom interview question generation
- Role-specific preparation guidance
- Behavioral question coaching
- Flash card creation for practice

### User Experience
- Clean, professional interface with modern design
- Real-time response streaming
- Error handling with user-friendly messages
- Loading states and progress indicators
- Mobile-responsive design

## 🔧 Technical Implementation

### Azure OpenAI Client Configuration

```csharp
// Dependency injection setup with options pattern
builder.Services.Configure<AzureOpenAIOptions>(
    builder.Configuration.GetSection("AzureOpenAI")
);

// Singleton Azure OpenAI client for connection pooling
builder.Services.AddSingleton<AzureOpenAIClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;
    return new AzureOpenAIClient(
        new Uri(options.Endpoint), 
        new ApiKeyCredential(options.ApiKey)
    );
});
```

### Chat Service Implementation

The `ChatService` class encapsulates Azure OpenAI interactions:

- **Conversation Management**: Maintains chat history with role-based messages
- **System Prompt Engineering**: Injects career coaching expertise
- **Error Handling**: Graceful degradation with informative error messages
- **Async/Await Pattern**: Non-blocking operations for optimal performance

### API Endpoint Design

RESTful API endpoint with comprehensive logging:

```csharp
app.MapPost("/api/chat", async (
    ChatRequest request, 
    ChatService chatService, 
    ILogger<Program> logger, 
    CancellationToken cancellationToken) =>
{
    // Request validation, logging, and response handling
});
```

## 📊 Azure AI Engineer Certification Skills Demonstrated

This project showcases competencies aligned with the **Azure AI Engineer Associate (AI-102)** certification:

### Design and Implement Azure AI Solutions
✅ Plan and manage Azure Cognitive Services deployments  
✅ Implement Azure OpenAI Service solutions  
✅ Configure model deployments with appropriate parameters  

### Integrate AI into Applications
✅ Integrate Azure OpenAI APIs into applications  
✅ Implement conversation flow and context management  
✅ Handle API responses and error conditions  

### Secure AI Solutions
✅ Manage API keys and authentication credentials  
✅ Implement secure communication protocols  
✅ Apply responsible AI principles  

### Monitor and Optimize AI Solutions
✅ Implement logging and telemetry  
✅ Configure performance parameters (temperature, tokens)  
✅ Optimize prompt engineering for quality responses  

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- Azure subscription with OpenAI service access
- Visual Studio 2022 or VS Code
- Azure OpenAI resource provisioned

### Configuration

1. **Clone the repository**
```bash
git clone <repository-url>
cd ResumeReview
```

2. **Configure Azure OpenAI settings**

Create `appsettings.Development.json`:
```json
{
  "AzureOpenAI": {
    "Endpoint": "https://YOUR-RESOURCE.cognitiveservices.azure.com/",
    "ApiKey": "YOUR-API-KEY",
    "Deployment": "YOUR-DEPLOYMENT-NAME"
  }
}
```

Or use User Secrets (recommended):
```bash
dotnet user-secrets init
dotnet user-secrets set "AzureOpenAI:ApiKey" "YOUR-API-KEY"
dotnet user-secrets set "AzureOpenAI:Endpoint" "YOUR-ENDPOINT"
dotnet user-secrets set "AzureOpenAI:Deployment" "YOUR-DEPLOYMENT"
```

3. **Install dependencies**
```bash
dotnet restore
```

4. **Run the application**
```bash
dotnet run
```

Navigate to `https://localhost:7XXX` (check console for exact port)

## 📦 Dependencies

```xml
<PackageReference Include="Azure.AI.OpenAI" Version="2.1.0" />
<PackageReference Include="System.ClientModel" Version="1.1.0" />
<PackageReference Include="Microsoft.Extensions.Http" Version="8.0.0" />
```

## 🎓 Learning Outcomes

Through building this project, I developed expertise in:

- **Azure Cloud Architecture**: Resource provisioning, configuration, and management
- **AI Model Deployment**: Understanding model parameters, quotas, and optimization
- **Prompt Engineering**: Crafting effective system prompts for domain-specific AI
- **Secure API Integration**: Managing credentials, implementing authentication
- **Production-Ready Code**: Error handling, logging, monitoring best practices
- **Modern Web Development**: Blazor Server, SignalR, real-time communication

## 🔐 Security Considerations

- API keys never committed to source control
- `.gitignore` configured for sensitive files
- HTTPS enforced for all communications
- Input sanitization implemented
- Rate limiting considerations documented
- Azure Key Vault integration path prepared

## 📈 Future Enhancements

- **Resume Document Upload**: Parse PDF/DOCX resumes using Azure Document Intelligence
- **LinkedIn Profile Analysis**: Integration with LinkedIn API for profile optimization
- **Job Description Matching**: Semantic similarity analysis using Azure Cognitive Search
- **Interview Simulation**: Voice-enabled mock interviews with Azure Speech Services
- **Progress Tracking**: User dashboard with improvement metrics
- **Multi-language Support**: Azure Translator integration for global reach

## 📄 License

This is a portfolio project demonstrating Azure AI capabilities.

## 👤 Contact

**Caleb Welch**  
Azure AI Engineer | Full Stack Developer  
[LinkedIn](https://www.linkedin.com/in/caleb-welch-502851121/) | [Portfolio](https://calebwelch1.github.io/portfolio/) | [GitHub](https://github.com/calebwelch1)

---

*This project demonstrates practical Azure AI implementation aligned with Azure AI Engineer Associate certification competencies. Built with modern best practices for enterprise-grade AI applications.*