using Microsoft.EntityFrameworkCore;
using DAL.Models;
using BLL.Services;
using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BLL.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<PizzashopContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IChangePasswordService, ChangePasswordService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<INavBarService, NavBarService>();
builder.Services.AddScoped<IUserListService, UserListService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 3;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5125",
            ValidAudience = "http://localhost:5125",

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test1232133454353533636gfhgfhxfdsfsdfsdfghgfhfghfghgfhfghfhfgh"))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline. 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


// dotnet ef dbcontext scaffold "Host=localhost;Database=pizzashop;Username=postgres;Password=Tatva@123" Npgsql.EntityFrameworkCore.PostgreSQL -o ../DAL/Models --context PizzaShopDbContext --context-dir ../DAL/DbContext -f 