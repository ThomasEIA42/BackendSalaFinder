using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Sala Finder API");
    options.WithTheme(ScalarTheme.DeepSpace);
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();