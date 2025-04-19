var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());



app.MapGet("/", () => "Chatbot Gateway API is running.");

app.MapPost("/chat", (HttpContext context) =>
{
    return Results.Ok(new
    {
        answer = "Mock response",
        citations = Array.Empty<object>()
    });
})
.WithOpenApi();

app.Run();
