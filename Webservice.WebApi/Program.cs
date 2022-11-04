using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Implementations;
using Core.DomainServices.Services.Interfaces;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore;
using Infrastructure.EF;
using Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Webservice.WebApi.GraphQL;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AvansMealsDbContext>(options => options.UseSqlServer(connectionString));

var userConnectionString = builder.Configuration.GetConnectionString("Security");
builder.Services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(userConnectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SecurityDbContext>();

builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.ValidateIssuer = false;
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(builder.Configuration["BearerToken:Key"]));
});

// Add repositories to dependency container
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IMealboxRepository, MealboxRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICanteenRepository, CanteenRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();


// Add services to dependency container
builder.Services.AddScoped<IMealboxService, MealboxService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// Add graphql dependencies
builder.Services.AddScoped<Query>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGraphQLServer().AddQueryType<Query>().ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Access seed images 
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UsePlayground(new PlaygroundOptions
{
    QueryPath = "/api/v1/Mealboxes",
    Path = "/playground"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/api/v1/Mealboxes");

app.Run();
