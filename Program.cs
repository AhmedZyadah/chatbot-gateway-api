using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
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

var fastApiUrl = builder.Configuration["FastApiUrl"];

app.MapGet("/", () => "Chatbot Gateway API is running.");

app.MapPost("/ingest", async (HttpContext httpContext) =>
{
    if (!httpContext.Request.HasFormContentType)
        return Results.BadRequest(new { error = "Content-Type must be multipart/form-data." });

    var form = await httpContext.Request.ReadFormAsync();
    var file = form.Files.GetFile("file");

    if (file == null)
        return Results.BadRequest(new { error = "No file uploaded." });

    var responseBody = string.Empty;

    // Read file into memory stream
    using (var memoryStream = new MemoryStream())
    {
        await file.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        using (var content = new MultipartFormDataContent())
        {
            var fileContent = new StreamContent(memoryStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            content.Add(fileContent, "file", file.FileName);

            // send the request to FastAPI
            var httpClient = httpContext.RequestServices.GetRequiredService<HttpClient>();
            var response = await httpClient.PostAsync($"{fastApiUrl}/ingest", content);
            responseBody = await response.Content.ReadAsStringAsync();
        }
    }

    // Send request to FastAPI
    return Results.Content(responseBody, "application/json");
})
.WithOpenApi();


app.MapPost("/query", async (HttpContext httpContext) =>
{
    try
    {
        var json = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // send the request to FastAPI
        var httpClient = httpContext.RequestServices.GetRequiredService<HttpClient>();
        var response = await httpClient.PostAsync($"{fastApiUrl}/query", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        return Results.Content(responseBody, "application/json");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Something went wrong: {ex.Message}");
    }
})
.WithOpenApi();


app.Run();
