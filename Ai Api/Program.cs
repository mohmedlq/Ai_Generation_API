using DataAccess.Gemini_Ai_Api;
using DataAccess.Groq_Ai_Api;
using DataAccess.Services;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.Configure<GeminiOptions>(
    builder.Configuration.GetSection("Gemini"));
builder.Services.AddHttpClient<GroqService>();
builder.Services.Configure<GroqOptions>(
    builder.Configuration.GetSection("Groq"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Êﬁ»· app.UseAuthorization();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
