using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using dotnet_backend.Database;
using dotnet_backend.Services;
using dotnet_backend.Services.Interface;
using dotnet_backend.Models;
using dotnet_backend.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== TU?N 1: SETUP BACKEND (SQLite) =====

// 1. L?y chu?i k?t n?i t? appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. ??ng ký DbContext v?i SQLite (phù h?p cho Uno Platform - offline support)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// 3. ? c?u hình JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Secret"];
var key = Encoding.ASCII.GetBytes(secretKey ?? "");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 4. ??ng ký Controllers v?i JSON chu?n (PropertyNamingPolicy = null)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// 5. ??ng ký các Services (Business Logic Layer)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();

// ? 6. B?t CORS cho Uno Platform (iOS, Android, WebAssembly)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUnoApp",
        policy => policy
            .AllowAnyOrigin()      // Cho phép m?i ngu?n (iOS, Android, WebAssembly)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// ? 7. Thêm Swagger/OpenAPI ?? test API (Tu?n 1)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Uno Store Backend API",
        Version = "v1",
        Description = "API Backend cho Uno Store App - Tu?n 1: Setup & Xem Danh sách S?n ph?m"
    });

    // Thêm c?u hình JWT cho Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nh?p JWT token v?i format: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ===== SEED DATABASE FROM data.sql =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // ??m b?o database ???c t?o
        await context.Database.EnsureCreatedAsync();
        
        // Seed data t? data.sql (?ã convert sang C# code)
        await DataSqlSeeder.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// ===== MIDDLEWARE PIPELINE =====

// ? 8. Kích ho?t Swagger (Development mode)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Uno Store API v1");
        c.RoutePrefix = string.Empty; // Swagger UI t?i root URL
    });
}

// ? 9. Kích ho?t CORS
app.UseCors("AllowUnoApp");

// ? 10. ? Kích ho?t Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

// ? 11. Map Controllers
app.MapControllers();

app.Run();
