using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineShoppingPlatform.Business.DataProtection;
using OnlineShoppingPlatform.Business.Operations.Order;
using OnlineShoppingPlatform.Business.Operations.Product;
using OnlineShoppingPlatform.Business.Operations.Setting;
using OnlineShoppingPlatform.Business.Operations.User;
using OnlineShoppingPlatform.Data.Context;
using OnlineShoppingPlatform.Data.Repository;
using OnlineShoppingPlatform.Data.UnitOfWork;
using OnlineShoppingPlatform.WebApi.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Swagger/OpenAPI setup to allow API documentation and testing in development environment
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme // JWT Security scheme definition for Swagger UI
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer Token on TexBox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});
// Registering data protection services
builder.Services.AddScoped<IDataProtection, DataProtecion>();
// Define directory for storing keys used in data protection
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"));
// Configure data protection for the application
builder.Services.AddDataProtection()
                .SetApplicationName("ShoppingPlatform")
                .PersistKeysToFileSystem(keysDirectory);
// Configure JWT authentication with token validation parameters
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                    };
                });

// Retrieve connection string from configuration
var cs = builder.Configuration.GetConnectionString("default");

builder.Services.AddDbContext<ShoppingPlatformDbContext>(options => options.UseSqlServer(cs));
// Register dependencies for repository and unit of work
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Register service dependencies for User, Product, Order, and Setting operations
builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddScoped<IProductService, ProductManager>();

builder.Services.AddScoped<IOrderService, OrderManager>();

builder.Services.AddScoped<ISettingService, SettingManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMaintenanceMode(); // Use maintenance mode middleware to control API availability during maintenance
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
