# Gemini Prompt API

A reusable **ASP.NET Core Web API** that acts as a secure backend gateway for Google's Gemini API.

The project allows frontend and backend applications to send a prompt, define the expected response structure, and optionally control the maximum output size. The server handles communication with Gemini and returns the generated result to the client.

The main goal is to provide a simple and reusable AI backend that keeps the Gemini API key and integration logic on the server instead of exposing them to client applications.

---

## Overview

Many applications need AI-generated content, but allowing a frontend application to communicate directly with Gemini creates security and architectural problems.

This project solves that by placing an ASP.NET Core API between the client and Gemini:

```text
Client Application
       │
       │ HTTP Request
       ▼
┌──────────────────────┐
│   Gemini Prompt API  │
│                      │
│ ASP.NET Core Web API │
└──────────┬───────────┘
           │
           │ Server-side API Key
           ▼
┌──────────────────────┐
│      Gemini API      │
└──────────────────────┘
```

The client only communicates with the application's API.

The Gemini API key remains on the server and is never exposed to the frontend.

---

## What This API Does

The API accepts three main inputs:

* **Prompt** — instructions describing what the AI should generate.
* **Response Schema** — an object describing the expected structure of the generated response.
* **Maximum Output Tokens** — an optional limit for the generated content.

The server then:

1. Receives the request from the client.
2. Builds the request for Gemini.
3. Sends the request using the configured Gemini model.
4. Receives the generated response.
5. Returns the result to the client.

This makes the API reusable across different applications and use cases.

For example, the same backend can be used by applications that generate:

```text
School Reports
School Broadcasts
Content Summaries
Structured Data
AI Forms
Text Generation
Other Custom AI Features
```

without requiring each frontend application to implement Gemini integration separately.

---

## Key Features

* ASP.NET Core Web API
* Google Gemini API integration
* Reusable AI generation endpoint
* Dynamic prompts
* Custom response structures
* Optional output token limits
* Dependency Injection
* Service-layer architecture
* Strong separation between API and external AI integration
* Swagger / OpenAPI documentation
* CORS support for frontend applications
* Secure API key configuration
* No database required
* Designed for reuse across multiple client applications

---

## Architecture

The project is intentionally separated into an API layer and a service/data-access layer.

```text
┌───────────────────────────────┐
│       Client Application      │
│                               │
│   React / Web / Other Apps    │
└───────────────┬───────────────┘
                │
                │ HTTPS
                ▼
┌───────────────────────────────┐
│        ASP.NET Core API       │
│                               │
│  AiApiController              │
│  Request DTOs                 │
└───────────────┬───────────────┘
                │
                │ Dependency Injection
                ▼
┌───────────────────────────────┐
│         DataAccess            │
│                               │
│  IGeminiService               │
│  GeminiService                │
│  GeminiOptions                │
└───────────────┬───────────────┘
                │
                │ HTTPS + Private API Key
                ▼
┌───────────────────────────────┐
│          Gemini API           │
└───────────────────────────────┘
```

### Why this structure?

The controller is responsible for handling HTTP requests and responses.

The Gemini service is responsible for communicating with the external Gemini API.

Configuration such as the API key and selected model is kept in dedicated options rather than being hard-coded into the service.

This keeps responsibilities separated and makes the integration easier to maintain and reuse.

---

## Tech Stack

### Backend

* C#
* .NET 8
* ASP.NET Core Web API
* REST
* `HttpClient`
* Dependency Injection
* `Microsoft.Extensions.Options`
* `System.Text.Json`

### API Documentation

* Swagger
* OpenAPI

### AI

* Google Gemini API

### Deployment

* MonsterASP.NET
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

# API

## Generate AI Content

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

### Request Fields

| Field       | Type    | Required | Description                                                 |
| ----------- | ------- | -------: | ----------------------------------------------------------- |
| `prompt`    | string  |      Yes | Instructions describing the content Gemini should generate. |
| `schema`    | object  |       No | Defines the expected structure of the generated response.   |
| `maxTokens` | integer |       No | Optional maximum number of output tokens.                   |

---

## Example

A client can request structured content such as:

```json
{
  "prompt": "Create a short school broadcast about reading.",
  "schema": {
    "title": "string",
    "introduction": "string",
    "content": "string",
    "conclusion": "string"
  }
}
```

The API forwards the request to Gemini and returns the generated result in a structure matching the requested format.

Example response:

```json
{
  "title": "The Importance of Reading",
  "introduction": "Reading is one of the most valuable habits...",
  "content": "Books help us develop knowledge, language, and critical thinking...",
  "conclusion": "Let us make reading a daily habit."
}
```

The response structure is determined by the request, making the endpoint reusable for different AI-powered features.

---

# Configuration

The Gemini API key is never stored directly in the source code.

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

The values above are examples only.

For production environments, sensitive configuration should be provided through environment variables or the hosting provider's secret configuration.

### Never commit sensitive values such as:

```text
API keys
Passwords
Access tokens
Connection strings
Private certificates
```

to Git.

---

# Local Development

## Requirements

* .NET 8 SDK
* Visual Studio 2022 or another compatible IDE
* A valid Gemini API key

## Clone the Repository

```bash
git clone <repository-url>
cd GeminiPromptAPI
```

## Restore Dependencies

```bash
dotnet restore
```

## Run the API

```bash
dotnet run --project "Ai_Api/Ai_Api.csproj"
```

Once the application is running, Swagger can be opened from:

```text
/swagger
```

Swagger provides an interactive interface for testing the API without requiring a separate frontend application.

---

# Production Deployment

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

# Security

One of the main design goals of this project is keeping the Gemini API key on the server.

The frontend should communicate only with the application's API:

```text
React
  │
  │ POST /api/AiApi/generate
  ▼
ASP.NET Core API
  │
  │ Private Gemini API Key
  ▼
Gemini API
```

This prevents client applications from requiring access to the private Gemini credentials.

The API can therefore act as a centralized AI integration layer for multiple frontend applications.

---

# CORS

The API supports cross-origin requests so that browser-based frontend applications such as React can communicate with it.

For production environments, CORS should be restricted to trusted frontend origins rather than allowing unrestricted access.

---

# Design Goals

This project was built around a few core engineering goals:

### Reusability

The API is not tied to a single application or feature.

A frontend can send different prompts and response structures and reuse the same backend service.

### Separation of Concerns

HTTP handling and Gemini integration are separated into different layers.

```text
Controller
    ↓
Service
    ↓
Gemini API
```

This keeps external API communication out of the controller and makes the codebase easier to maintain.

### Secure Configuration

Secrets and environment-specific settings are kept outside the application source code.

### Frontend / Backend Separation

Client applications do not need to know how Gemini works internally.

They only need to know how to communicate with the REST endpoint.

---

# Potential Improvements

Possible future improvements include:

* Authentication and authorization
* Request validation
* Rate limiting
* Centralized exception handling
* Structured logging
* API versioning
* Response caching
* Usage monitoring
* Support for multiple Gemini models
* Request and response persistence
* Production-grade CORS policies

---

# Purpose

This project was created as a reusable AI backend service and as a practical implementation of:

* ASP.NET Core Web API development
* REST API design
* Service-layer architecture
* Dependency Injection
* External API integration
* Secure configuration management
* AI integration
* Frontend / backend separation
* Production deployment
