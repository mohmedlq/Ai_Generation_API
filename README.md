
````markdown
# Gemini Prompt API

A RESTful ASP.NET Core Web API that provides a structured interface for generating AI-powered content using Google's Gemini API.

The API accepts a user-defined prompt, JSON schema, and optional token limit, then forwards the request to Gemini and returns the generated structured JSON response.

---

## Overview

Gemini Prompt API is designed as a reusable backend service for applications that need dynamic AI-generated content without exposing the Gemini API key to the client.

The main idea is:

Client → ASP.NET Core API → Gemini API → ASP.NET Core API → Client

This architecture keeps the Gemini API key on the server and allows frontend applications such as React to consume the backend through a simple REST API.

---

## Features

- RESTful ASP.NET Core Web API
- Google Gemini API integration
- Dynamic prompt generation
- Dynamic JSON response schemas
- Optional maximum output token configuration
- Structured JSON responses
- Swagger / OpenAPI documentation
- Dependency Injection
- Strong separation between API and data/service layers
- CORS configuration for frontend applications
- Secure API key configuration through environment variables / User Secrets
- No database required

---

## Architecture

```text
Client Application
       │
       │ HTTP/HTTPS
       ▼
┌──────────────────────┐
│   ASP.NET Core API   │
│                      │
│   Ai_Api             │
└──────────┬───────────┘
           │
           │ Dependency Injection
           ▼
┌──────────────────────┐
│     DataAccess       │
│                      │
│    GeminiService     │
│    GeminiOptions     │
└──────────┬───────────┘
           │
           │ HTTPS + API Key
           ▼
┌──────────────────────┐
│      Gemini API      │
└──────────────────────┘
````

---

## Tech Stack

### Backend

* C#
* .NET 8
* ASP.NET Core Web API
* REST
* HttpClient
* Dependency Injection
* Microsoft.Extensions.Options
* System.Text.Json

### API Documentation

* Swagger
* OpenAPI

### AI

* Google Gemini API

### Deployment

* Somee.com
* GitHub

---

## Project Structure

```text
GeminiPromptAPI/
│
├── Ai_Api/
│   ├── Controllers/
│   │   └── AiApiController.cs
│   │
│   ├── Dtos/
│   │   └── AiGenerateRequest.cs
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── Ai_Api.csproj
│
├── DataAccess/
│   ├── Options/
│   │   └── GeminiOptions.cs
│   │
│   ├── Services/
│   │   ├── GeminiService.cs
│   │   └── IGeminiService.cs
│   │
│   └── DataAccess.csproj
│
├── Dockerfile
├── README.md
└── GeminiPromptAPI.sln
```

---

## API Endpoint

### Generate AI Content

```http
POST /api/AiApi/generate
```

### Request

```json
{
  "prompt": "Generate a short school broadcast.",
  "schema": {
    "title": "string",
    "content": "string"
  },
  "maxTokens": 500
}
```

### Parameters

| Parameter   | Type    | Required | Description                                          |
| ----------- | ------- | -------- | ---------------------------------------------------- |
| `prompt`    | string  | Yes      | Instructions sent to Gemini                          |
| `schema`    | object  | No       | JSON schema defining the expected response structure |
| `maxTokens` | integer | No       | Maximum number of output tokens                      |

---

## Example Response

```json
{
  "title": "Morning School Broadcast",
  "content": "Good morning everyone..."
}
```

The exact response structure depends on the schema supplied with the request.

---

## Configuration

The Gemini API key is **not stored in the source code**.

For local development, ASP.NET Core User Secrets can be used.

Example configuration:

```json
{
  "Gemini": {
    "ApiKey": "YOUR_API_KEY",
    "Model": "YOUR_MODEL"
  }
}
```

> The example above is for documentation only. Never commit a real API key to the repository.

For production environments, sensitive values should be configured using environment variables or the hosting provider's secret management system.

---

## Local Development

### Requirements

* .NET 8 SDK
* Visual Studio 2022 or another compatible IDE
* Gemini API access

### Run

Clone the repository and open the solution:

```bash
git clone <repository-url>
cd GeminiPromptAPI
```

Then run:

```bash
dotnet restore
dotnet run --project "Ai_Api/Ai_Api.csproj"
```

Swagger will be available through the local application URL:

```text
/swagger
```

---

## Production

The API is currently deployed using MonsterASP.NET.

### Production API

```text
https://geminipromptapi.runasp.net
```

### Swagger

```text
https://geminipromptapi.runasp.net/swagger
```

---

## Security

This project intentionally keeps the Gemini API key on the backend.

The frontend should **never** communicate directly with Gemini using the private API key.

Recommended architecture:

```text
React
  │
  │ POST /api/AiApi/generate
  ▼
ASP.NET Core API
  │
  │ Private Gemini API Key
  ▼
Gemini
```

Never commit:

```text
API keys
passwords
access tokens
connection strings
private certificates
```

to Git.

---

## CORS

The API supports cross-origin requests so that frontend applications such as React can communicate with the backend.

For production deployments, CORS should ideally be restricted to trusted frontend origins.

---

## Future Improvements

Potential improvements include:

* Authentication and authorization
* Rate limiting
* Request validation
* Centralized exception handling
* Structured logging
* API versioning
* Response caching
* Usage monitoring
* More Gemini models
* Request/response persistence
* Production-grade CORS policies

---

## Purpose

This project was built as a reusable AI backend service and as a practical demonstration of:

* ASP.NET Core Web API development
* RESTful API design
* Service-layer architecture
* Dependency Injection
* External API integration
* Secure configuration management
* AI integration
* Frontend/backend separation

---

## License

This project is currently private and intended for personal development However you can mock it and add your personal Api Key.

