using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 0, 0))));

// Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT-konfig
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key mangler i appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Autorisasjonspolicy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Registrer JwtService for bruk i controller
builder.Services.AddScoped<JwtService>();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CV Management API", Version = "v1" });
});

var app = builder.Build();

// Middleware
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Opprett Admin-bruker hvis den ikke finnes
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        var newAdmin = new User
        {
            UserName = "admin",
            Email = "admin@example.com",
            Role = UserRole.Admin
        };

        var result = await userManager.CreateAsync(newAdmin, "Admin2025!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
            Console.WriteLine("Admin user created and added to role");
        }
        else
        {
            Console.WriteLine("Admin user creationn failed:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }
    else
    {
        Console.WriteLine("Admin user already exist.");
    }
}

// Kjør API
app.MapControllers();
app.Run();
