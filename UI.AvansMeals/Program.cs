using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Implementations;
using Core.DomainServices.Services.Interfaces;
using Infrastructure.EF;
using Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add database contexts to the server
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AvansMealsDbContext>(options => options.UseSqlServer(connectionString));

var userConnectionString = builder.Configuration.GetConnectionString("Security");
builder.Services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(userConnectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SecurityDbContext>();

// Add policies to set authorization boundries
builder.Services.AddAuthorization(options =>
    options.AddPolicy("KM", policy => policy.RequireClaim("KantineMedewerker")));

builder.Services.AddAuthorization(options =>
    options.AddPolicy("S", policy => policy.RequireClaim("Student")));

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

// Set paths for account functionality
builder.Services.Configure<CookieAuthenticationOptions>(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "addproducttocategory",
    pattern: "Category/{id}/AddProduct",
    defaults: new { controller = "Category", action = "AddProduct" });

app.MapControllerRoute(
    name: "deleteproductfromcategory",
    pattern: "Category/{categoryId}/DeleteProduct/{productId}",
    defaults: new { controller = "Category", action = "DeleteProduct" });

app.Run();
