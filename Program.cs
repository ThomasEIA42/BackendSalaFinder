using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalaFinder.DAO;
using SalaFinder.Services;
using SalaFinder.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ✅ Registrar el DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Registrar Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ✅ Registrar todos los servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<INoShowService, NoShowService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ISpaceService, SpaceService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Sala Finder API");
    options.WithTheme(ScalarTheme.DeepSpace);
});

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();