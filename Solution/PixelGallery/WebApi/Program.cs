using Microsoft.OpenApi.Models;

using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PixelGallery", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi.xml"));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddPersistence(connectionString);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();
app.MapControllers();

app.Run();

/// <summary>
/// necessary for unit tests
/// </summary>
public partial class Program
{
}
