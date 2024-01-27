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
using Core.Account;
using Data.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
string ConnString = "production";


builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1440);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddEfRepositories(builder.Configuration.GetConnectionString(ConnString));
builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 4;   // минимальная длина
    opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
    opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
    opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
    opts.Password.RequireDigit = false; // требуются ли цифры
}).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();


builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = Int32.MaxValue;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(2000);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Home/AccesDenide";
    options.SlidingExpiration = true;
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


#region Servises
builder.Services.AddSingleton<PhotoService>();
builder.Services.AddSingleton<GroupService>();
builder.Services.AddSingleton<CourseService>();
builder.Services.AddSingleton<TeacherService>();
builder.Services.AddSingleton<StudentService>();
#endregion

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
