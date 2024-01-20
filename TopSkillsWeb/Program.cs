using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TopSkillsWeb.Models;
using Core;
using Data.Repository;
using Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
// Add services to the container.
builder.Services.AddControllersWithViews();

string ConnString = "production";

builder.Services.AddEfRepositories(builder.Configuration.GetConnectionString(ConnString));
builder.Services.AddDbContext<ApplicationContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString(ConnString));
                } 
                , ServiceLifetime.Transient
                );
builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 4;   // ����������� �����
    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    opts.Password.RequireDigit = false; // ��������� �� �����
}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = Int32.MaxValue;
});
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(2000);
    options.LoginPath = "/Authorization/Login";
    options.AccessDeniedPath = "/Home/AccesDenide";
    options.SlidingExpiration = true;
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1440);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var SuppotredCultures = new[]
    {
        new CultureInfo("ru"),
        new CultureInfo("en")
    };

    options.RequestCultureProviders = new[] { new CookieRequestCultureProvider() };
    options.DefaultRequestCulture = new RequestCulture("ru");
    options.SupportedCultures = SuppotredCultures;
    options.SupportedUICultures = SuppotredCultures;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);



app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
