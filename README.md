# Universal AI Prompt API

A reusable **ASP.NET Core Web API** that provides a secure backend gateway for multiple AI providers such as **Google Gemini and Groq**.

The API allows client applications to send a prompt, response schema, and generation settings without exposing AI provider API keys to the frontend.

## Architecture

```text
Client Application
       │
       ▼
ASP.NET Core Web API
       │
       ├── Gemini
       ├── Groq
       └── Other AI Providers
```

## Features

* ASP.NET Core Web API
* Gemini integration
* Groq integration
* Structured JSON generation
* Custom prompts and schemas
* Token and temperature control
* Dependency Injection
* Service-layer architecture
* Secure API key configuration
* Swagger / OpenAPI
* CORS support

## API

### Gemini

```http
POST /api/AiApi/generate/Gemini
```

### Groq

```http
POST /api/AiApi/generate/Groq
```

### Request

```json
{
  "prompt": "Create a short school broadcast.",
  "schema": {
    "title": "string",
    "content": "string"
  },
  "tokens": 1200,
  "temperature": 0.2
}
```

## Project Structure

```text
UniversalAI/
├── Ai_Api/
│   ├── Controllers/
│   ├── Dtos/
│   └── Program.cs
│
├── DataAccess/
│   ├── Options/
│   └── Services/
│
├── Dockerfile
├── README.md
└── UniversalAI.sln
```

## Tech Stack

* C#
* .NET 8
* ASP.NET Core Web API
* HttpClient
* Dependency Injection
* System.Text.Json
* Swagger / OpenAPI

## Security

AI provider API keys are stored on the server and should never be exposed in client applications or committed to Git.

Use **User Secrets** for local development and environment/hosting secrets for production.

## Deployment

Production deployment can be hosted on any compatible ASP.NET hosting provider.

## Purpose

A reusable AI backend that allows applications to integrate multiple AI providers through a single API.
